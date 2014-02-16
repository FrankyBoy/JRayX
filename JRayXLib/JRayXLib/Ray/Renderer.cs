using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Common.Logging;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Ray.Tracer;
using JRayXLib.Shapes;

namespace JRayXLib.Ray
{
    public class Renderer
    {
        private ConcurrentDictionary<Thread, BackwardRayTracer> _logics;
        private int _widthPx;
        private int _heightPx;
        private WideColor[,] _lbuf;

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private Scene _scene;
        private int _splitMultiplier;
        private int _threadCount;

        #region Public Vars
        public int SplitMultiplier
        {
            get { return _splitMultiplier; }
            set {
                _splitMultiplier = value;
                UpdateMaxThreads();
            }
        }

        public int ThreadCount
        {
            get { return _threadCount; }
            set
            {
                _threadCount = value;
                UpdateMaxThreads();
            }
        }

        public Scene Scene {
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                Log.Info("Initializing Scene: " + value.GetName());
                _scene = value;
                _logics = new ConcurrentDictionary<Thread, BackwardRayTracer>();
            }
        }

        public double BrigthnessScale { get; set; }
        #endregion


        public Renderer()
        {
            ThreadCount = Environment.ProcessorCount;
            SplitMultiplier = 4;
            BrigthnessScale = 1.0;
        }

        private BackwardRayTracer GetLogic()
        {
            Thread me = Thread.CurrentThread;
            return _logics.GetOrAdd(me, x => new BackwardRayTracerHQ(_scene));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RenderImage(Texture image)
        {
            try
            {
                if (_lbuf == null || _widthPx != image.Width || _heightPx != image.Height)
                {
                    _lbuf = new WideColor[image.Width, image.Height];
                    _widthPx = image.Width;
                    _heightPx = image.Height;
                    _scene.Camera.SetScreenDimensions(_widthPx, _heightPx);
                }

                var max = ExecuteTasks();
                
                double scale = 255.0/max*BrigthnessScale;

                for (int i = 0; i < _heightPx; i++)
                {
                    for (int j = 0; j < _widthPx; j++)
                    {
                        WideColor color = _lbuf[i,j];

                        image[i, j] = (color * scale).To8Bit();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Info("Exception in renderImage:");
                Log.Info(e);
                Shutdown = true;
            }
        }

        private int ExecuteTasks()
        {
            var splitCount = _threadCount*SplitMultiplier;
            int toProcess = splitCount;
            var localMaxBrightness = new int[splitCount];
            using (var resetEvent = new ManualResetEvent(false))
            {
                for (int i = 0; i < splitCount; i++)
                {
                    int id = i;
                    ThreadPool.QueueUserWorkItem(x =>
                        {
                            localMaxBrightness[id] = RenderImagePart(id);
                            if (Interlocked.Decrement(ref toProcess) == 0)
                                resetEvent.Set();
                        });
                }

                resetEvent.WaitOne();
            }
            return localMaxBrightness.Max();
        }

        protected bool Shutdown { get; set; }

        /**
         * Renders the part <code>id</code> of <code>splitCount</code> of the image to ibuf.
         * 
         * @param id a number from <code>0</code> to <code>splitCount-1</code>
         */

        private int RenderImagePart(object idObj)
        {
            var splitCount = _threadCount * SplitMultiplier;
            var id = (int) idObj;
            int slotHeight = _heightPx / splitCount;
            int from = slotHeight*id;
            int to = slotHeight*(id + 1);
            if (id == splitCount - 1)
            {
                to = _heightPx;
            }

            BackwardRayTracer logic = GetLogic();
            var rayDirection = new Vect3();
            Camera camera = _scene.Camera;

            var ray = new Shapes.Ray(new Vect3(camera.GetPosition()), rayDirection);

            var vertAdd = new Vect3(camera.GetViewPaneHeightVector());
            Vect.Scale(vertAdd, 1.0/(_heightPx - 1), vertAdd);
            var horzAdd = new Vect3(camera.GetViewPaneWidthVector());
            Vect.Scale(horzAdd, 1.0/(_widthPx - 1), horzAdd);

            var localMaxBrightness = 0;
            for (int i = from; i < to; i++)
            {
                for (int j = 0; j < _widthPx; j++)
                {

                    Vect.Subtract(camera.GetViewPaneEdge(), ray.GetOrigin(), rayDirection);
                    Vect.AddMultiple(rayDirection, vertAdd, i, rayDirection);
                    Vect.AddMultiple(rayDirection, horzAdd, j, rayDirection);

                    rayDirection.Normalize();

                    WideColor color = logic.Shoot(ray);

                    var localBrightness = color.GetMax();
                    if (localBrightness > localMaxBrightness)
                        localMaxBrightness = localBrightness;

                    _lbuf[i, j] = color;
                }
            }
            return localMaxBrightness;
        }


        private void UpdateMaxThreads()
        {
            ThreadPool.SetMaxThreads(_threadCount, SplitMultiplier * _threadCount);
        }
    }
}
