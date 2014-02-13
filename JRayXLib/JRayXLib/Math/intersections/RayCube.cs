using JRayXLib.Common;

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
		
            if(rayDirection.Data[0]>0){
                tmp = (boxCenter.Data[0]+boxWidthHalf - rayOrigin.Data[0]) / rayDirection.Data[0];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection.Data[1]>0){
                tmp = (boxCenter.Data[1]+boxWidthHalf - rayOrigin.Data[1]) / rayDirection.Data[1];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection.Data[2]>0){
                tmp = (boxCenter.Data[2]+boxWidthHalf - rayOrigin.Data[2]) / rayDirection.Data[2];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection.Data[0]<0){
                tmp = (boxCenter.Data[0]-boxWidthHalf - rayOrigin.Data[0]) / rayDirection.Data[0];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection.Data[1]<0){
                tmp = (boxCenter.Data[1]-boxWidthHalf - rayOrigin.Data[1]) / rayDirection.Data[1];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection.Data[2]<0){
                tmp = (boxCenter.Data[2]-boxWidthHalf - rayOrigin.Data[2]) / rayDirection.Data[2];
                if(tmp < distance) distance = tmp;
            }
		
            return distance;
        }
    }
}
