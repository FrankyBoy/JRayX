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
            normal.normalize();
        }

        public override double GetHitPointDistance(Ray r) {
            double ret = RayPlane.GetHitPointRayPlaneDistance(r.GetOrigin(), r.GetDirection(), Position, LookAt);
            if (ret <= Constants.MinDistance) {
                return double.PositiveInfinity;
            }
            return ret;
        }

        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal) {
            LookAt.CopyDataTo(normal);
        }

        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("Not supported yet.");
        }

        public override void Rotate(Matrix4 tmp) {
            VectMatrix.multiply(LookAt, tmp, LookAt);
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

