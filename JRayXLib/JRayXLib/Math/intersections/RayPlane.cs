
/**
 *
 * @author Immanuel S. Hayden <cs071007 at fhstp.ac.at>
 * @see http://www.gamespp.com/algorithms/collisionDetectionTutorial02.html
 */

using JRayXLib.Common;

namespace JRayXLib.Math.intersections
{
    abstract public class RayPlane {
        public static double GetHitPointRayPlaneDistance(Vect3 rayOrigin, Vect3 rayDirection, Vect3 planePosition, Vect3 planeNormal) {

            double ret = Vect.dotProduct(rayDirection, planeNormal); // set ret to cos(a)

            if(System.Math.Abs(ret - 0) < Constants.EPS) { // if cos(a) == 0 -> no hit
                return double.PositiveInfinity;
            }

            ret = (Vect.dotProduct(planePosition, planeNormal) -  Vect.dotProduct(rayOrigin, planeNormal)) / ret;

            return ret > Constants.MinDistance ? ret : double.PositiveInfinity;
        }
    }
}
