using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    public abstract class Basic3DObject : I3DObject
    {

        protected Color Color;
        public Vect3 Position { get; set; }
        public Vect3 LookAt { get; set; }
        protected double Reflectivity;

        protected Basic3DObject(Vect3 position, Vect3 lookAt, double rotationRad) : this(position, lookAt) {
            Rotate(lookAt, rotationRad);
        }

        protected Basic3DObject(Vect3 position, Vect3 lookAt) {
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

        public abstract double GetHitPointDistance(Ray r);

        public Color GetColorAt(Vect3 hitPoint) {
            return Color;
        }

        public abstract void GetNormalAt(Vect3 hitPoint, Vect3 normal);


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

        public abstract bool Contains(Vect3 hitPoint);
    
        public double GetRefractionIndex(){
            return 1.03;
        }
    
        public Sphere GetBoundingSphere(){
            return new Sphere(GetBoundingSphereCenter(),GetBoundingSphereRadius(),Color.Black);
        }
    
        public Vect3 GetBoundingSphereCenter(){
            return Position;
        }

        public abstract double GetBoundingSphereRadius();
    
       
        public bool IsEnclosedByCube(Vect3 cCenter, double cWidthHalf){
            Sphere s = GetBoundingSphere();
    	
            return CubeSphere.IsSphereEnclosedByCube(cCenter, cWidthHalf, s.Position, s.GetRadius());
        }
    }
}
