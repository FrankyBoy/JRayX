using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RayCube {
        /**
	 * Calculates the distance to the nearest ray-cube intersection (in positive ray-direction) for a ray originating in a cube.
	 * 
	 * @param rayOrigin
	 * @param rayDirection
	 * @param boxCenter
	 * @param boxWidthHalf
	 * @return rayOrigin + <b>x</b> * rayDirection = a-cube-border-point
	 */
        public static double GetDistanceToBorderPlane(Vect3 rayOrigin, Vect3 rayDirection, Vect3 boxCenter, double boxWidthHalf){
            double distance=double.PositiveInfinity, tmp;

            double[] dirData = rayDirection.Data;
            double[] centerData = boxCenter.Data;
            double[] originData = rayOrigin.Data;
            
            if(dirData[0]>0){
                tmp = (centerData[0]+boxWidthHalf - originData[0]) / dirData[0];
                if(tmp < distance) distance = tmp;
            }
		
            if(dirData[1]>0){
                tmp = (centerData[1]+boxWidthHalf - originData[1]) / dirData[1];
                if(tmp < distance) distance = tmp;
            }
		
            if(dirData[2]>0){
                tmp = (centerData[2]+boxWidthHalf - originData[2]) / dirData[2];
                if(tmp < distance) distance = tmp;
            }
		
            if(dirData[0]<0){
                tmp = (centerData[0]-boxWidthHalf - originData[0]) / dirData[0];
                if(tmp < distance) distance = tmp;
            }
		
            if(dirData[1]<0){
                tmp = (centerData[1]-boxWidthHalf - originData[1]) / dirData[1];
                if(tmp < distance) distance = tmp;
            }
		
            if(dirData[2]<0){
                tmp = (centerData[2]-boxWidthHalf - originData[2]) / dirData[2];
                if(tmp < distance) distance = tmp;
            }
		
            return distance;
        }
    }
}
