using System;
using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Scenes
{
    public class RandomForest : Scene {

        private readonly Octree _tree;
        private static readonly Random Rd = new Random();

        public RandomForest(){
            const double dist = 0.2;
            const double h = 0.3;
            const double camheight = 10;

            var objects = new List<I3DObject>();

            for(double x = 1;x<camheight*3;x+=h)
                for(double z = -camheight;z<camheight;z+=h)
                    AddTree(x+ Rd.NextDouble() *dist-dist/2,-2,z+Rd.NextDouble()*dist-dist/2, objects);
        
            Camera = new Camera(new Vect3(-camheight, camheight*2, 0), new Vect3(-camheight+1, camheight*2-1, 0), new Vect3(0.707106781186547, 0.707106781186547, 0), 1, 1, 1);
            objects.Add(Camera);
            Objects = objects.ToArray();
        
            _tree = Octree.BuildTree(new Vect3(camheight*1.5,-3.1,0), Objects);
        }

        private void AddTree(double x, double y, double z, IList<I3DObject> objects)
        {
            const double dist = 0.1;
            double t0 = Rd.NextDouble()*dist-dist/2;
            double t1 = Rd.NextDouble()*dist-dist/2;
            const double ld = 0.2;

            var leafColor = new Color
                {
                    A = byte.MaxValue,
                    R = byte.MaxValue / 2,
                    G = (byte) (byte.MaxValue * (1+Rd.NextDouble() / 2)),
                    B = byte.MaxValue / 2
                };
            var @base = new Color
                {
                    A = byte.MaxValue,
                    R = 0x8B,
                    G = 0x45,
                    B = 0x13
                };

            if(Rd.NextDouble()<0.5){
                objects.Add(new Cone(new Vect3(x,y+2,z),new Vect3(t0,-1.6,t1),10,leafColor));
                objects.Add(new Cone(new Vect3(x,y+2,z),new Vect3(t0*2,-2,t1*2),1,@base));
            }else{
                objects.Add(new Sphere(new Vect3(x,y+1.5-ld/2,z),ld,leafColor));
                objects.Add(new Cone(new Vect3(x,y+1.5,z),new Vect3(t0*2,-1.5,t1*2),1,@base));
            }
        }
    
        public override Sky GetSky() {
            return null;
        }

        public override Octree GetSceneTree(){
            return _tree;
        }
    }
}
