using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    public class Triangle : Basic3DObject
    {
        // edge from v1 to v2
        protected Vect3 EdgeV1V2;

        // edge from v1 to v3
        protected Vect3 EdgeV1V3;

        // second corner of this triangle
        protected Vect3 V2;

        // third corner of this triangle
        protected Vect3 V3;

        public Triangle(Vect3 v1, Vect3 v2, Vect3 v3, Color color)
            : base(v1, new Vect3())
        {
            Color = color;
            V2 = v2;
            V3 = v3;

            EdgeV1V2 = v2 - v1;
            EdgeV1V3 = v3 - v1;
            LookAt = Vect3Extensions.CrossProduct(EdgeV1V2, EdgeV1V3).Normalize();
        }

        public override double GetHitPointDistance(Ray r)
        {
            return RayTriangle.GetHitPointRayTriangleDistance(r.Origin, r.Direction, Position, EdgeV1V2, EdgeV1V3);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            return new Vect3(LookAt);
        }

        public override bool Contains(Vect3 hitPoint)
        {
            Vect3 tmp = hitPoint - Position;

            Vect3 temp3 = Vect3Extensions.CrossProduct(EdgeV1V2, EdgeV1V3);
            if (System.Math.Abs(temp3.DotProduct(tmp)) > Constants.EPS)
            {
                return false;
            }

            double uu = EdgeV1V2.DotProduct(EdgeV1V2);
            double uv = EdgeV1V2.DotProduct(EdgeV1V3);
            double vv = EdgeV1V3.DotProduct(EdgeV1V3);
            double wu = EdgeV1V2.DotProduct(tmp);
            double wv = EdgeV1V3.DotProduct(tmp);
            double d = uv * uv - uu * vv;

            double s = (uv * wv - vv * wu) / d;
            if (s < 0 || s > 1)
            {
                return false;
            }

            double t = (uv * wu - uu * wv) / d;
            if (t < 0 || s + t > 1)
            {
                return false;
            }

            return true;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            EdgeV1V2 = VectMatrix.Multiply(EdgeV1V2, rotationMatrix);
            EdgeV1V3 = VectMatrix.Multiply(EdgeV1V3, rotationMatrix);

            LookAt = Vect3Extensions.CrossProduct(EdgeV1V2, EdgeV1V3);
        }

        public new string ToString()
        {
            return base.ToString() + " dir=[" + EdgeV1V2 + "/" + EdgeV1V3 + "]";
        }

        public new Vect3 GetBoundingSphereCenter()
        {
            return Vect3Extensions.Avg(new[] { Position, V2, V3 });
        }

        public override double GetBoundingSphereRadius()
        {
            Vect3 avg = Vect3Extensions.Avg(new[] { Position, V2, V3 });
            avg -= V3;

            return avg.Length();
        }
    }
}