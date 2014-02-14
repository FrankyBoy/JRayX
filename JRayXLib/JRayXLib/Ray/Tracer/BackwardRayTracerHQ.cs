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

        private static  Vect3 LIGHT_DIRECTION = new Vect3(2, -1, -2); //direction of the sun's light-rays
        private static  double DIFFUSE_LIGHT_INTENSITY = 0.8;
        private static  double AMBIENT_LIGHT_INTENSITY = 0.2;
    
        public BackwardRayTracerHQ(Scene scene) : base(scene) {
        
            LIGHT_DIRECTION.normalize();
        }

        protected WideColor shootRay(Shapes.Ray ray, int level) {
            if (level == MAX_RECURSION_DEPTH) {
                return Color.Red.ToWide();
            }

            //Find nearest Hit
            CollisionDetails c = findNearestHit(ray);

            //No hit
            if (c.Obj == null) {
                return Color.White.ToWide();
            }
        
            //Calculate hit position
            Vect3 hitPoint = new Vect3();
            Vect.AddMultiple(ray.GetOrigin(), ray.GetDirection(), c.Distance, hitPoint);
        
            //Get color and normal at hitpoint
            var color = c.Obj.GetColorAt(hitPoint).ToWide(); 
            var normal = new Vect3();
            c.Obj.GetNormalAt(hitPoint, normal);
        
            //Check if anything is blocking direct sunlight (go where the sunlight comes from)
            Vect3 lrDir = new Vect3();
            Vect.Scale(LIGHT_DIRECTION, -1, lrDir);
            Shapes.Ray lightRay = new Shapes.Ray(hitPoint, lrDir);
            CollisionDetails lc = findNearestHit(lightRay);
        
            //if nothing blocks the sun's light, add ambient and diffuse light, otherwise ambient only  
            double lightScale = 0;
            if(lc.Obj==null)
                lightScale = Vect.dotProduct(normal, LIGHT_DIRECTION);
            lightScale = AMBIENT_LIGHT_INTENSITY + DIFFUSE_LIGHT_INTENSITY*(lightScale<0?-lightScale:0);

            return color.Scale(lightScale);
        }
    }
}
