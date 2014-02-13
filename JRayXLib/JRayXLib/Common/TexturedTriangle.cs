using JRayXLib.Math;
using JRayXLib.Math.intersections;

namespace JRayXLib.Common
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
            : base(v1, v2, v3, 0xFF00FF00){
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
        public new uint GetColorAt(Vect3 hitPoint) {
            return IntColors.mixSurfaceSurface(GetTextureColorAt(hitPoint), Color);
        }

        public new double GetReflectivityAt(Vect3 hitPoint) {
            double alpha = ((double)((GetTextureColorAt(hitPoint) >> 24) & 0xFF)) / 256;
            return (1 - alpha) * Reflectivity;
        }

        private uint GetTextureColorAt(Vect3 hitPoint) {
            var texcoord = new Vect2();
            Vect.InterpolateTriangle(Position, v2, v3, _t1, _t2, _t3, hitPoint, texcoord);
            return _texture.GetColorAt(texcoord);
        }
    }
}
