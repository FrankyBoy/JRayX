using System;
using JRayXLib.Colors;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class TexturedSphere : Sphere {

        private readonly Texture _texture;

        public TexturedSphere(Vect3 position, double radius, double rotation, String imagePath) 
            : this(position, radius, rotation, new Color(), 0, imagePath){}

        public TexturedSphere(Vect3 position, double radius, double rotation, Color color, double reflectivity, String imagePath) 
            : this(position, new Vect3(0, radius, 0), rotation, color, reflectivity, imagePath){}

        public TexturedSphere(Vect3 position, Vect3 lookAt, double rotation, String imagePath)
            : this(position, lookAt, rotation, new Color(), 0, imagePath) { }

        public TexturedSphere(Vect3 position, Vect3 lookAt, double rotation, Color color, double reflectivity, String imagePath)
            : base(position, lookAt, rotation, color, reflectivity)
        {
            _texture = Texture.Load(imagePath);
        }

        public new Color GetColorAt(Vect3 hitPoint) {
            return GetTextureColorAt(hitPoint).MixSurfaceSurface(Color);
        }

        public new double GetReflectivityAt(Vect3 hitPoint) {
            double alpha = GetTextureColorAt(hitPoint).A / 256.0;
            return (1 - alpha) * Reflectivity;
        }

        private Color GetTextureColorAt(Vect3 hitPoint) {
            // calculate x (longitude)
            var tmp = hitPoint - Position;
            tmp.Normalize();
            double y = System.Math.Acos(Vect.DotProduct(tmp, LookAt)) / System.Math.PI;

            // project to equator plane
            double dist = - Vect.DotProduct(tmp, LookAt);
            Vect.AddMultiple(tmp, LookAt, dist, ref tmp);
            tmp.Normalize();

            double x = System.Math.Acos(Vect.DotProduct(tmp, RotVect)) / (2 * System.Math.PI);
            tmp = Vect.CrossProduct(tmp, RotVect);
            if(Vect.DotProduct(tmp, LookAt) < 0) {
                x = 0.5 + x;
            } else {
                x = 0.5 - x;
            }

            return _texture.GetColorAt(x, y);
        }
    }
}
