package jray.common;

import jray.math.Constants;
import jray.math.VectMatrix;
import jray.math.intersections.RayPlane;

public class Plane extends Object3D {

    public Plane(Vect3 position, Vect3 normal, int color, double reflectivity) {
        this(position, normal);
        this.color = color;
        this.reflectivity = reflectivity;
    }

    public Plane(Vect3 position, Vect3 normal, int color) {
        this(position, normal);
        this.color = color;
    }

    public Plane(Vect3 position, Vect3 normal) {
        super(position, normal);
        normal.normalize();
    }

    @Override
    public double getHitPointDistance(Ray r) {
        double ret = RayPlane.getHitPointRayPlaneDistance(r.getOrigin(), r.getDirection(), position, lookAt);
        if (ret <= Constants.MIN_DISTANCE) {
            return Double.POSITIVE_INFINITY;
        }
        return ret;
    }

    @Override
    public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
        lookAt.copyDataTo(normal);
    }

    @Override
    public boolean contains(Vect3 hitPoint) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public void rotate(Matrix4 tmp) {
        VectMatrix.multiply(lookAt, tmp, lookAt);
    }

    @Override
	public Sphere getBoundingSphere() {
		return null;
	}

	@Override
	public double getBoundingSphereRadius() {
		return Double.POSITIVE_INFINITY;
	}
}
