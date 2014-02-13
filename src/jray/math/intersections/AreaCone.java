package jray.math.intersections;

import jray.common.Vect3;
import jray.math.Vect;

public class AreaCone {
	
	public static boolean isAreaIntersectingCone(Vect3 planeNormal, Vect3 planePoint, double planeWidth2, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
		Vect3 qx = new Vect3();
		Vect3 p = new Vect3();
		Vect3 q = new Vect3();
		double len=0;
		
		//try "bending" axis towards plane normal 
		Vect.addMultiple(conePosition, coneAxis, coneAxisLength, q);
		Vect.project(planeNormal, coneAxis, qx);
		Vect.addMultiple(qx, planeNormal, -1, p);
		
		if(p.quadLength()==0){// axis equals plane normal
			coneAxis.copyDataTo(p);
			len = coneAxisLength;
		}else{//bend axis towards plane normal as far as sinPhi allows
			p.normalize();
			Vect.addMultiple(q, p, coneAxisLength*Math.sin(Math.acos(coneCosPhi)), q);
			Vect.subtract(q, conePosition, q);
			len = q.length();
			Vect.scale(q, 1/len, p);
		}

		double d = RayPlane.getHitPointRayPlaneDistance(conePosition, p, planePoint, planeNormal);
		
		if(d<len){
			//check if Hitpoint is in the +/-width/2 - area of the plane
			Vect.addMultiple(conePosition, p, d, p);
			if(Math.abs(p.data[0]-planePoint.data[0])<planeWidth2*2&&
				Math.abs(p.data[1]-planePoint.data[1])<planeWidth2*2&&
				Math.abs(p.data[2]-planePoint.data[2])<planeWidth2*2)
				return true;
		}
		/*
		//check if area border lines intersect cone
		for(int i=0;i<2;i++){
			for(int j=0;j<2;j++){
				p.data[0] = 0;
				p.data[0] = 0;
				p.data[0] = 0;
			}
		}*/
		
		return false;
	}
}
