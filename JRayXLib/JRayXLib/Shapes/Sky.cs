using JRayXLib.Colors;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class Sky : Basic3DObject {
        readonly Texture _texture;

        public Sky(string texture) : base(new Vect3(0), new Vect3(0)){
            _texture = Texture.Load(texture);
        }

        public Sky(Color color)
            : base(new Vect3(0), new Vect3(0))
        {
            Color = color;
        }

        public override double GetHitPointDistance(Ray r) {
            return double.PositiveInfinity;
        }

        public new Color GetColorAt(Vect3 hitPoint)
        {
            var hpLength = hitPoint.Length();
            double x = System.Math.Acos(hitPoint.Data[1] / hpLength) / System.Math.PI;
            double y = System.Math.Acos(hitPoint.Data[2] / hpLength) / System.Math.PI;

            return _texture.GetColorAt(x, y);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint) {

            return Vect.Invert(hitPoint); // the normal is everywhere the vect pt -> 0
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
