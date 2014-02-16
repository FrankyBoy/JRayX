using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    public class Triangle : Basic3DObject {
        /**
	 * edge from v1 to v2
	 */
        protected Vect3 edgev1v2 = new Vect3();
        /**
     * edge from v1 to v3
     */
        protected Vect3 edgev1v3 = new Vect3();
        /**
     * second corner of this triangle
     */
        protected Vect3 v2;
        /**
     * third corner of this triangle
     */
        protected Vect3 v3;

        public Triangle(Vect3 v1, Vect3 v2, Vect3 v3, Color color)
            : base(v1, new Vect3())
        {
            Color = color;
            this.v2 = v2;
            this.v3 = v3;

            Vect.Subtract(v2, v1, ref edgev1v2);
            Vect.Subtract(v3, v1, ref edgev1v3);
            Vect.CrossProduct(edgev1v2, edgev1v3, ref _lookAt);
            LookAt.Normalize();
        }

        public override double GetHitPointDistance(Ray r) {
            return RayTriangle.GetHitPointRayTriangleDistance(r.GetOrigin(), r.GetDirection(), Position, edgev1v2, edgev1v3);
        }

        public override void GetNormalAt(Vect3 hitPoint, ref Vect3 normal)
        {
            LookAt.CopyDataTo(normal);
        }

        public override bool Contains(Vect3 hitPoint) {
            var tmp = new Vect3();
            var temp3 = new Vect3();

            Vect.Subtract(hitPoint, Position, ref tmp);

            Vect.CrossProduct(edgev1v2, edgev1v3, ref temp3);
            if (System.Math.Abs(Vect.DotProduct(temp3, tmp)) > 1e-10) {
                return false;
            }

            double uu = Vect.DotProduct(edgev1v2, edgev1v2);
            double uv = Vect.DotProduct(edgev1v2, edgev1v3);
            double vv = Vect.DotProduct(edgev1v3, edgev1v3);
            double wu = Vect.DotProduct(edgev1v2, tmp);
            double wv = Vect.DotProduct(edgev1v3, tmp);
            double d = uv * uv - uu * vv;

            double s = (uv * wv - vv * wu) / d;
            if (s < 0 || s > 1) {
                return false;
            }

            double t = (uv * wu - uu * wv) / d;
            if (t < 0 || s + t > 1) {
                return false;
            }

            return true;
        }

        public override void Rotate(Matrix4 rotationMatrix) {
            VectMatrix.Multiply(edgev1v2, rotationMatrix, ref edgev1v2);
            VectMatrix.Multiply(edgev1v3, rotationMatrix, ref edgev1v3);

            Vect.CrossProduct(edgev1v2, edgev1v3, ref _lookAt);
        }

        public new string ToString() {
            return base.ToString() + " dir=[" + edgev1v2 + "/" + edgev1v3 + "]";
        }
    
        public new Vect3 GetBoundingSphereCenter(){
            return Vect.Avg(new []{Position, v2, v3});
        }

        public override double GetBoundingSphereRadius()
        {
            Vect3 avg = Vect.Avg(new[] { Position, v2, v3 });
            Vect.Subtract(avg, v3, ref avg);
		
            return avg.Length();
        }
    }
}
