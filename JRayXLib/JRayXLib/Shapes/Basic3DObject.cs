using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    public abstract class Basic3DObject : I3DObject
    {
        protected Color Color;

        protected double Reflectivity;
        // ReSharper disable InconsistentNaming
        protected Vect3 _lookAt;
        protected Vect3 _position;
        // ReSharper restore InconsistentNaming

        protected Basic3DObject(Vect3 position, Vect3 lookAt, double rotationRad) : this(position, lookAt)
        {
            Rotate(lookAt, rotationRad);
        }

        protected Basic3DObject(Vect3 position, Vect3 lookAt)
        {
            Position = position;
            LookAt = lookAt;
            Color = Color.Red;
        }

        public virtual Vect3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vect3 LookAt
        {
            get { return _lookAt; }
            set { _lookAt = value; }
        }

        public void Rotate(Vect3 axis, double rad)
        {
            var tmp = new Matrix4();
            Matrix.CreateRotationMatrix(axis, rad, tmp);
            Rotate(tmp);
        }


        public virtual Color GetColorAt(Vect3 hitPoint)
        {
            return Color;
        }


        public virtual double GetReflectivityAt(Vect3 hitPoint)
        {
            return Reflectivity;
        }

        public virtual double GetRefractionIndex()
        {
            return 1.03;
        }

        public virtual Sphere GetBoundingSphere()
        {
            return new Sphere(GetBoundingSphereCenter(), GetBoundingSphereRadius(), Color.Black);
        }

        public virtual bool IsEnclosedByCube(Vect3 cCenter, double cWidthHalf)
        {
            Sphere s = GetBoundingSphere();

            return CubeSphere.IsSphereEnclosedByCube(cCenter, cWidthHalf, s.Position, s.GetRadius());
        }

        public abstract bool Contains(Vect3 hitPoint);
        public abstract void Rotate(Matrix4 rotationMatrix);
        public abstract Vect3 GetNormalAt(Vect3 hitPoint);
        public abstract double GetHitPointDistance(Ray r);

        public virtual void SetReflectivity(double reflectivity)
        {
            Reflectivity = reflectivity;
        }

        public virtual Vect3 GetBoundingSphereCenter()
        {
            return Position;
        }

        public abstract double GetBoundingSphereRadius();
    }
}