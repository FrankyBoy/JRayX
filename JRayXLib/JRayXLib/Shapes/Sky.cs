using JRayXLib.Colors;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class Sky : Basic3DObject {
        readonly Texture _texture;

        public Sky(string texture) : base(null, null){
            _texture = Texture.Load(texture);
        }

        public Sky(Color color)
            : base(null, null, 0)
        {
            Color = color;
        }

        public override double GetHitPointDistance(Ray r) {
            return double.PositiveInfinity;
        }

        public new Color GetColorAt(Vect3 hitPoint)
        {
            double[] hpdat = hitPoint.Data;

            double x = System.Math.Acos(hpdat[1] / hitPoint.Length()) / System.Math.PI;
            double y = System.Math.Acos(hpdat[2] / hitPoint.Length()) / System.Math.PI;

            return _texture.GetColorAt(x, y);
        }

        public override void GetNormalAt(Vect3 hitPoint, ref Vect3 normal) {
            Vect.Invert(hitPoint, ref normal); // the normal is everywhere the vect pt -> 0
        }

        public override bool Contains(Vect3 hitPoint) {
            return false;
        }

        public override double GetBoundingSphereRadius()
        {
            throw new System.NotImplementedException();
        }

        public override void Rotate(Matrix4 rotationMatrix) {
        }
    }
}
