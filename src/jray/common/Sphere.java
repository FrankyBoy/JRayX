package jray.common;

import jray.math.Vect;
import jray.math.VectMatrix;
import jray.math.intersections.RaySphere;

/**
 * Shpere implementation
 * Position is the sphere's center
 * LookAt is the direction of the north pole
 */
public class Sphere extends Object3D {

    protected double radius;
    protected Vect3 rotVect;

    public Sphere(Vect3 position, double radius, int color) {
        this(position, radius, color, 0);
    }

    public Sphere(Vect3 position, double radius, int color, double reflectivity) {
        this(position, new Vect3(0, radius, 0), 0, color);
        this.reflectivity = reflectivity;
    }

    public Sphere(Vect3 position, Vect3 lookAt, double rotationRad, int color, double reflectivity) {
        this(position, lookAt, rotationRad, color);
        this.reflectivity = reflectivity;
    }

    public Sphere(Vect3 position, Vect3 lookAt, double rotationRad, int color) {
        super(position, lookAt);

        this.color = color;
        this.radius = lookAt.length();

        // calculate the rotation of the 0-meridian
        this.rotVect = new Vect3(lookAt);

        double[] rVData = rotVect.getData();

        // check so we don't end up with two linear dependent vectors
        if(rVData[1] != 0 && rVData[0] == 0 && rVData[2] == 0) {
            rVData[0] += 1;
        } else {
            rVData[1] += 1;
        }
        Vect.crossProduct(rotVect, lookAt, rotVect);
        this.rotVect.normalize();
        this.lookAt.normalize();
        this.rotate(lookAt, rotationRad);
    }

    @Override
    public double getHitPointDistance(Ray r) {
        return RaySphere.getHitPointRaySphereDistance(r.getOrigin(), r.getDirection(), position, radius);
    }

    @Override
    public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
        Vect.subtract(hitPoint, position, normal);
        normal.normalize();
    }

    @Override
    public boolean contains(Vect3 hitPoint) {
        return Vect.distance(hitPoint, position) == radius;
    }

    @Override
    public void rotate(Matrix4 rotationMatrix) {
        VectMatrix.multiply(lookAt, rotationMatrix, lookAt);
        VectMatrix.multiply(rotVect, rotationMatrix, rotVect);

        lookAt.normalize();
        rotVect.normalize();
    }

    @Override
    public Sphere getBoundingSphere(){
    	return this;
    }
    
    public double getRadius() {
		return radius;
	}
}
