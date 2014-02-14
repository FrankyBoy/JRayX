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
            return System.Math.Abs(cubeCenter.Data[0] - point.Data[0]) < cubeWidthHalf &&
                   System.Math.Abs(cubeCenter.Data[1] - point.Data[1]) < cubeWidthHalf &&
                   System.Math.Abs(cubeCenter.Data[2] - point.Data[2]) < cubeWidthHalf;
        }
    }
}
