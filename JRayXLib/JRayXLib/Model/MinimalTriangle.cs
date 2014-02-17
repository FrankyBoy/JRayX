using System;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Model
{
    public class MinimalTriangle : Basic3DObject
    {
        protected Sphere Bounds;
        protected Vect3 V2;
        protected Vect3 V3;

        public MinimalTriangle(Vect3 normal, Vect3 v1, Vect3 v2, Vect3 v3) : base(v1, normal)
        {
            V2 = v2;
            V3 = v3;

            Vect3 avg = GetBoundingSphereCenter();
            Vect3 tmp = avg - v3;

            Bounds = new Sphere(avg, tmp.Length(), Color.Black);
        }

        public override bool Contains(Vect3 hitPoint)
        {
            throw new Exception("not implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r)
        {
            Vect3 v1v2 = V2 - Position;
            Vect3 v1v3 = V3 - Position;

            double ret = RayTriangle.GetHitPointRayTriangleDistance(r.Origin, r.Direction, Position, v1v2, v1v3);
            if (ret <= 0)
            {
                return double.PositiveInfinity;
            }
            return ret;
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            return LookAt;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            throw new Exception("not implemented");
        }

        public new Sphere GetBoundingSphere()
        {
            return Bounds;
        }

        public new Vect3 GetBoundingSphereCenter()
        {
            return Vect3Extensions.Avg(new[] {Position, V2, V3});
        }

        public override double GetBoundingSphereRadius()
        {
            return Bounds.GetRadius();
        }

        public void Purge()
        {
            Bounds = null;
        }
    }
}