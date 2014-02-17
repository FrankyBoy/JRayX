using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RayCube
    {
        /**
	 * Calculates the distance to the nearest ray-cube intersection (in positive ray-direction) for a ray originating in a cube.
	 * 
	 * @param rayOrigin
	 * @param rayDirection
	 * @param boxCenter
	 * @param boxWidthHalf
	 * @return rayOrigin + <b>x</b> * rayDirection = a-cube-border-point
	 */

        public static double GetDistanceToBorderPlane(Vect3 rayOrigin, Vect3 rayDirection, Vect3 boxCenter,
                                                      double boxWidthHalf)
        {
            double distance = double.PositiveInfinity, tmp;

            if (rayDirection.X > 0)
            {
                tmp = (boxCenter.X + boxWidthHalf - rayOrigin.X)/rayDirection.X;
                if (tmp < distance)
                    distance = tmp;
            }

            if (rayDirection.Y > 0)
            {
                tmp = (boxCenter.Y + boxWidthHalf - rayOrigin.Y)/rayDirection.Y;
                if (tmp < distance)
                    distance = tmp;
            }

            if (rayDirection.Z > 0)
            {
                tmp = (boxCenter.Z + boxWidthHalf - rayOrigin.Z)/rayDirection.Z;
                if (tmp < distance)
                    distance = tmp;
            }

            if (rayDirection.X < 0)
            {
                tmp = (boxCenter.X - boxWidthHalf - rayOrigin.X)/rayDirection.X;
                if (tmp < distance)
                    distance = tmp;
            }

            if (rayDirection.Y < 0)
            {
                tmp = (boxCenter.Y - boxWidthHalf - rayOrigin.Y)/rayDirection.Y;
                if (tmp < distance)
                    distance = tmp;
            }

            if (rayDirection.Z < 0)
            {
                tmp = (boxCenter.Z - boxWidthHalf - rayOrigin.Z)/rayDirection.Z;
                if (tmp < distance)
                    distance = tmp;
            }

            return distance;
        }
    }
}