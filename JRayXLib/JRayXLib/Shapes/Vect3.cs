using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public struct Vect3
    {
        private readonly double[] _data;

        public Vect3(double a = 0, double b = 0, double c = 0)
        {
            _data = new[] { a, b, c };
        }

        public Vect3(Vect3 old) : this(old._data[0], old._data[1], old._data[2]) { }

        public bool Equals(Vect3 v, double eps = Constants.EPS)
        {
            return System.Math.Abs(_data[0] - v._data[0]) < eps
                && System.Math.Abs(_data[1] - v._data[1]) < eps
                && System.Math.Abs(_data[2] - v._data[2]) < eps;
        }

        public double Length()
        {
            return System.Math.Sqrt(QuadLength());
        }

        public double QuadLength()
        {
            return _data[0] * _data[0] + _data[1] * _data[1] + _data[2] * _data[2];
        }

        // TODO: make this return a new vector and not scale itself.
        public Vect3 Normalize()
        {
            double len = Length();
            return new Vect3(
                _data[0] / len,
                _data[1] / len,
                _data[2] / len
            );
        }

        public void CopyDataTo(ref Vect3 other)
        {
            other._data[0] = _data[0];
            other._data[1] = _data[1];
            other._data[2] = _data[2];
        }

        #region operator overloads
        public static Vect3 operator -(Vect3 vec1, Vect3 vec2)
        {
            double[] v1 = vec1._data;
            double[] v2 = vec2._data;

            return new Vect3(
                v1[0] - v2[0],
                v1[1] - v2[1],
                v1[2] - v2[2]
            );
        }

        public static Vect3 operator +(Vect3 vec1, Vect3 vec2)
        {
            double[] v1 = vec1._data;
            double[] v2 = vec2._data;

            return new Vect3(
                v1[0] + v2[0],
                v1[1] + v2[1],
                v1[2] + v2[2]
            );
        }

        public static Vect3 operator *(Vect3 vec, double d)
        {
            double[] vdat = vec._data;

            return new Vect3(
                vdat[0] * d,
                vdat[1] * d,
                vdat[2] * d
            );
        }

        public static Vect3 operator /(Vect3 vec, double d)
        {
            double[] vdat = vec._data;

            return new Vect3(
                vdat[0] / d,
                vdat[1] / d,
                vdat[2] / d
            );
        }

        public double this[int i]
        {
            get { return _data[i]; }
            set { _data[i] = value; }
        }

        #endregion

    }
}
