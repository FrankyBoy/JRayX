package jray.ray;

import java.awt.Dimension;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;

import javax.swing.JComponent;
import javax.swing.JFrame;

import jray.ray.scenes.*;


/**
 * Frame showing a Scene
 *
 */
public class GUI extends JFrame {

    private static final long serialVersionUID = -8613076431270950685L;
    private Renderer renderer;
    private JComponent drawComponent;
    private boolean repainting;

    public GUI(Renderer renderer, boolean repaint) {
        super("JRay");

        this.renderer = renderer;
        this.drawComponent = this.renderer.getDrawComponent();
        this.drawComponent.setPreferredSize(new Dimension(200, 200));
        this.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        this.addWindowListener(new WindowAdapter() {

            @Override
            public void windowClosing(WindowEvent e) {
                GUI.this.repainting = false;
                GUI.this.renderer.shutdown();
            }
        });
        this.getContentPane().add(drawComponent);
        this.setLocation(0, 0);
        this.pack();

        if (repaint) {
            repainting = true;
            new Thread(new Runnable() {

                public void run() {
                    repaintLoop();
                }
            }).start();
        }
    }

    /**
     * Repaints the scene in an endless loop
     */
    private void repaintLoop() {
        long lastUpdate = System.nanoTime();

        while (repainting) {
            //repaint
            drawComponent.paint(drawComponent.getGraphics());

            //draw FPS-count every second
            long now = System.nanoTime();
            if (lastUpdate + 1000000000 < now) {
                setTitle("JRay " + String.format("%6.2f", renderer.getFrameRate()) + " fps");
                System.out.println(String.format("%6.2f", renderer.getFrameRate()) + " fps");
                lastUpdate = now;
            }
        }
    }

    public static void main(String[] args) {
        Scene s;
        try {
        	//s = new KugelTest();
        	s = new RandomForest();
        	//s = new MeshTest();
        } catch (Exception ioe) {
            ioe.printStackTrace();
            return;
        }

        Renderer r = new Renderer(s, 1 + (int) (Runtime.getRuntime().availableProcessors() * 1.5));

        new GUI(r, true).setVisible(true);
    }
}
