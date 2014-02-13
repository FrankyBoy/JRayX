using System;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Common
{
    public class TexturedSphere : Sphere {

        private readonly Texture _texture;

        public TexturedSphere(Vect3 position, double radius, double rotation, String imagePath) 
            : this(position, radius, rotation, 0, 0, imagePath){}

        public TexturedSphere(Vect3 position, double radius, double rotation, uint color, double reflectivity, String imagePath) 
            : this(position, new Vect3(0, radius, 0), rotation, color, reflectivity, imagePath){}

        public TexturedSphere(Vect3 position, Vect3 lookAt, double rotation, String imagePath)
            : this(position, lookAt, rotation, 0, 0, imagePath) {}

        public TexturedSphere(Vect3 position, Vect3 lookAt, double rotation, uint color, double reflectivity, String imagePath)
            : base(position, lookAt, rotation, color, reflectivity)
        {
            _texture = Texture.Load(imagePath);
        }

        public new uint GetColorAt(Vect3 hitPoint) {
            return IntColors.mixSurfaceSurface(GetTextureColorAt(hitPoint), Color);
        }

        public new double GetReflectivityAt(Vect3 hitPoint) {
            double alpha = ((double) ((GetTextureColorAt(hitPoint) >> 24) & 0xFF)) / 256;
            return (1 - alpha) * Reflectivity;
        }

        private uint GetTextureColorAt(Vect3 hitPoint) {
            // calculate x (longitude)
            var tmp = new Vect3();
            Vect.subtract(hitPoint, Position, tmp);
            tmp.normalize();
            double y = System.Math.Acos(Vect.dotProduct(tmp, LookAt)) / System.Math.PI;

            // project to equator plane
            double dist = - Vect.dotProduct(tmp, LookAt);
            Vect.AddMultiple(tmp, LookAt, dist, tmp);
            tmp.normalize();

            double x = System.Math.Acos(Vect.dotProduct(tmp, RotVect)) / (2 * System.Math.PI);
            Vect.crossProduct(tmp, RotVect, tmp);
            if(Vect.dotProduct(tmp, LookAt) < 0) {
                x = 0.5 + x;
            } else {
                x = 0.5 - x;
            }

            return _texture.GetColorAt(x, y);
        }
    }
}
