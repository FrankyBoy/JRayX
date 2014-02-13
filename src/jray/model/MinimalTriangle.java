package jray.model;

import jray.common.Matrix4;
import jray.common.Object3D;
import jray.common.Ray;
import jray.common.Sphere;
import jray.common.Vect3;
import jray.math.Triangle;
import jray.math.Vect;
import jray.math.intersections.RayTriangle;

public class MinimalTriangle extends Object3D{

	protected Vect3 v2;
	protected Vect3 v3;
	protected Sphere bounds;
	
	public MinimalTriangle(Vect3 normal, Vect3 v1, Vect3 v2, Vect3 v3) {
		super(v1, normal);
		this.v2 = v2;
		this.v3 = v3;
		
		Vect3 avg = getBoundingSphereCenter();
		Vect3 tmp = new Vect3();
		Vect.subtract(avg, v3, tmp);
    	
		bounds = new Sphere(avg,tmp.length(),0xFF000000);
	}

	@Override
	public boolean contains(Vect3 hitPoint) {
		throw new RuntimeException("not implemented");
	}

	@Override
	public double getHitPointDistance(Ray r) {
		Vect3 v1v2 = new Vect3();
		Vect3 v1v3 = new Vect3();
		
		Vect.subtract(v2, position, v1v2);
		Vect.subtract(v3, position, v1v3);
		
		double ret = RayTriangle.getHitPointRayTriangleDistance(r.getOrigin(), r.getDirection(), position, v1v2, v1v3);
        if (ret <= 0) {
            return Double.POSITIVE_INFINITY;
        }
        return ret;
	}

	@Override
	public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
		lookAt.copyDataTo(normal);
	}

	@Override
	public void rotate(Matrix4 rotationMatrix) {
		throw new RuntimeException("not implemented");
	}
	
    public Sphere getBoundingSphere(){
    	return bounds;
    }
	
	@Override
    public Vect3 getBoundingSphereCenter(){
		return Triangle.getCircumScribedCircleCenter(position, v2, v3);
    }
    
	@Override
    public double getBoundingSphereRadius(){
    	return bounds.getRadius();
    }
	
	public void purge(){
		bounds = null;
	}
}
