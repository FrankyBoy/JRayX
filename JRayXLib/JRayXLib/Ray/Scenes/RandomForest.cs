using System;
using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Scenes
{
    public class RandomForest : Scene {

        private List<Object3D> objects = new List<Object3D>();
        private Camera c;
        private Octree tree;
        private static Random rd = new Random();

        public RandomForest(){
            double dist=0.2;
            double h = 0.3;
            double camheight = 10;
    	
            for(double x = 1;x<camheight*3;x+=h)
                for(double z = -camheight;z<camheight;z+=h)
                    addTree(x+ rd.NextDouble() *dist-dist/2,-2,z+rd.NextDouble()*dist-dist/2);
        
            c = new Camera(new Vect3(-camheight, camheight*2, 0), new Vect3(-camheight+1, camheight*2-1, 0), new Vect3(0.707106781186547, 0.707106781186547, 0), 1, 1, 1);
            objects.Add(c);
        
            tree = Octree.BuildTree(new Vect3(camheight*1.5,-3.1,0),objects);
        }

        private void addTree(double x, double y, double z){
            double dist= 0.1;
            double t0 = rd.NextDouble()*dist-dist/2;
            double t1 = rd.NextDouble()*dist-dist/2;
            double ld = 0.2;

            Color leafColor = new Color
                {
                    A = byte.MaxValue,
                    R = byte.MaxValue / 2,
                    G = (byte) (byte.MaxValue * (1+rd.NextDouble() / 2)),
                    B = byte.MaxValue / 2
                };
            Color @base = new Color
                {
                    A = byte.MaxValue,
                    R = 0x8B,
                    G = 0x45,
                    B = 0x13
                };

            if(rd.NextDouble()<0.5){
                objects.Add(new Cone(new Vect3(x,y+2,z),new Vect3(t0,-1.6,t1),10,leafColor));
                objects.Add(new Cone(new Vect3(x,y+2,z),new Vect3(t0*2,-2,t1*2),1,@base));
            }else{
                objects.Add(new Sphere(new Vect3(x,y+1.5-ld/2,z),ld,leafColor));
                objects.Add(new Cone(new Vect3(x,y+1.5,z),new Vect3(t0*2,-1.5,t1*2),1,@base));
            }
        }
    
        public override List<Object3D> GetObjects() {
            return objects;
        }
    
        public override Camera GetCamera() {
            return c;
        }
    
        public override Sky GetSky() {
            return null;
        }
    
        public new Octree GetSceneTree(){
            return tree;
        }
    }
}
