/**
 * A backward ray tracer using surface normals for simple light effects.
 */

using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Tracer
{
    public class BackwardRayTracerHQ : BackwardRayTracer
    {
        private const double DiffuseLightIntensity = 0.8;
        private const double AmbientLightIntensity = 0.2;

        public BackwardRayTracerHQ(Scene scene) : base(scene)
        {
        }

        protected override WideColor ShootRay(Shapes.Ray ray, int level)
        {
            if (level == MaxRecursionDepth)
            {
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
            Vect3 hitPoint = ray.Origin + ray.Direction*c.Distance;

            //Get color and normal at hitpoint
            WideColor color = c.Obj.GetColorAt(hitPoint).ToWide();
            Vect3 normal = c.Obj.GetNormalAt(hitPoint);

            //Check if anything is blocking direct sunlight (go where the sunlight comes from)
            Vect3 lrDir = Scene.LightDirection*-1;
            var lightRay = new Shapes.Ray
                {
                    Origin = hitPoint,
                    Direction = lrDir
                };
            CollisionDetails lc = FindNearestHit(lightRay);

            //if nothing blocks the sun's light, add ambient and diffuse light, otherwise ambient only  
            double lightScale = 0;
            if (lc.Obj == null)
                lightScale = normal.DotProduct(Scene.LightDirection);
            lightScale = AmbientLightIntensity + DiffuseLightIntensity*(lightScale < 0 ? -lightScale : 0);

            return color*lightScale;
        }
    }
}