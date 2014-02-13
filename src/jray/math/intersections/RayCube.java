package jray.math.intersections;

import jray.common.Vect3;

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
	public static double getDistanceToBorderPlane(Vect3 rayOrigin, Vect3 rayDirection, Vect3 boxCenter, double boxWidthHalf){
		double distance=Double.POSITIVE_INFINITY, tmp;
		
		if(rayDirection.data[0]>0){
			tmp = (boxCenter.data[0]+boxWidthHalf - rayOrigin.data[0]) / rayDirection.data[0];
			if(tmp < distance) distance = tmp;
		}
		
		if(rayDirection.data[1]>0){
			tmp = (boxCenter.data[1]+boxWidthHalf - rayOrigin.data[1]) / rayDirection.data[1];
			if(tmp < distance) distance = tmp;
		}
		
		if(rayDirection.data[2]>0){
			tmp = (boxCenter.data[2]+boxWidthHalf - rayOrigin.data[2]) / rayDirection.data[2];
			if(tmp < distance) distance = tmp;
		}
		
		if(rayDirection.data[0]<0){
			tmp = (boxCenter.data[0]-boxWidthHalf - rayOrigin.data[0]) / rayDirection.data[0];
			if(tmp < distance) distance = tmp;
		}
		
		if(rayDirection.data[1]<0){
			tmp = (boxCenter.data[1]-boxWidthHalf - rayOrigin.data[1]) / rayDirection.data[1];
			if(tmp < distance) distance = tmp;
		}
		
		if(rayDirection.data[2]<0){
			tmp = (boxCenter.data[2]-boxWidthHalf - rayOrigin.data[2]) / rayDirection.data[2];
			if(tmp < distance) distance = tmp;
		}
		
		return distance;
	}
}
