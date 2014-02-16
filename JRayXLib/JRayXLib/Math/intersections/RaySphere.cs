using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RaySphere {

        /**
     *
     * @param rayOrigin
     * @param rayDirection
     * @param sphereCenter The center of the sphere
     * @param sphereRadius The radius of the spere
     * @param biggerThan
     * @return the (smaller) scalar to apply to the ray to get a hit point with
     * the sphere or Double.POSITIVE_INFINITY if no hit point is present
     */
        public static double GetHitPointRaySphereDistance(Vect3 rayOrigin, Vect3 rayDirection, Vect3 sphereCenter, double sphereRadius) {
            /* calculate b, c for the quadratic formula described in
         * http://en.wikipedia.org/wiki/Line–sphere_intersection
         * a is always 1 as the ray-direction is normalized
         *
         * l .... ray's direction vector
         * cp ... center point of sphere
         * r .... sphere radius
         * */

            Vect3 tmp = sphereCenter - rayOrigin;

            double b = Vect.DotProduct(rayDirection, tmp);
            double c = Vect.DotProduct(tmp, tmp) - sphereRadius * sphereRadius;

            double sqrtExpr = b * b - c;

            double ret;

            if (sqrtExpr < 0) {            // kein Treffer
                return double.PositiveInfinity;
            }

            if (System.Math.Abs(sqrtExpr - 0) < Constants.EPS) {    // Tangent
                ret = b;
            } else {                        // Sekante
                sqrtExpr = System.Math.Sqrt(sqrtExpr);
                double d1 = (b + sqrtExpr);
                double d2 = (b - sqrtExpr);

                // first set ret to the smaller value of the both
                ret = d1 < d2 ? d1 : d2;

                // if the smaller value is <= 0 -> put the other value in ret
                if (ret <= Constants.MinDistance) {
                    if (System.Math.Abs(ret - d1) < Constants.EPS) {
                        ret = d2;
                    } else {
                        ret = d1;
                    }
                }
            }

            if (ret <= Constants.MinDistance) {
                return double.PositiveInfinity;
            }
            return ret;
        }
    
        public static bool IsRayOriginatingInSphere(Vect3 rayOrigin, Vect3 rayDirection, Vect3 sphereCenter, double sphereRadius){
            var tmp = sphereCenter - rayOrigin;
    	
            return tmp.QuadLength()<sphereRadius*sphereRadius;
        }
    }
}
