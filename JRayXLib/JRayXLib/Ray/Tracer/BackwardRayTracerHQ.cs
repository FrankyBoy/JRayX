/**
 * A backward ray tracer using surface normals for simple light effects.
 */

using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Tracer
{
    public class BackwardRayTracerHQ : BackwardRayTracer{

        private const double DiffuseLightIntensity = 0.8;
        private const double AmbientLightIntensity = 0.2;

        public BackwardRayTracerHQ(Scene scene) : base(scene) {}

        protected override WideColor ShootRay(Shapes.Ray ray, int level) {
            if (level == MaxRecursionDepth) {
                return Color.Red.ToWide();
            }

            //Find nearest Hit
            CollisionDetails c = FindNearestHit(ray);

            //No hit
            if (c.Obj == null || double.IsInfinity(c.Distance))
            {
                return Color.White.ToWide();
            }
        
            //Calculate hit position
            var hitPoint = new Vect3(0);
            Vect.AddMultiple(ray.GetOrigin(), ray.GetDirection(), c.Distance, ref hitPoint);
        
            //Get color and normal at hitpoint
            var color = c.Obj.GetColorAt(hitPoint).ToWide(); 
            var normal = c.Obj.GetNormalAt(hitPoint);
        
            //Check if anything is blocking direct sunlight (go where the sunlight comes from)
            var lrDir = new Vect3(0);
            Vect.Scale(Scene.LightDirection, -1, ref lrDir);
            var lightRay = new Shapes.Ray(hitPoint, lrDir);
            CollisionDetails lc = FindNearestHit(lightRay);
        
            //if nothing blocks the sun's light, add ambient and diffuse light, otherwise ambient only  
            double lightScale = 0;
            if(lc.Obj==null)
                lightScale = Vect.DotProduct(normal, Scene.LightDirection);
            lightScale = AmbientLightIntensity + DiffuseLightIntensity*(lightScale<0?-lightScale:0);

            return color * lightScale;
        }
    }
}
