﻿using System;
using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Shapes;

namespace JRayXLib.Scene.Loaders
{
    public class RandomForrestLoader : ISceneLoader
    {
        private static readonly Random Rd = new Random();

        public Scene LoadScene()
        {
            var result = new OctreeScene();

            const double dist = 0.2;
            const double h = 0.3;
            const double camheight = 10;

            var objects = new List<I3DObject>();

            for (double x = 1; x < camheight * 3; x += h)
                for (double z = -camheight; z < camheight; z += h)
                    AddTree(x + Rd.NextDouble() * dist - dist / 2, -2, z + Rd.NextDouble() * dist - dist / 2, objects);

            var cam = new Camera(
                new Vect3 { X = -camheight, Y = camheight * 2 },
                new Vect3 { X = -camheight + 1, Y = camheight * 2 - 1 },
                new Vect3 { X = Constants.InvSqurtTwo, Y = Constants.InvSqurtTwo }, 1, 1, 1);
            
            result.Camera = cam;
            result.UpdateObjects(objects);

            return result;
        }

        private void AddTree(double x, double y, double z, IList<I3DObject> objects)
        {
            const double dist = 0.1;
            double t0 = Rd.NextDouble() * dist - dist / 2;
            double t1 = Rd.NextDouble() * dist - dist / 2;
            const double ld = 0.2;

            var leafColor = new Color
            {
                A = byte.MaxValue,
                R = byte.MaxValue / 3,
                G = (byte)(byte.MaxValue * ((1 + Rd.NextDouble()) / 2)),
                B = byte.MaxValue / 3
            };
            var @base = new Color
            {
                A = byte.MaxValue,
                R = 0x8B,
                G = 0x45,
                B = 0x13
            };

            if (Rd.NextDouble() < 0.5)
            {
                objects.Add(new Cone(new Vect3 { X = x, Y = y + 2, Z = z }, new Vect3 { X = t0, Y = -1.6, Z = t1 }, 10, leafColor));
                objects.Add(new Cone(new Vect3 { X = x, Y = y + 2, Z = z }, new Vect3 { X = t0 * 2, Y = -2, Z = t1 * 2 }, 1, @base));
            }
            else
            {
                objects.Add(new Sphere(new Vect3 { X = x, Y = y + 1.5 - ld / 2, Z = z }, ld, leafColor));
                objects.Add(new Cone(new Vect3 { X = x, Y = y + 1.5, Z = z }, new Vect3 { X = t0 * 2, Y = -1.5, Z = t1 * 2 }, 1, @base));
            }
        }
    }
}
