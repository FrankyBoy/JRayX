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
		
            if(rayDirection[0]>0){
                tmp = (boxCenter[0]+boxWidthHalf - rayOrigin[0]) / rayDirection[0];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection[1]>0){
                tmp = (boxCenter[1]+boxWidthHalf - rayOrigin[1]) / rayDirection[1];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection[2]>0){
                tmp = (boxCenter[2]+boxWidthHalf - rayOrigin[2]) / rayDirection[2];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection[0]<0){
                tmp = (boxCenter[0]-boxWidthHalf - rayOrigin[0]) / rayDirection[0];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection[1]<0){
                tmp = (boxCenter[1]-boxWidthHalf - rayOrigin[1]) / rayDirection[1];
                if(tmp < distance) distance = tmp;
            }
		
            if(rayDirection[2]<0){
                tmp = (boxCenter[2]-boxWidthHalf - rayOrigin[2]) / rayDirection[2];
                if(tmp < distance) distance = tmp;
            }
		
            return distance;
        }
    }
}
