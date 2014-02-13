package jray.math.intersections;

import jray.common.Vect3;
import jray.math.Constants;
import jray.math.Vect;

public class RayTriangle {

	public static final double TRI_EPS = 0;
	
    public static double getHitPointRayTriangleDistance(Vect3 rayPosition, Vect3 rayDirection, Vect3 trianglePos, Vect3 triangleVect1, Vect3 triangleVect2) {
        Vect3 tmp = new Vect3();
        Vect.crossProduct(triangleVect1, triangleVect2, tmp);
        double ret = RayPlane.getHitPointRayPlaneDistance(rayPosition, rayDirection, trianglePos, tmp);
        if(ret == Double.POSITIVE_INFINITY || ret < Constants.MIN_DISTANCE) {
            return Double.POSITIVE_INFINITY;
        }

        Vect.addMultiple(rayPosition, rayDirection, ret, tmp);
        Vect.subtract(tmp, trianglePos, tmp);

        double uu = Vect.dotProduct(triangleVect1, triangleVect1);
        double uv = Vect.dotProduct(triangleVect1, triangleVect2);
        double vv = Vect.dotProduct(triangleVect2, triangleVect2);
        double wu = Vect.dotProduct(triangleVect1, tmp);
        double wv = Vect.dotProduct(triangleVect2, tmp);
        double d = uv * uv - uu * vv;

        double s = (uv * wv - vv * wu) / d;
        if (s < -TRI_EPS || s > 1+TRI_EPS) {
            return Double.POSITIVE_INFINITY;
        }

        double t = (uv * wu - uu * wv) / d;
        if (t < -TRI_EPS || s + t > 1+TRI_EPS) {
            return Double.POSITIVE_INFINITY;
        }
        
        return ret;
    }
}
