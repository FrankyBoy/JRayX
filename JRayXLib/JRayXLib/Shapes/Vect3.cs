using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public struct Vect3
    {
        public double X, Y, Z;


        public Vect3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vect3(double x, double y) : this(x, y, 0) { }
        public Vect3(Vect3 old) : this(old.X, old.Y, old.Z) { }

        public bool Equals(Vect3 v, double eps = Constants.EPS)
        {
            return System.Math.Abs(X - v.X) < eps
                   && System.Math.Abs(Y - v.Y) < eps
                   && System.Math.Abs(Z - v.Z) < eps;
        }

        public double Length()
        {
            return System.Math.Sqrt(QuadLength());
        }

        public double QuadLength()
        {
            return X*X + Y*Y + Z*Z;
        }

        public Vect3 Normalize()
        {
            double len = Length();
            return new Vect3(
                X/len,
                Y/len,
                Z/len
                );
        }

        public void CopyDataTo(ref Vect3 other)
        {
            other.X = X;
            other.Y = Y;
            other.Z = Z;
        }

        #region operator overloads

        public static Vect3 operator -(Vect3 v1, Vect3 v2)
        {
            return new Vect3(
                v1.X - v2.X,
                v1.Y - v2.Y,
                v1.Z - v2.Z
                );
        }

        public static Vect3 operator +(Vect3 v1, Vect3 v2)
        {
            return new Vect3(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z
                );
        }

        public static Vect3 operator *(Vect3 v, double d)
        {
            return new Vect3(
                v.X*d,
                v.Y*d,
                v.Z*d
                );
        }

        public static Vect3 operator /(Vect3 v, double d)
        {
            return new Vect3(
                v.X/d,
                v.Y/d,
                v.Z/d
                );
        }

        #endregion
    }
}