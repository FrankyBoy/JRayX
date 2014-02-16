using System;

namespace JRayXLib.Shapes
{
    public class Ray {

        private readonly Vect3 _origin;
        public Vect3 Direction { get; set; }

        public Ray() : this(new Vect3(0), new Vect3(0)){}

        /**
     * Creates a new Ray
     * 
     * @param origin of the ray
     * @param direction of the ray - MUST be normed to length 1 
     */
        public Ray(Vect3 origin, Vect3 direction) {
            _origin = origin;
            Direction = direction;
        }

        public Vect3 GetOrigin() {
            return _origin;
        }

        public override string ToString() {
            return "Ray origin=" + _origin + " dir=" + Direction;
        }
    }
}
