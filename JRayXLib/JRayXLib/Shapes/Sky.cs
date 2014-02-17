using System;
using JRayXLib.Colors;

namespace JRayXLib.Shapes
{
    public class Sky : Basic3DObject
    {
        private readonly Texture _texture;

        public Sky(string texture)
            : base(new Vect3(0, 0, 0), new Vect3(0, 0, 0))
        {
            _texture = Texture.Load(texture);
        }

        public Sky(Color color)
            : base(new Vect3(0, 0, 0), new Vect3(0, 0, 0))
        {
            Color = color;
        }

        public override double GetHitPointDistance(Ray r)
        {
            return double.PositiveInfinity;
        }

        public new Color GetColorAt(Vect3 hitPoint)
        {
            double hpLength = hitPoint.Length();
            double x = System.Math.Acos(hitPoint.Y/hpLength)/System.Math.PI;
            double y = System.Math.Acos(hitPoint.Z/hpLength)/System.Math.PI;

            return _texture.GetColorAt(x, y);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            return hitPoint*-1; // the normal is everywhere the vect pt -> 0
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return false;
        }

        public override double GetBoundingSphereRadius()
        {
            throw new NotImplementedException();
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
        }
    }
}