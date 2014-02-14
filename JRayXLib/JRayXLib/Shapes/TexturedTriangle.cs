using JRayXLib.Colors;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class TexturedTriangle : Triangle {

        /**
     * stores the texture
     */
        private readonly Texture _texture;
        /**
     * stores the texture coordinates
     */
        private readonly Vect2 _t1;
        private readonly Vect2 _t2;
        private readonly Vect2 _t3;

        public TexturedTriangle(Vect3 v1, Vect2 t1, Vect3 v2, Vect2 t2, Vect3 v3, Vect2 t3, string imagePath)
            : base(v1, v2, v3, Color.Blue){
            _t1 = t1;
            _t2 = t2;
            _t3 = t3;
        
            _texture = Texture.Load(imagePath);
            }

        /**
         * gets the color of the mapped texture at the given point
         * @param hitPoint a point on the surface of this triangle
         * @return the color of the mapped texture at the given point
         */
        public new Color GetColorAt(Vect3 hitPoint)
        {
            return GetTextureColorAt(hitPoint).MixSurfaceSurface(Color);
        }

        public new double GetReflectivityAt(Vect3 hitPoint) {
            double alpha = GetTextureColorAt(hitPoint).A / 256.0;
            return (1 - alpha) * Reflectivity;
        }

        private Color GetTextureColorAt(Vect3 hitPoint)
        {
            var texcoord = new Vect2();
            Vect.InterpolateTriangle(Position, v2, v3, _t1, _t2, _t3, hitPoint, texcoord);
            return _texture.GetColorAt(texcoord);
        }
    }
}
