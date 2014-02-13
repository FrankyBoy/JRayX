package jray.ray;

import java.awt.Component;
import java.awt.Graphics;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;
import java.awt.image.BufferedImage;
import java.awt.image.DataBufferInt;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.TimeUnit;

import javax.swing.JComponent;

import jray.common.Ray;
import jray.common.Vect3;
import jray.math.Vect;
import jray.math.intersections.LongColors;
import jray.ray.tracer.BackwardRayTracer;
import jray.ray.tracer.BackwardRayTracerHQ;


/**
 * Renders a Scene to an image as seen by a specified Camera. 
 * <p>
 * To utilize the full power of multiple CPU-cores, this class maintains a
 * thread pool and splits up the image into many horizontal stripes. The total count of
 * image-parts is a multiple of the thread count (default is <code>threadCount * 4</code>).
 */
public class Renderer {

    /**
     * Count of image stripes
     */
    private final int splitCount;
    /**
     * Count of threads in the thread pool
     */
    private final int threadCount;
    /**
     * The scene the renderer is looking at.
     */
    private Scene scene;
    /**
     * RayTraceLogic objects assigned to its thread. This map is exclusively accessed by the
     * method getLogic() which returns the RayTraceLogic accordingly to the thread calling it.
     * This method additionally creates RayTraceLogic objects and stores them in this field
     * if a thread uses it the first time.
     */
    private Map<Thread, BackwardRayTracer> logics;
    /**
     * An ExecutorService wrapping a thread pool, which consists of <code>threadCount</code> threads.
     */
    private ExecutorService service;
    /**
     * An array containing a task for each part of the image. Each task knows the id of it's image part and
     * is reused for each redraw of the image.
     */
    private Callable<?>[] tasks;
    /**
     * An array containing the futures of the tasks - they are not used to retrieve return values but to
     * wait for finished tasks.
     */
    private Future<?>[] futures;
    /**
     * The width of the image in pixel
     */
    private int widthPx;
    /**
     * The height of the image in pixel
     */
    private int heightPx;
    /**
     * The content of the image with one integer per pixel
     */
    private int[] ibuf;
    private long[] lbuf;
    /**
     * Factor used to scale the brightness of the resulting image
     */
    private double brigthnessScale = 1.0;
    /**
     * The framerate - used to measure performance
     */
    private double frameRate = 0;

    /**
     * Creates a new renderer.
     * 
     * @param scene the scene
     * @param threadCount size of the thread pool
     */
    public Renderer(Scene scene, int threadCount) {
        this.threadCount = threadCount;
        this.splitCount = threadCount * 4;

        System.out.println("Initializing Renderer...");
        System.out.println(" - Object Count:            " + scene.getObjects().size());
        System.out.println(" - Thread Count:            " + threadCount);
        System.out.println(" - Image Parts:             " + splitCount);

        System.out.print(" - ThreadPool:              ");
        service = Executors.newFixedThreadPool(this.threadCount);
        System.out.println("Launched");

        System.out.print(" - Callables and Futures:   ");
        tasks = new Callable<?>[splitCount];
        futures = new Future<?>[splitCount];

        int splits = splitCount;
        for (int i = 0; i < splits; i++) {
            final int id = i;

            tasks[i] = new Callable<Object>() {

                @Override
                public Object call() throws Exception {
                    renderImagePart(id);
                    return null;
                }
            };
        }
        System.out.println("Ready");
        setScene(scene);

        System.out.println("Renderer initialised successfully - READY FOR RUN");
    }

    /**
     * Initializes the Renderer using a scene and a camera
     * 
     * @param scene the scene
     */
    public synchronized void setScene(Scene scene) {
        System.out.println("Initializing Scene: " + scene.getName());
        this.scene = scene;
        logics = Collections.synchronizedMap(new HashMap<Thread, BackwardRayTracer>(threadCount));
    }

    /**
     * Gets the RayTraceLogic for the current thread. 
     * @return the RayTraceLogic for the current thread.
     */
    private BackwardRayTracer getLogic() {
        Thread me = Thread.currentThread();
        BackwardRayTracer ret = logics.get(me);

        if (ret == null) {//create one if it does not already exist
            ret = new BackwardRayTracerHQ(scene);
            logics.put(me, ret);
        }

        return ret;
    }

    /**
     * Renders the scene to an image
     *
     * @param image the image where data is stored
     */
    private synchronized void renderImage(BufferedImage image) {
        if (Renderer.this.service.isShutdown()) {
            throw new RuntimeException("This renderer must no longer be used as it has been shut down!");
        }

        try {
            if (lbuf == null || widthPx != image.getWidth() || heightPx != image.getHeight()) {
                lbuf = new long[(image.getWidth()) * (image.getHeight())];
            }

            widthPx = image.getWidth();
            heightPx = image.getHeight();

            ibuf = ((DataBufferInt) image.getRaster().getDataBuffer()).getData();

            for (int i = 0; i < futures.length; i++) {
                futures[i] = service.submit(tasks[i]);
            }

            for (Future<?> f : futures) {
                f.get();
            }

            // search max color value in whole picture
            int max = 255;
            for (int i = 0; i < lbuf.length; i++) {
                int m = LongColors.getMax(lbuf[i]);
                if (m > max) {
                    max = m;
                }
            }
            double scale = 255.0 / max * brigthnessScale;

            for(int i = 0, j = 0;j < heightPx; i++) {
                long color = lbuf[(j) * (widthPx) + i];

                ibuf[j * widthPx + i] = LongColors.toInt(LongColors.scale(color, scale));

                if(i == widthPx - 1) {
                    ++j;
                    i = -1;
                }
            }

        } catch (Exception e) {
            System.out.println("Exception in renderImage:");
            e.printStackTrace(System.out);
            System.exit(1);
        }
    }

    /**
     * Renders the part <code>id</code> of <code>splitCount</code> of the image to ibuf.
     * 
     * @param id a number from <code>0</code> to <code>splitCount-1</code>
     */
    private void renderImagePart(int id) {
        int slotHeight = heightPx / splitCount;
        int from = slotHeight * id;
        int to = slotHeight * (id + 1);
        if (id == splitCount - 1) {
            to = heightPx;
        }

        BackwardRayTracer logic = getLogic();
        Vect3 rayDirection = new Vect3();
        Camera camera = scene.getCamera();

        Ray ray = new Ray(new Vect3(camera.getPosition()), rayDirection);

        Vect3 vertAdd = new Vect3(camera.getViewPaneHeightVector());
        Vect.scale(vertAdd, 1.0 / (heightPx - 1), vertAdd);
        Vect3 horzAdd = new Vect3(camera.getViewPaneWidthVector());
        Vect.scale(horzAdd, 1.0 / (widthPx - 1), horzAdd);

        for (int i = from; i < to; i++) {
            for (int j = 0; j < widthPx; j++) {
            	
            	Vect.subtract(camera.getViewPaneEdge(), ray.getOrigin(), rayDirection);
            	Vect.addMultiple(rayDirection, vertAdd, i, rayDirection);
            	Vect.addMultiple(rayDirection, horzAdd, j, rayDirection);
                
                rayDirection.normalize();

                long color = logic.shoot(ray);

                lbuf[i * widthPx + j] = color;
            }
        }
    }

    /**
     * Shuts down this Renderer. The thread pool will be terminated and the components generated by this instance
     * will throw exceptions upon invocation. 
     */
    public synchronized void shutdown() {
        try {
            System.out.println("Shutting down Renderer...");
            service.shutdown();
            System.out.println("Waiting for ThreadPool Shutdown...");
            service.awaitTermination(1, TimeUnit.DAYS);
            System.out.println("Renderer shutdown complete!");
        } catch (InterruptedException t) {
        }
    }

    /**
     * Creates and returns a component which can be added to a frame. This component will be updated using the image the renderer sees.
     *
     * @return the "window" to the scene
     */
    public JComponent getDrawComponent() {
        final ExtJComponent ret = new ExtJComponent();

        /**
         * Checks if the component has been resized and updates the size of the BufferdImage accordingly
         */
        ret.addComponentListener(new ComponentAdapter() {

            @Override
            public void componentResized(ComponentEvent e) {
                Component c = e.getComponent();
                ret.updateImageDimensions();
                scene.getCamera().setScreenDimensions(c.getWidth(), c.getHeight());
            }
        });

        return ret;
    }

    private final class ExtJComponent extends JComponent {

        private static final long serialVersionUID = 1L;
        BufferedImage image = null;
        private int widthPx = -1, heightPx = -1;

        private ExtJComponent() {
            updateImageDimensions();
        }

        @Override
        public synchronized void paintComponent(Graphics g) {
            super.paintComponent(g);

            long startNanos = System.nanoTime();
            // Render image and update Component
            Renderer.this.renderImage(image);
            g.drawImage(image, 0, 0, null);
            frameRate = 1000000000.0 / (System.nanoTime() - startNanos);
        }

        public synchronized void updateImageDimensions() {
            widthPx = this.getWidth();
            heightPx = this.getHeight();
            image = new BufferedImage((widthPx > 0 ? widthPx : 1), (heightPx > 0 ? heightPx : 1), BufferedImage.TYPE_INT_ARGB);
        }
    }

    public double getBrigthnessScale() {
        return brigthnessScale;
    }

    public void setBrigthnessScale(double brigthnessScale) {
        this.brigthnessScale = brigthnessScale;
    }

    public double getFrameRate() {
        return frameRate;
    }
}
