package jray.common;

import jray.math.Matrix;
import jray.math.intersections.CubeSphere;

abstract public class Object3D {

    protected int color; //ARGB
    protected Vect3 position;
    protected Vect3 lookAt;
    protected double reflectivity; // 0 means no reflection, 1 means everything is reflected

    /**
     * Creates an Object3D. All subclasses must use this constructor as super-constructor.
     *
     * @param position the position of this object
     * @param lookAt a vector specifying the heading of this object
     * @param rotationRad an angle defining the rotation of this object. 0 means that the Y-axis is used as up-Vector.
     */
    public Object3D(Vect3 position, Vect3 lookAt, double rotationRad) {
        this(position, lookAt);
        this.rotate(lookAt, rotationRad);
    }

    public Object3D(Vect3 position, Vect3 lookAt) {
        this.position = position;
        this.lookAt = lookAt;
        color = 0xFFFF0000;
    }

    public Vect3 getPosition() {
        return position;
    }

    public void setPosition(Vect3 position) {
        this.position = position;
    }

    public Vect3 getLookAt() {
        return lookAt;
    }

    public void setLookAt(Vect3 lookAt) {
        this.lookAt = lookAt;
        this.lookAt.normalize();
    }

    public void rotate(Vect3 axis, double rad) {
        Matrix4 tmp = new Matrix4();
        Matrix.createRotationMatrix(axis, rad, tmp);
        rotate(tmp);
    }

    /**
     * Rotate an object with this matrix (whatever this means for the object)
     * @param tmp
     */
    public abstract void rotate(Matrix4 rotationMatrix);

    /**
     * Calculates if and where the given ray hits this object. Value must always
     * be bigger than 0 (objects at 0 distance are skipped)
     *
     * @param r ray
     * @return the hit distance. If there was no hit Double.POSITIVE_INFINITY is returned.
     */
    public abstract double getHitPointDistance(Ray r);

    /**
     * Gets the "native" color of this object at the specified point.
     * This method does not take care of reflections, lighting or any other surface properties.
     *
     * @param hitPoint a point on the surface of this object
     * @return the color at the given point
     */
    public int getColorAt(Vect3 hitPoint) {
        return this.color;
    }

    /**
     * Calculates and returns the normal at the given hitPoint.
     * <p>
     * The normal calculated by this method MUST be normalized!
     *
     * @param hitPoint a point on the surface of this object
     * @param normal a buffer in which the calculated normal will be stored
     */
    public abstract void getNormalAt(Vect3 hitPoint, Vect3 normal);

    public void setColor(int color) {
        this.color = color;
    }

    public void setColor(byte[] color) {
        setColor(color[0] << 24 | color[1] << 16 | color[2] << 8 | color[3]);
    }

    public double getReflectivityAt(Vect3 hitPoint) {
        return reflectivity;
    }

    public void setReflectivity(double reflectivity) {
        this.reflectivity = reflectivity;
    }

    @Override
    public String toString() {
        return getClass().getSimpleName() + "@" + position;
    }

    /**
     * returns true if the point is part of the object
     * @param hitPoint
     * @return
     */
    public abstract boolean contains(Vect3 hitPoint);
    
    /**
     * Returns the refraction index (when entering the object)
     */
    public double getRefractionIndex(){
    	return 1.03;
    }
    
    public Sphere getBoundingSphere(){
    	return new Sphere(getBoundingSphereCenter(),getBoundingSphereRadius(),0xFF000000);
    }
    
    public Vect3 getBoundingSphereCenter(){
    	return position;
    }
    
    public double getBoundingSphereRadius(){
    	throw new RuntimeException("not implemented");
    }
    
    /**
     * Returns true if this objects is completely enclosed by this cube
     * 
     * @param cCenter center of the cube
     * @param cWidthHalf half width of the cube
     * @return
     */
    public boolean isEnclosedByCube(Vect3 cCenter, double cWidthHalf){
    	Sphere s = getBoundingSphere();
    	
    	return CubeSphere.isSphereEnclosedByCube(cCenter, cWidthHalf, s.getPosition(), s.getRadius());
    }
}
