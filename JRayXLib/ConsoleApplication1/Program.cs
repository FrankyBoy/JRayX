using JRayXLib.Ray;
using JRayXLib.Ray.Scenes;
using JRayXLib.Shapes;
using System.Drawing;

namespace ConsoleApplication1
{
    class Program
    {
        private static void Main(string[] args)
        {
            //var scene = new RandomForest();
            //var scene = new MeshTest();
            var scene = new KugelTest();
            var renderer = new Renderer(scene, 1);
            renderer.SplitMultiplier = 1;
            var target = new Texture(500, 500);
            renderer.RenderImage(target);
            Bitmap bmp = target.ToBitmap();
            bmp.Save("test.png");
        }
    }
}
