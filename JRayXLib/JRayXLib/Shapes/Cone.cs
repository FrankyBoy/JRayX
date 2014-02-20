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
            LookAt = VectMatrix.Multiply(LookAt, rotationMatrix).Normalize();
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
            var @base = Position + LookAt*AxisLength;
            Vect3 tmp = Vect3Extensions.Project(@base, LookAt);
            var normal = (@base - tmp).Normalize();
            double len = CosPhi*AxisLength;
            normal *= len;

            tmp = @base + normal;
            @base = @base - normal;

            var center = (Position + tmp + @base)/3;

            tmp = center - Position;

            return new Sphere(center, tmp.Length(), Color.Black);
        }

        public override double GetBoundingSphereRadius()
        {
            return GetBoundingSphere().Radius;
        }

        public override bool IsEnclosedByCube(Vect3 bcenter, double w2)
        {
            return CubeCone.IsCubeEnclosingCone(bcenter, w2, Position, LookAt, AxisLength, CosPhi);
        }
    }
}