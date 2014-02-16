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
            LookAt = axis / AxisLength;
            Color = color;
        }
	
        public override void Rotate(Matrix4 rotationMatrix) {
            VectMatrix.Multiply(LookAt, rotationMatrix, ref _lookAt);

            LookAt = LookAt.Normalize();
        }
	
        public override double GetHitPointDistance(Ray r) {
            return RayCone.GetRayConeIntersectionDistance(r.GetOrigin(), r.Direction, Position, LookAt, CosPhi, AxisLength);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            var tmp = hitPoint - Position;

            Vect3 tmp2 = Vect3Extensions.CrossProduct(tmp, LookAt);
            return Vect3Extensions.CrossProduct(tmp, tmp2).Normalize();
        }
	
        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("not implemented");
        }

        public new Sphere GetBoundingSphere(){
            var @base = new Vect3(Position.Data[0] + LookAt.Data[0] * AxisLength,
                                     Position.Data[1] + LookAt.Data[1] * AxisLength,
                                     Position.Data[2] + LookAt.Data[2] * AxisLength);

            var normal = new Vect3(@base.Data[2], @base.Data[0], @base.Data[1]);
            var tmp = Vect3Extensions.Project(normal, LookAt);
            normal -= tmp;
            normal = normal.Normalize();
            var len = CosPhi* AxisLength;

            tmp = @base + normal * len;
            @base = @base - normal * len;

            var center = new Vect3((Position.Data[0] + tmp.Data[0] + @base.Data[0]) / 3,
                                     (Position.Data[1] + tmp.Data[1] + @base.Data[1]) / 3,
                                     (Position.Data[2] + tmp.Data[2] + @base.Data[2]) / 3);

            tmp = center - Position;
		
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
