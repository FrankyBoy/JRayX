﻿using System;
using JRayXLib;
using JRayXLib.Scene.Loaders;
using JRayXLib.Shapes;

namespace ConsoleApplication1
{
    class Program
    {
        private static void Main()
        {
            var scene = new RandomForrestLoader().LoadScene();
            //var scene = new MeshTest();
            //var scene = new KugelTest();

            var renderer = new Renderer
                {
                    Scene = scene,
                    ThreadCount = Environment.ProcessorCount,
                    SplitMultiplier = 4
                };

            var target = new Texture(800, 600);

            renderer.RenderImage(target);
            var bmp = target.ToBitmap();
            bmp.Save("test1.png");
            /*
            renderer.RenderImage(target);
            bmp = target.ToBitmap();
            bmp.Save("test2.png");

            renderer.RenderImage(target);
            bmp = target.ToBitmap();
            bmp.Save("test3.png");*/
        }
    }
}
