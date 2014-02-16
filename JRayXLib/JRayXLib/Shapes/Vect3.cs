using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public struct Vect3
    {
        public readonly double[] Data;
        
        public Vect3(double a = 0, double b = 0, double c = 0)
        {
            Data = new []{a, b, c};
        }

        public Vect3(Vect3 old) : this(old.Data[0], old.Data[1], old.Data[2]) { }

        public bool Equals(Vect3 v, double eps = Constants.EPS){
            return System.Math.Abs(Data[0] - v.Data[0]) < eps
                && System.Math.Abs(Data[1] - v.Data[1]) < eps
                && System.Math.Abs(Data[2] - v.Data[2]) < eps;
        }

        public double Length() {
            return System.Math.Sqrt(QuadLength());
        }

        public double QuadLength() {
            return Data[0] * Data[0] + Data[1] * Data[1] + Data[2] * Data[2];
        }

        public Vect3 Normalize() {
            double len = Length();
            Data[0] /= len;
            Data[1] /= len;
            Data[2] /= len;
            return this;
        }
        
        public void CopyDataTo(ref Vect3 other)
        {
            other.Data[0] = Data[0];
            other.Data[1] = Data[1];
            other.Data[2] = Data[2];
        }
    }
}
