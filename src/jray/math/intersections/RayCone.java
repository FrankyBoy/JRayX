package jray.math.intersections;

import jray.common.Vect3;
import jray.math.Vect;

public class RayCone {

	public static double getRayConeIntersectionDistance(Vect3 ro, Vect3 rd, Vect3 v, Vect3 a, double cos, double axisLength) {
		double add = Vect.dotProduct(a, rd);
		cos *= cos;
		Vect3 e = new Vect3(ro.data[0] - v.data[0],ro.data[1] - v.data[1],ro.data[2] - v.data[2]);
		double ade = Vect.dotProduct(a, e);
		double dde = Vect.dotProduct(rd, e);
		double ede = Vect.dotProduct(e, e);
		
    	double c2 = add*add - cos;
    	double c1 = add*ade - cos*dde;
    	double c0 = ade*ade - cos*ede;
    	
    	cos = c1*c1-c0*c2;
    	
    	if(cos<0)
    		return -1;
    	cos = Math.sqrt(cos);
    	
    	//solutions
    	double t0 = (-c1 + cos)/(c2);
    	double t1 = (-c1 - cos)/(c2);
    	
    	//project both solutions onto a - must be between 0 and 1
    	e.data[0] = ro.data[0] + t0*rd.data[0] - v.data[0];
    	e.data[1] = ro.data[1] + t0*rd.data[1] - v.data[1];
    	e.data[2] = ro.data[2] + t0*rd.data[2] - v.data[2];
    	cos = Vect.dotProduct(e, a);
    	if(cos <= 0 || cos > axisLength) t0 = -1;
    	
    	e.data[0] = ro.data[0] + t1*rd.data[0] - v.data[0];
    	e.data[1] = ro.data[1] + t1*rd.data[1] - v.data[1];
    	e.data[2] = ro.data[2] + t1*rd.data[2] - v.data[2];
    	cos = Vect.dotProduct(e, a);
    	if(cos <= 0 || cos > axisLength) t1 = -1;
    	
    	if(t0>0){
    		if(t1>0){
    			if(t0<t1) return t0;
    			else return t1;
    		}else return t0;
    	}if(t1>0) return t1;
    	
    	return -1;
    }
}
