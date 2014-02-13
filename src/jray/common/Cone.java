package jray.common;

import jray.math.Vect;
import jray.math.VectMatrix;
import jray.math.intersections.CubeCone;
import jray.math.intersections.RayCone;

public class Cone extends Object3D{

	public double cosPhi;
	public double axisLength;
	
	public Cone(Vect3 position, Vect3 axis, double phiDegree, int color) {
		super(position, axis);
		cosPhi = Math.cos(Math.toRadians(phiDegree));
		axisLength = axis.length();
		Vect.scale(axis, 1/axisLength, axis);
		this.color = color;
	}
	
	@Override
	public void rotate(Matrix4 rotationMatrix) {
		VectMatrix.multiply(lookAt, rotationMatrix, lookAt);

        lookAt.normalize();
	}
	
	@Override
	public double getHitPointDistance(Ray r) {
		return RayCone.getRayConeIntersectionDistance(r.getOrigin(), r.getDirection(), position, lookAt, cosPhi, axisLength);
	}
	
	@Override
	public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
		Vect3 tmp = new Vect3();
		Vect3 tmp2 = new Vect3();
		Vect.subtract(hitPoint, position, tmp);
		
		Vect.crossProduct(tmp, lookAt, tmp2);
		Vect.crossProduct(tmp, tmp2, normal);
		normal.normalize();
	}
	
	@Override
	public boolean contains(Vect3 hitPoint) {
		throw new RuntimeException("not implemented");
	}

	public Sphere getBoundingSphere(){
		Vect3 base =  new Vect3(position.data[0]+lookAt.data[0]*axisLength,
				position.data[1]+lookAt.data[1]*axisLength,
				position.data[2]+lookAt.data[2]*axisLength);

		Vect3 normal = new Vect3();
		normal.data[0] = base.data[2];
		normal.data[1] = base.data[0];
		normal.data[2] = base.data[1];
		Vect3 tmp = new Vect3();
		Vect.project(normal, lookAt, tmp);
		Vect.subtract(normal, tmp, normal);
		normal.normalize();
		double l = cosPhi*axisLength;
		
		Vect.addMultiple(base, normal, l, tmp);
		Vect.addMultiple(base, normal, -l, base);
		
		Vect3 center = new Vect3((position.data[0]+tmp.data[0]+base.data[0])/3,
						 (position.data[1]+tmp.data[1]+base.data[1])/3,
						 (position.data[2]+tmp.data[2]+base.data[2])/3);
		
		Vect.subtract(center, position, tmp);
		
		return new Sphere(center,tmp.length(),0xFF000000);
    }

	@Override
	public boolean isEnclosedByCube(Vect3 bcenter, double w2) {
		return CubeCone.isCubeEnclosingCone(bcenter, w2, position, lookAt, axisLength, cosPhi);
	}
}
