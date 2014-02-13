using JRayXLib.Math;

namespace JRayXLib.Common
{
    public class Sky : Object3D {
        readonly Texture _texture;

        public Sky(string texture) : base(null, null){
            _texture = Texture.Load(texture);
        }

        public Sky(uint color) : base(null, null, 0){
            Color = color;
        }

        public override double GetHitPointDistance(Ray r) {
            return double.PositiveInfinity;
        }

        public new int GetColorAt(Vect3 hitPoint) {
            double[] hpdat = hitPoint.GetData();

            double x = System.Math.Acos(hpdat[1] / hitPoint.Length()) / System.Math.PI;
            double y = System.Math.Acos(hpdat[2] / hitPoint.Length()) / System.Math.PI;

            return _texture.GetColorAt(x, y);
        }

        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal) {
            Vect.Invert(hitPoint, normal); // the normal is everywhere the vect pt -> 0
        }

        public override bool Contains(Vect3 hitPoint) {
            return false;
        }

        public override void Rotate(Matrix4 rotationMatrix) {
        }
    }
}
