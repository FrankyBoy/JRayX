
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
            : this(position, radius, color, 0) {}

        public Sphere(Vect3 position, double radius, Color color, double reflectivity)
            : this(position, new Vect3(0, radius), 0, color)
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
            RotVect = new Vect3(lookAt);

            double[] rVData = RotVect.Data;

            // check so we don't end up with two linear dependent vectors
            if (System.Math.Abs(rVData[1] - 0) > Constants.EPS
                && System.Math.Abs(rVData[0] - 0) < Constants.EPS
                && System.Math.Abs(rVData[2] - 0) < Constants.EPS)
            {
                rVData[0] += 1;
            }
            else
            {
                rVData[1] += 1;
            }
            Vect.CrossProduct(RotVect, lookAt, ref RotVect);
            RotVect.Normalize();
            LookAt.Normalize();
            Rotate(lookAt, rotationRad);
        }

        public override double GetHitPointDistance(Ray r)
        {
            return RaySphere.GetHitPointRaySphereDistance(r.GetOrigin(), r.GetDirection(), Position, Radius);
        }

        public override void GetNormalAt(Vect3 hitPoint, ref Vect3 normal)
        {
            Vect.Subtract(hitPoint, Position, ref normal);
            normal.Normalize();
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return System.Math.Abs(Vect.Distance(hitPoint, ref _position) - Radius) < Constants.EPS;
        }
        
        public override void Rotate(Matrix4 rotationMatrix)
        {
            VectMatrix.Multiply(LookAt, rotationMatrix, ref _lookAt);
            VectMatrix.Multiply(RotVect, rotationMatrix, ref RotVect);

            LookAt.Normalize();
            RotVect.Normalize();
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