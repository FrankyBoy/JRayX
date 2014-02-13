package jray.math.intersections;

import jray.common.Vect3;
import jray.math.Constants;
import jray.math.Vect;

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
    public static double getHitPointRaySphereDistance(Vect3 rayOrigin, Vect3 rayDirection, Vect3 sphereCenter, double sphereRadius) {

        Vect3 tmp = new Vect3();

        /* calculate b, c for the quadratic formula described in
         * http://en.wikipedia.org/wiki/Line–sphere_intersection
         * a is always 1 as the ray-direction is normalized
         *
         * l .... ray's direction vector
         * cp ... center point of sphere
         * r .... sphere radius
         * */

        double b = 0; // = l * cp
        double c = 0; // = cp² - r² = (cp + radius)  * (cp - radius)

        Vect.subtract(sphereCenter, rayOrigin, tmp);

        b = Vect.dotProduct(rayDirection, tmp);
        c = Vect.dotProduct(tmp, tmp) - sphereRadius * sphereRadius;

        double sqrt_expr = b * b - c;

        double ret;

        if (sqrt_expr < 0) {            // kein Treffer
            return Double.POSITIVE_INFINITY;
        } else if (sqrt_expr == 0) {    // Tangent
            ret = b;
        } else {                        // Sekante
            sqrt_expr = Math.sqrt(sqrt_expr);
            double d1 = (b + sqrt_expr);
            double d2 = (b - sqrt_expr);

            // first set ret to the smaller value of the both
            ret = d1 < d2 ? d1 : d2;

            // if the smaller value is <= 0 -> put the other value in ret
            if (ret <= Constants.MIN_DISTANCE) {
                if (ret == d1) {
                    ret = d2;
                } else {
                    ret = d1;
                }
            }
        }

        if (ret <= Constants.MIN_DISTANCE) {
            return Double.POSITIVE_INFINITY;
        }
        return ret;
    }
    
    public static boolean isRayOriginatingInSphere(Vect3 rayOrigin, Vect3 rayDirection, Vect3 sphereCenter, double sphereRadius){
    	Vect3 tmp = new Vect3();
    	Vect.subtract(sphereCenter, rayOrigin, tmp);
    	
    	return tmp.quadLength()<sphereRadius*sphereRadius;
    }
}
