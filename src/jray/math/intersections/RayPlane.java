package jray.math.intersections;

import jray.common.Vect3;
import jray.math.Constants;
import jray.math.Vect;

/**
 *
 * @author Immanuel S. Hayden <cs071007 at fhstp.ac.at>
 * @see http://www.gamespp.com/algorithms/collisionDetectionTutorial02.html
 */
abstract public class RayPlane {
    public static double getHitPointRayPlaneDistance(Vect3 rayOrigin, Vect3 rayDirection, Vect3 planePosition, Vect3 planeNormal) {

        double ret = Vect.dotProduct(rayDirection, planeNormal); // set ret to cos(a)

        if(ret == 0) { // if cos(a) == 0 -> no hit
            return Double.POSITIVE_INFINITY;
        }

        ret = (Vect.dotProduct(planePosition, planeNormal) -  Vect.dotProduct(rayOrigin, planeNormal)) / ret;

        return ret > Constants.MIN_DISTANCE ? ret : Double.POSITIVE_INFINITY;
    }
}
