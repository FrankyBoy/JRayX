using System;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Util;

namespace JRayXLib.Shapes
{
    public class Cone : Basic3DObject {

        public double CosPhi;
        public double AxisLength;

        public Cone(Vect3 position, Vect3 axis, double phiDegree, Color color)
            : base(position, axis)
        {
            CosPhi = System.Math.Cos(MathHelper.ToRadians(phiDegree));
            AxisLength = axis.Length();
            Vect.Scale(axis, 1/AxisLength, axis);
            Color = color;
        }
	
        public override void Rotate(Matrix4 rotationMatrix) {
            VectMatrix.multiply(LookAt, rotationMatrix, LookAt);

            LookAt.normalize();
        }
	
        public override double GetHitPointDistance(Ray r) {
            return RayCone.GetRayConeIntersectionDistance(r.GetOrigin(), r.GetDirection(), Position, LookAt, CosPhi, AxisLength);
        }
	
        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal) {
            var tmp = new Vect3();
            var tmp2 = new Vect3();
            Vect.subtract(hitPoint, Position, tmp);
		
            Vect.crossProduct(tmp, LookAt, tmp2);
            Vect.crossProduct(tmp, tmp2, normal);
            normal.normalize();
        }
	
        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("not implemented");
        }

        public new Sphere GetBoundingSphere(){
            var @base =  new Vect3(Position.Data[0]+LookAt.Data[0]*AxisLength,
                                     Position.Data[1]+LookAt.Data[1]*AxisLength,
                                     Position.Data[2]+LookAt.Data[2]*AxisLength);

            var normal = new Vect3();
            normal.Data[0] = @base.Data[2];
            normal.Data[1] = @base.Data[0];
            normal.Data[2] = @base.Data[1];
            var tmp = new Vect3();
            Vect.Project(normal, LookAt, tmp);
            Vect.subtract(normal, tmp, normal);
            normal.normalize();
            double l = CosPhi*AxisLength;
		
            Vect.AddMultiple(@base, normal, l, tmp);
            Vect.AddMultiple(@base, normal, -l, @base);
		
            var center = new Vect3((Position.Data[0]+tmp.Data[0]+@base.Data[0])/3,
                                     (Position.Data[1]+tmp.Data[1]+@base.Data[1])/3,
                                     (Position.Data[2]+tmp.Data[2]+@base.Data[2])/3);
		
            Vect.subtract(center, Position, tmp);
		
            return new Sphere(center,tmp.Length(),Color.Black);
        }

        public override double GetBoundingSphereRadius()
        {
            return GetBoundingSphere().GetRadius();
        }

        public new bool IsEnclosedByCube(Vect3 bcenter, double w2) {
            return CubeCone.IsCubeEnclosingCone(bcenter, w2, Position, LookAt, AxisLength, CosPhi);
        }
    }
}
