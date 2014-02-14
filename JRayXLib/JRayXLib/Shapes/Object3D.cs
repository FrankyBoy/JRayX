using System;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    abstract public class Object3D {

        protected Color Color; //ARGB
        public Vect3 Position { get; set; }
        protected Vect3 LookAt { get; set; }
        protected double Reflectivity; // 0 means no reflection, 1 means everything is reflected

        /**
         * Creates an Object3D. All subclasses must use this constructor as super-constructor.
         *
         * @param position the position of this object
         * @param lookAt a vector specifying the heading of this object
         * @param rotationRad an angle defining the rotation of this object. 0 means that the Y-axis is used as up-Vector.
         */

        protected Object3D(Vect3 position, Vect3 lookAt, double rotationRad) : this(position, lookAt) {
            Rotate(lookAt, rotationRad);
        }

        protected Object3D(Vect3 position, Vect3 lookAt) {
            Position = position;
            LookAt = lookAt;
            Color = Color.Red;
        }

        public void Rotate(Vect3 axis, double rad) {
            var tmp = new Matrix4();
            Matrix.CreateRotationMatrix(axis, rad, tmp);
            Rotate(tmp);
        }

        /**
     * Rotate an object with this matrix (whatever this means for the object)
     * @param tmp
     */
        public abstract void Rotate(Matrix4 rotationMatrix);

        /**
     * Calculates if and where the given ray hits this object. Value must always
     * be bigger than 0 (objects at 0 distance are skipped)
     *
     * @param r ray
     * @return the hit distance. If there was no hit Double.POSITIVE_INFINITY is returned.
     */
        public abstract double GetHitPointDistance(Ray r);

        /**
     * Gets the "native" color of this object at the specified point.
     * This method does not take care of reflections, lighting or any other surface properties.
     *
     * @param hitPoint a point on the surface of this object
     * @return the color at the given point
     */
        public Color GetColorAt(Vect3 hitPoint) {
            return Color;
        }

        /**
     * Calculates and returns the normal at the given hitPoint.
     * <p>
     * The normal calculated by this method MUST be normalized!
     *
     * @param hitPoint a point on the surface of this object
     * @param normal a buffer in which the calculated normal will be stored
     */
        public abstract void GetNormalAt(Vect3 hitPoint, Vect3 normal);

        public void SetColor(Color color)
        {
            Color = color;
        }

        public void SetColor(byte[] color) {
            SetColor(new Color { A = color[0], R = color[1], G = color[2], B = color[3] });
        }

        public double GetReflectivityAt(Vect3 hitPoint) {
            return Reflectivity;
        }

        public void SetReflectivity(double reflectivity) {
            Reflectivity = reflectivity;
        }

        public override string ToString()
        {
            return GetType().Name + "@" + Position;
        }

        /**
     * returns true if the point is part of the object
     * @param hitPoint
     * @return
     */
        public abstract bool Contains(Vect3 hitPoint);
    
        /**
     * Returns the refraction index (when entering the object)
     */
        public double GetRefractionIndex(){
            return 1.03;
        }
    
        public Sphere GetBoundingSphere(){
            return new Sphere(GetBoundingSphereCenter(),GetBoundingSphereRadius(),Color.Black);
        }
    
        public Vect3 GetBoundingSphereCenter(){
            return Position;
        }
    
        public double GetBoundingSphereRadius(){
            throw new Exception("not implemented");
        }
    
        /**
     * Returns true if this objects is completely enclosed by this cube
     * 
     * @param cCenter center of the cube
     * @param cWidthHalf half width of the cube
     * @return
     */
        public bool IsEnclosedByCube(Vect3 cCenter, double cWidthHalf){
            Sphere s = GetBoundingSphere();
    	
            return CubeSphere.IsSphereEnclosedByCube(cCenter, cWidthHalf, s.Position, s.GetRadius());
        }
    }
}
