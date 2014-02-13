package jray.common;

import jray.math.Vect;
import jray.math.VectMatrix;
import jray.math.intersections.RayTriangle;

public class Triangle extends Object3D {
	/**
	 * edge from v1 to v2
	 */
    protected Vect3 edgev1v2 = new Vect3();
    /**
     * edge from v1 to v3
     */
    protected Vect3 edgev1v3 = new Vect3();
    /**
     * second corner of this triangle
     */
    protected Vect3 v2;
    /**
     * third corner of this triangle
     */
    protected Vect3 v3;

    public Triangle(Vect3 v1, Vect3 v2, Vect3 v3, int color) {
        super(v1, new Vect3());
        this.color = color;
        this.v2 = v2;
        this.v3 = v3;

        Vect.subtract(v2, v1, edgev1v2);
        Vect.subtract(v3, v1, edgev1v3);
        Vect.crossProduct(edgev1v2, edgev1v3, this.lookAt);
        lookAt.normalize();
    }

    @Override
    public double getHitPointDistance(Ray r) {
    	return RayTriangle.getHitPointRayTriangleDistance(r.getOrigin(), r.getDirection(), position, edgev1v2, edgev1v3);
    }

    @Override
    public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
        lookAt.copyDataTo(normal);
    }

    public boolean contains(Vect3 hitPoint) {
        Vect3 tmp = new Vect3();
        Vect3 temp3 = new Vect3();

        Vect.subtract(hitPoint, position, tmp);

        Vect.crossProduct(edgev1v2, edgev1v3, temp3);
        if (Math.abs(Vect.dotProduct(temp3, tmp)) > 1e-10) {
            return false;
        }

        double uu = Vect.dotProduct(edgev1v2, edgev1v2);
        double uv = Vect.dotProduct(edgev1v2, edgev1v3);
        double vv = Vect.dotProduct(edgev1v3, edgev1v3);
        double wu = Vect.dotProduct(edgev1v2, tmp);
        double wv = Vect.dotProduct(edgev1v3, tmp);
        double d = uv * uv - uu * vv;

        double s = (uv * wv - vv * wu) / d;
        if (s < 0 || s > 1) {
            return false;
        }

        double t = (uv * wu - uu * wv) / d;
        if (t < 0 || s + t > 1) {
            return false;
        }

        return true;
    }

    @Override
    public void rotate(Matrix4 rotationMatrix) {
        VectMatrix.multiply(edgev1v2, rotationMatrix, edgev1v2);
        VectMatrix.multiply(edgev1v3, rotationMatrix, edgev1v3);

        Vect.crossProduct(edgev1v2, edgev1v3, this.lookAt);
    }

    @Override
    public String toString() {
        return super.toString() + " dir=[" + edgev1v2 + "/" + edgev1v3 + "]";
    }
    
	@Override
    public Vect3 getBoundingSphereCenter(){
    	return Vect.avg(position, v2, v3);
    }
    
	@Override
    public double getBoundingSphereRadius(){
		Vect3 avg = Vect.avg(position, v2, v3);
		Vect.subtract(avg, v3, avg);
		
    	return avg.length();
    }
}
