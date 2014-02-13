package jray.math.intersections;

import jray.common.Vect3;

public class CubeSphere {

    public static boolean isSphereEnclosedByCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, Double sRadius){
    	return Math.abs(cCenter.data[0]-sCenter.data[0])<cWidthHalf - sRadius&&
	           Math.abs(cCenter.data[1]-sCenter.data[1])<cWidthHalf - sRadius&&
	           Math.abs(cCenter.data[2]-sCenter.data[2])<cWidthHalf - sRadius;
    }
    
    public static boolean isSphereIntersectingCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, Double sRadius){
    	return Math.abs(cCenter.data[0]-sCenter.data[0])<cWidthHalf + sRadius&&
	           Math.abs(cCenter.data[1]-sCenter.data[1])<cWidthHalf + sRadius&&
	           Math.abs(cCenter.data[2]-sCenter.data[2])<cWidthHalf + sRadius;
    }
}
