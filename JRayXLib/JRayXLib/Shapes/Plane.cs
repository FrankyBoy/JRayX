using System;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Shapes
{
    public class Plane : Basic3DObject {

        public Plane(Vect3 position, Vect3 normal, Color color, double reflectivity)
            : this(position, normal)
        {
            Color = color;
            Reflectivity = reflectivity;
        }

        public Plane(Vect3 position, Vect3 normal, Color color)
            : this(position, normal)
        {
            Color = color;
        }

        public Plane(Vect3 position, Vect3 normal) : base(position, normal){
            LookAt = LookAt.Normalize();
        }

        public override double GetHitPointDistance(Ray r) {
            double ret = RayPlane.GetHitPointRayPlaneDistance(r.GetOrigin(), r.Direction, Position, LookAt);
            if (ret <= Constants.MinDistance) {
                return double.PositiveInfinity;
            }
            return ret;
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            return new Vect3(LookAt);
        }

        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("Not supported yet.");
        }

        public override void Rotate(Matrix4 tmp) {
            VectMatrix.Multiply(LookAt, tmp, ref _lookAt);
        }

        public new Sphere GetBoundingSphere() {
            return null;
        }


        public override double GetBoundingSphereRadius()
        {
            return double.PositiveInfinity;
        }
    }
}

