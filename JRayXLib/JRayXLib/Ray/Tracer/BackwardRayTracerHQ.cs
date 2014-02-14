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

        private static readonly Vect3 LightDirection = new Vect3(2, -1, -2); //direction of the sun's light-rays
        private const double DiffuseLightIntensity = 0.8;
        private const double AmbientLightIntensity = 0.2;

        public BackwardRayTracerHQ(Scene scene) : base(scene) {
        
            LightDirection.normalize();
        }

        protected new WideColor ShootRay(Shapes.Ray ray, int level) {
            if (level == MaxRecursionDepth) {
                return Color.Red.ToWide();
            }

            //Find nearest Hit
            CollisionDetails c = FindNearestHit(ray);

            //No hit
            if (c.Obj == null) {
                return Color.White.ToWide();
            }
        
            //Calculate hit position
            var hitPoint = new Vect3();
            Vect.AddMultiple(ray.GetOrigin(), ray.GetDirection(), c.Distance, hitPoint);
        
            //Get color and normal at hitpoint
            var color = c.Obj.GetColorAt(hitPoint).ToWide(); 
            var normal = new Vect3();
            c.Obj.GetNormalAt(hitPoint, normal);
        
            //Check if anything is blocking direct sunlight (go where the sunlight comes from)
            var lrDir = new Vect3();
            Vect.Scale(LightDirection, -1, lrDir);
            var lightRay = new Shapes.Ray(hitPoint, lrDir);
            CollisionDetails lc = FindNearestHit(lightRay);
        
            //if nothing blocks the sun's light, add ambient and diffuse light, otherwise ambient only  
            double lightScale = 0;
            if(lc.Obj==null)
                lightScale = Vect.dotProduct(normal, LightDirection);
            lightScale = AmbientLightIntensity + DiffuseLightIntensity*(lightScale<0?-lightScale:0);

            return color.Scale(lightScale);
        }
    }
}
