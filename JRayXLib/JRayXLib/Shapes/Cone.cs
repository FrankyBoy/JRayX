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
            Vect.Scale(axis, 1 / AxisLength, ref axis);
            Color = color;
        }
	
        public override void Rotate(Matrix4 rotationMatrix) {
            VectMatrix.Multiply(LookAt, rotationMatrix, ref _lookAt);

            LookAt.Normalize();
        }
	
        public override double GetHitPointDistance(Ray r) {
            return RayCone.GetRayConeIntersectionDistance(r.GetOrigin(), r.GetDirection(), Position, LookAt, CosPhi, AxisLength);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            var tmp = Vect.Subtract(hitPoint, Position);

            Vect3 tmp2 = Vect.CrossProduct(tmp, LookAt);
            return Vect.CrossProduct(tmp, tmp2).Normalize();
        }
	
        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("not implemented");
        }

        public new Sphere GetBoundingSphere(){
            var @base =  new Vect3( Position.Data[0]+LookAt.Data[0]*AxisLength,
                                     Position.Data[1]+LookAt.Data[1]*AxisLength,
                                     Position.Data[2]+LookAt.Data[2]*AxisLength);

            var normal = new Vect3(@base.Data[2], @base.Data[0], @base.Data[1]);
            var tmp = new Vect3(0);
            Vect.Project(normal, LookAt, ref tmp);
            normal = Vect.Subtract(normal, tmp);
            normal.Normalize();
            double l = CosPhi*AxisLength;

            Vect.AddMultiple(@base, normal, l, ref tmp);
            Vect.AddMultiple(@base, normal, -l, ref  @base);
		
            var center = new Vect3((Position.Data[0]+tmp.Data[0]+@base.Data[0])/3,
                                     (Position.Data[1]+tmp.Data[1]+@base.Data[1])/3,
                                     (Position.Data[2]+tmp.Data[2]+@base.Data[2])/3);

            tmp = Vect.Subtract(center, Position);
		
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
