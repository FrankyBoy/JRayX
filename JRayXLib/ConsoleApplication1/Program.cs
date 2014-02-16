using System;
using JRayXLib.Ray;
using JRayXLib.Ray.Scenes;
using JRayXLib.Shapes;

namespace ConsoleApplication1
{
    class Program
    {
        private static void Main(string[] args)
        {
            var scene = new RandomForest();
            //var scene = new MeshTest();
            //var scene = new KugelTest();

            var renderer = new Renderer
                {
                    Scene = scene,
                    ThreadCount = Environment.ProcessorCount,
                    SplitMultiplier = 2
                };

            var target = new Texture(500, 500);

            renderer.RenderImage(target);
            var bmp = target.ToBitmap();
            bmp.Save("test.png");
        }
    }
}
