/**
 * Shpere implementation
 * position is the sphere's center
 * lookAt is the direction of the north pole
 */

using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    public class Sphere : Basic3DObject
    {
        protected double Radius;
        protected Vect3 RotVect;

        public Sphere(Vect3 position, double radius, Color color)
            : this(position, radius, color, 0)
        {
        }

        public Sphere(Vect3 position, double radius, Color color, double reflectivity)
            : this(position, new Vect3 { Y = radius }, 0, color)
        {
            Reflectivity = reflectivity;
        }

        public Sphere(Vect3 position, Vect3 lookAt, double rotationRad, Color color, double reflectivity)
            : this(position, lookAt, rotationRad, color)
        {
            Reflectivity = reflectivity;
        }

        public Sphere(Vect3 position, Vect3 lookAt, double rotationRad, Color color) : base(position, lookAt)
        {
            Color = color;
            Radius = lookAt.Length();

            // calculate the rotation of the 0-meridian
            RotVect = lookAt;

            // check so we don't end up with two linear dependent vectors
            if (System.Math.Abs(RotVect.Y) > Constants.EPS
                && System.Math.Abs(RotVect.X) < Constants.EPS
                && System.Math.Abs(RotVect.Z) < Constants.EPS)
            {
                RotVect.X += 1;
            }
            else
            {
                RotVect.Y += 1;
            }
            RotVect = Vect3Extensions.CrossProduct(RotVect, lookAt).Normalize();
            LookAt = LookAt.Normalize();
            Rotate(lookAt, rotationRad);
        }

        public override double GetHitPointDistance(Ray r)
        {
            return RaySphere.GetHitPointRaySphereDistance(r.Origin, r.Direction, Position, Radius);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            Vect3 tmp = hitPoint - Position;
            return tmp.Normalize();
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return System.Math.Abs(Vect3Extensions.Distance(hitPoint, Position) - Radius) < Constants.EPS;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            LookAt = VectMatrix.Multiply(LookAt, rotationMatrix);
            RotVect = VectMatrix.Multiply(RotVect, rotationMatrix);

            LookAt = LookAt.Normalize();
            RotVect = RotVect.Normalize();
        }

        public new Sphere GetBoundingSphere()
        {
            return this;
        }

        public override double GetBoundingSphereRadius()
        {
            return GetBoundingSphere().GetRadius();
        }

        public double GetRadius()
        {
            return Radius;
        }
    }
}