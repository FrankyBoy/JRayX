package jray.math.intersections;

import jray.common.Vect3;
import jray.math.Vect;

public class PlaneCone {
	
	public static boolean isPlaneIntersectingCone(Vect3 planeNormal, Vect3 planePoint, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
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
		
		return d < len;
	}
}
