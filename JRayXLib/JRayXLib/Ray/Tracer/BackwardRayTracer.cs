using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Tracer
{
    public class BackwardRayTracer {

        protected static bool UseOctree = true;
        protected static int MaxRecursionDepth = 100;
        protected Scene Scene;
    
        protected Octree Tree;
    
        public BackwardRayTracer(Scene scene)
        {
            Scene = scene;
            Tree = scene.GetSceneTree();
        }

        public WideColor Shoot(Shapes.Ray ray) {
            return ShootRay(ray, 0);
        }

        protected virtual WideColor ShootRay(Shapes.Ray ray, int level)
        {
            if (level == MaxRecursionDepth) {
                return Color.Red.ToWide();
            }

            //Find nearest Hit
            CollisionDetails c = FindNearestHit(ray);

            //No hit
            if (c.Obj == null) {
                return Color.White.ToWide();
            }
        
            //Calculate hit point
            var hitPoint = new Vect3();
            Vect.AddMultiple(ray.GetOrigin(), ray.GetDirection(), c.Distance, ref hitPoint); //Position des Treffers berechnen
        
            //Return object's color at hit point
            return c.Obj.GetColorAt(hitPoint).ToWide();
        }

        protected CollisionDetails FindNearestHit(Shapes.Ray ray)
        {
            if(!UseOctree || Tree==null){
                var c = new CollisionDetails(ray);
    		
                //Find nearest Hit
                foreach (Basic3DObject objectCandidate in Scene.Objects) {
                    double distanceCandidate = objectCandidate.GetHitPointDistance(ray);
                    if (distanceCandidate > 0 && distanceCandidate < c.Distance) {
                        c.Obj = objectCandidate;
                        c.Distance = distanceCandidate;
                        c.Checks++;
                    }
                }
	        
                return c;
            }
            return RayPath.GetFirstCollision(Tree, ray);
        }
    }
}
