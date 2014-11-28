using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Tracer
{
    public class BackwardRayTracer
    {
        protected static bool UseOctree = true;
        protected static int MaxRecursionDepth = 100;
        protected Scene.Scene Scene;
        
        public BackwardRayTracer(Scene.Scene scene)
        {
            Scene = scene;
        }

        public WideColor Shoot(Shapes.Ray ray)
        {
            return ShootRay(ray, 0);
        }

        protected virtual WideColor ShootRay(Shapes.Ray ray, int level)
        {
            if (level == MaxRecursionDepth)
            {
                return Color.Red.ToWide();
            }

            //Find nearest Hit
            CollisionDetails c = Scene.FindNearestHit(ray);

            //No hit
            if (c.Obj == null)
            {
                return Color.White.ToWide();
            }

            //Calculate hit point
            Vect3 hitPoint = ray.Origin + ray.Direction*c.Distance;

            //Return object's color at hit point
            return c.Obj.GetColorAt(hitPoint).ToWide();
        }
    }
}