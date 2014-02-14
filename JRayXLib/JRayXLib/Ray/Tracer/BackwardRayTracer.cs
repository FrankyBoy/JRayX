using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Tracer
{
    public class BackwardRayTracer {

        protected static bool USE_OCTREE = true;
        protected static int MAX_RECURSION_DEPTH = 100;
        protected Object3D[] objects;
    
        protected Octree tree;
    
        public BackwardRayTracer(Scene scene) {
            List<Object3D> allObjects = new List<Object3D>();
            allObjects.AddRange(scene.GetObjects());
        
            objects = allObjects.ToArray();
        
            tree = scene.GetSceneTree();
        }

        public WideColor shoot(Shapes.Ray ray) {
            return shootRay(ray, 0);
        }

        protected WideColor shootRay(Shapes.Ray ray, int level)
        {
            if (level == MAX_RECURSION_DEPTH) {
                return Color.Red.ToWide();
            }

            //Find nearest Hit
            CollisionDetails c = findNearestHit(ray);

            //No hit
            if (c.Obj == null) {
                return Color.White.ToWide();
            }
        
            //Calculate hit point
            var hitPoint = new Vect3();
            Vect.AddMultiple(ray.GetOrigin(), ray.GetDirection(), c.Distance, hitPoint); //Position des Treffers berechnen
        
            //Return object's color at hit point
            return c.Obj.GetColorAt(hitPoint).ToWide();
        }

        protected CollisionDetails findNearestHit(Shapes.Ray ray)
        {
            if(!USE_OCTREE || tree==null){
                var c = new CollisionDetails(ray);
    		
                //Find nearest Hit
                foreach (Object3D objectCandidate in objects) {
                    double distanceCandidate = objectCandidate.GetHitPointDistance(ray);
                    if (distanceCandidate > 0 && distanceCandidate < c.Distance) {
                        c.Obj = objectCandidate;
                        c.Distance = distanceCandidate;
                        c.Checks++;
                    }
                }
	        
                return c;
            }
            return RayPath.getFirstCollision(tree, ray);
        }
    }
}
