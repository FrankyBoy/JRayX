using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class PointCube
    {
        /**
     * Returns true if, and only if, the point <code>point</code> is enclosed in the given cube. 
     * 
     * @param cubeCenter
     * @param cubeWidthHalf
     * @param point
     * @return
     */

        public static bool Encloses(Vect3 cubeCenter, double cubeWidthHalf, Vect3 point)
        {
            return System.Math.Abs(cubeCenter.X - point.X) < cubeWidthHalf &&
                   System.Math.Abs(cubeCenter.Y - point.Y) < cubeWidthHalf &&
                   System.Math.Abs(cubeCenter.Z - point.Z) < cubeWidthHalf;
        }
    }
}