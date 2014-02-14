using System;
using System.Text;
using System.Text.RegularExpressions;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class Vect3 {

        public override int GetHashCode()
        {
            return (Data != null ? Data.GetHashCode() : 0);
        }

        public readonly double[] Data;

        public Vect3(double a, double b, double c) : this(new[]{a, b, c}) {}

        public Vect3(double[] data) {
            if (data.Length == 3) {
                Data = data;
            } else {
                throw new Exception("data.length muss 3 sein");
            }
        }

        public Vect3(Vect3 old) {
            double[] dat = old.GetData();
            Data = new[]{dat[0], dat[1], dat[2]};
        }

        public Vect3() : this(new double[3]) {}

        public double[] GetData() {
            return Data;
        }

        public override string ToString() {
            
            var sb = new StringBuilder("(");

            foreach (double d in Data) {
                sb.Append(string.Format("{0:0.##} ", d));
            }
            sb.Append(")");
            return sb.ToString();
        }

        public bool Equals(Vect3 v, double eps){
            return System.Math.Abs(Data[0]-v.Data[0])<eps
                && System.Math.Abs(Data[1] - v.Data[1]) < eps
                && System.Math.Abs(Data[2] - v.Data[2]) < eps;
        }
    
        public override bool Equals(Object o) {
            if (!(o is Vect3)) {
                return false;
            }

            for (int i = 0; i < 3; i++) {
                if (System.Math.Abs(Data[i] - ((Vect3)o).GetData()[i]) > Constants.EPS)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool Equals(Vect3 other)
        {
            return Equals(Data, other.Data);
        }


        public double Length() {
            return System.Math.Sqrt(Data[0] * Data[0] + Data[1] * Data[1] + Data[2] * Data[2]);
        }

        public double QuadLength() {
            return Data[0] * Data[0] + Data[1] * Data[1] + Data[2] * Data[2];
        }

        public void normalize() {
            double len = Length();
            Data[0] /= len;
            Data[1] /= len;
            Data[2] /= len;
        }

        public bool SameDirection(Vect3 v2) {
            double d = Data[0] / v2.Data[0]; // scale-factor

            return System.Math.Abs(Data[1] / v2.Data[1] - d) < Constants.EPS &&
                   System.Math.Abs(Data[2] / v2.Data[2] - d) < Constants.EPS;
        }

        public double Get(int i) {
            return Data[i];
        }

        public void CopyDataTo(Vect3 v) {
            double[] vd = v.GetData();
            vd[0] = Data[0];
            vd[1] = Data[1];
            vd[2] = Data[2];
        }

        static readonly Regex BasePatern = new Regex("\\[\\s*[+-]?[0-9]+(\\.[0-9]+)?\\s+[+-]?[0-9]+(\\.[0-9]+)?\\s+[+-]?[0-9]+(\\.[0-9]+)?\\s*\\]");
        static readonly Regex NumPatern = new Regex("\\s+");

        public static Vect3 ParseVect3(String s) {
            s = s.Trim();

            if (BasePatern.IsMatch(s)) {
                s = s.Replace("[\\[\\]]", "").Trim(); // kill the braces
                String[] field = NumPatern.Split(s);
                return new Vect3(Double.Parse(field[0]),
                                 Double.Parse(field[1]),
                                 Double.Parse(field[2]));
            }
            throw new Exception("String " + s + " has wrong format");
        }
    }
}
