using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RayTriangle {

        public static double TriEPS = 0;
	
        public static double GetHitPointRayTriangleDistance(Vect3 rayPosition, Vect3 rayDirection, Vect3 trianglePos, Vect3 triangleVect1, Vect3 triangleVect2) {
            Vect3 tmp = new Vect3();
            Vect.CrossProduct(triangleVect1, triangleVect2, tmp);
            double ret = RayPlane.GetHitPointRayPlaneDistance(rayPosition, rayDirection, trianglePos, tmp);
            if(ret == double.PositiveInfinity || ret < Constants.MinDistance) {
                return double.PositiveInfinity;
            }

            Vect.AddMultiple(rayPosition, rayDirection, ret, tmp);
            Vect.Subtract(tmp, trianglePos, tmp);

            double uu = Vect.DotProduct(triangleVect1, triangleVect1);
            double uv = Vect.DotProduct(triangleVect1, triangleVect2);
            double vv = Vect.DotProduct(triangleVect2, triangleVect2);
            double wu = Vect.DotProduct(triangleVect1, tmp);
            double wv = Vect.DotProduct(triangleVect2, tmp);
            double d = uv * uv - uu * vv;

            double s = (uv * wv - vv * wu) / d;
            if (s < -TriEPS || s > 1+TriEPS) {
                return double.PositiveInfinity;
            }

            double t = (uv * wu - uu * wv) / d;
            if (t < -TriEPS || s + t > 1+TriEPS) {
                return double.PositiveInfinity;
            }
        
            return ret;
        }
    }
}
