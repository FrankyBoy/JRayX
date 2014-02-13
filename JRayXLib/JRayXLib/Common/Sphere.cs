
/**
 * Shpere implementation
 * position is the sphere's center
 * lookAt is the direction of the north pole
 */

using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Common
{
    public class Sphere : Object3D
    {

        protected double Radius;
        protected Vect3 RotVect;

        public Sphere(Vect3 position, double radius, uint color)
            : this(position, radius, color, 0) {}

        public Sphere(Vect3 position, double radius, uint color, double reflectivity)
            : this(position, new Vect3(0, radius, 0), 0, color)
        {
            Reflectivity = reflectivity;
        }

        public Sphere(Vect3 position, Vect3 lookAt, double rotationRad, uint color, double reflectivity)
            : this(position, lookAt, rotationRad, color)
        {
            Reflectivity = reflectivity;
        }

        public Sphere(Vect3 position, Vect3 lookAt, double rotationRad, uint color) : base(position, lookAt)
        {
            Color = color;
            Radius = lookAt.Length();

            // calculate the rotation of the 0-meridian
            RotVect = new Vect3(lookAt);

            double[] rVData = RotVect.GetData();

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
            Vect.crossProduct(RotVect, lookAt, RotVect);
            RotVect.normalize();
            LookAt.normalize();
            Rotate(lookAt, rotationRad);
        }

        public override double GetHitPointDistance(Ray r)
        {
            return RaySphere.GetHitPointRaySphereDistance(r.GetOrigin(), r.GetDirection(), Position, Radius);
        }
        
        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal)
        {
            Vect.subtract(hitPoint, Position, normal);
            normal.normalize();
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return System.Math.Abs(Vect.Distance(hitPoint, Position) - Radius) < Constants.EPS;
        }
        
        public override void Rotate(Matrix4 rotationMatrix)
        {
            VectMatrix.multiply(LookAt, rotationMatrix, LookAt);
            VectMatrix.multiply(RotVect, rotationMatrix, RotVect);

            LookAt.normalize();
            RotVect.normalize();
        }

        public new Sphere GetBoundingSphere()
        {
            return this;
        }

        public double GetRadius()
        {
            return Radius;
        }
    }
}