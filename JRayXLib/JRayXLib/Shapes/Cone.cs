using System;
using JRayXLib.Colors;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Util;

namespace JRayXLib.Shapes
{
    public class Cone : Basic3DObject
    {
        public double AxisLength;
        public double CosPhi;

        public Cone(Vect3 position, Vect3 axis, double phiDegree, Color color)
            : base(position, axis)
        {
            CosPhi = System.Math.Cos(MathHelper.ToRadians(phiDegree));
            AxisLength = axis.Length();
            LookAt = axis/AxisLength;
            Color = color;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            VectMatrix.Multiply(LookAt, rotationMatrix, ref _lookAt);

            LookAt = LookAt.Normalize();
        }

        public override double GetHitPointDistance(Ray r)
        {
            return RayCone.GetRayConeIntersectionDistance(r.Origin, r.Direction, Position, LookAt, CosPhi,
                                                          AxisLength);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            Vect3 tmp = hitPoint - Position;

            Vect3 tmp2 = Vect3Extensions.CrossProduct(tmp, LookAt);
            return Vect3Extensions.CrossProduct(tmp, tmp2).Normalize();
        }

        public override bool Contains(Vect3 hitPoint)
        {
            throw new Exception("not implemented");
        }

        public new Sphere GetBoundingSphere()
        {
            var @base = new Vect3(Position.X + LookAt.X*AxisLength,
                                  Position.Y + LookAt.Y*AxisLength,
                                  Position.Z + LookAt.Z*AxisLength);

            var normal = new Vect3(@base.Z, @base.X, @base.Y);
            Vect3 tmp = Vect3Extensions.Project(normal, LookAt);
            normal -= tmp;
            normal = normal.Normalize();
            double len = CosPhi*AxisLength;

            tmp = @base + normal*len;
            @base = @base - normal*len;

            var center = new Vect3((Position.X + tmp.X + @base.X)/3,
                                   (Position.Y + tmp.Y + @base.Y)/3,
                                   (Position.Z + tmp.Z + @base.Z)/3);

            tmp = center - Position;

            return new Sphere(center, tmp.Length(), Color.Black);
        }

        public override double GetBoundingSphereRadius()
        {
            return GetBoundingSphere().GetRadius();
        }

        public override bool IsEnclosedByCube(Vect3 bcenter, double w2)
        {
            return CubeCone.IsCubeEnclosingCone(bcenter, w2, Position, LookAt, AxisLength, CosPhi);
        }
    }
}