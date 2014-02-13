package jray.math.intersections;

import jray.common.Vect3;

public class PointCube {
	/**
	 * Returns true if, and only if, the point <code>point</code> is enclosed in the given cube. 
	 * 
	 * @param cubeCenter
	 * @param cubeWidthHalf
	 * @param point
	 * @return
	 */
	public static boolean encloses(Vect3 cubeCenter, double cubeWidthHalf, Vect3 point){
		return Math.abs(cubeCenter.data[0]-point.data[0])<cubeWidthHalf&&
	       Math.abs(cubeCenter.data[1]-point.data[1])<cubeWidthHalf&&
	       Math.abs(cubeCenter.data[2]-point.data[2])<cubeWidthHalf;
	}
}
