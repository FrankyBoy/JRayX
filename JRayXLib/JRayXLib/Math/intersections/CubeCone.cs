
using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class CubeCone {
	
        /**
	 * Returns true if, and only if, the cube is enclosing the cone. This is true if a point of the cone is inside the cube and
	 * the cone is not intersecting any of the cube's border planes.
	 * 
	 * @param cubeCenter
	 * @param cubeWidthHalf
	 * @param conePosition
	 * @param coneAxis
	 * @param coneAxisLength
	 * @param coneCosPhi
	 * @return
	 */
        public static bool IsCubeEnclosingCone(Vect3 cubeCenter, double cubeWidthHalf, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
            Vect3 planeNormal = new Vect3(),
                  planePoint = new Vect3(cubeCenter);
		
            if(!PointCube.Encloses(cubeCenter, cubeWidthHalf, conePosition))
                return false;
		
            for(int a=0;a<3;a++){
                planeNormal.Data[a] = 1;
			
                for(int d=0;d<2;d++){
                    planePoint.Data[a] = planePoint.Data[a] - planeNormal.Data[a] * cubeWidthHalf;
					
                    if(PlaneCone.IsPlaneIntersectingCone(planeNormal, planePoint, conePosition, coneAxis, coneAxisLength, coneCosPhi))
                        return false;

                    planePoint.Data[a] = cubeCenter.Data[a];
                    planeNormal.Data[a] *= -1;
                }

                planeNormal.Data[a] = 0;
            }
		
            return true;
        }
	
        /**
	 * Returns true if, and only if, the cube is intersecting the cone. This is true if a random point of the cone is inside the cube or
	 * the cone is intersecting any of the cube's border areas.
	 * 
	 * @param cubeCenter
	 * @param cubeWidthHalf
	 * @param conePosition
	 * @param coneAxis
	 * @param coneAxisLength
	 * @param coneCosPhi
	 * @return
	 */
        public static bool IsCubeIntersectingCone(Vect3 cubeCenter, double cubeWidthHalf, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
            Vect3 planeNormal = new Vect3(),
                  planePoint = new Vect3(cubeCenter);
		
            if(PointCube.Encloses(cubeCenter, cubeWidthHalf, conePosition))
                return true;
		
            for(int a=0;a<3;a++){
                planeNormal.Data[a] = 1;
			
                for(int d=0;d<2;d++){
                    planePoint.Data[a] = planePoint.Data[a] - planeNormal.Data[a] * cubeWidthHalf;
					
                    if(AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis, coneAxisLength, coneCosPhi))
                        return true;

                    planePoint.Data[a] = cubeCenter.Data[a];
                    planeNormal.Data[a] *= -1;
                }

                planeNormal.Data[a] = 0;
            }
            //System.out.println("ni with "+cubeCenter+" +/-"+cubeWidthHalf);
            return false;
        }
    }
}
