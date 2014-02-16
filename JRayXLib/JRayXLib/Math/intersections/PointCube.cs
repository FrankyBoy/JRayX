using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class PointCube {
        /**
	 * Returns true if, and only if, the point <code>point</code> is enclosed in the given cube. 
	 * 
	 * @param cubeCenter
	 * @param cubeWidthHalf
	 * @param point
	 * @return
	 */
        public static bool Encloses(Vect3 cubeCenter, double cubeWidthHalf, Vect3 point){
            return System.Math.Abs(cubeCenter[0] - point[0]) < cubeWidthHalf &&
                   System.Math.Abs(cubeCenter[1] - point[1]) < cubeWidthHalf &&
                   System.Math.Abs(cubeCenter[2] - point[2]) < cubeWidthHalf;
        }
    }
}
