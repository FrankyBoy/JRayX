package jray.math.intersections;

import jray.common.Vect3;

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
	public static boolean isCubeEnclosingCone(Vect3 cubeCenter, double cubeWidthHalf, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
		Vect3 planeNormal = new Vect3(),
		      planePoint = new Vect3(cubeCenter);
		
		if(!PointCube.encloses(cubeCenter, cubeWidthHalf, conePosition))
			return false;
		
		for(int a=0;a<3;a++){
			planeNormal.data[a] = 1;
			
			for(int d=0;d<2;d++){
				planePoint.data[a] = planePoint.data[a] - planeNormal.data[a]*cubeWidthHalf;
					
				if(PlaneCone.isPlaneIntersectingCone(planeNormal, planePoint, conePosition, coneAxis, coneAxisLength, coneCosPhi))
					return false;
				
				planePoint.data[a] = cubeCenter.data[a];
				planeNormal.data[a] *= -1;
			}
			
			planeNormal.data[a] = 0;
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
	public static boolean isCubeIntersectingCone(Vect3 cubeCenter, double cubeWidthHalf, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
		Vect3 planeNormal = new Vect3(),
		      planePoint = new Vect3(cubeCenter);
		
		if(PointCube.encloses(cubeCenter, cubeWidthHalf, conePosition))
			return true;
		
		for(int a=0;a<3;a++){
			planeNormal.data[a] = 1;
			
			for(int d=0;d<2;d++){
				planePoint.data[a] = planePoint.data[a] - planeNormal.data[a]*cubeWidthHalf;
					
				if(AreaCone.isAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis, coneAxisLength, coneCosPhi))
					return true;
				
				planePoint.data[a] = cubeCenter.data[a];
				planeNormal.data[a] *= -1;
			}
			
			planeNormal.data[a] = 0;
		}
		//System.out.println("ni with "+cubeCenter+" +/-"+cubeWidthHalf);
		return false;
	}
}
