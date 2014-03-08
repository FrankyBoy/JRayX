/**
 *
 * @author Immanuel S. Hayden <cs071007 at fhstp.ac.at>
 * @see http://www.gamespp.com/algorithms/collisionDetectionTutorial02.html
 */

using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public abstract class RayPlane
    {
        public static double GetHitPointRayPlaneDistance(Vect3 rayOrigin, Vect3 rayDirection, Vect3 planePosition, Vect3 planeNormal)
        {
            double ret = rayDirection.DotProduct(planeNormal); // set ret to cos(a)

            if (System.Math.Abs(ret) < Constants.EPS)
            {
                // if cos(a) == 0 -> no hit
                return double.PositiveInfinity;
            }

            ret = (planePosition.DotProduct(planeNormal) - rayOrigin.DotProduct(planeNormal))/ret;

            return ret > Constants.EPS ? ret : double.PositiveInfinity;
        }

    }
}