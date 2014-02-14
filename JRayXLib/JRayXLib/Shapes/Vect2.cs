using System;
using System.Text;
using System.Text.RegularExpressions;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class Vect2 {
        

        public override int GetHashCode()
        {
            return (_data != null ? _data.GetHashCode() : 0);
        }

        private readonly double[] _data;

        public Vect2(double a, double b) : this(new []{a, b}) {
            
        }

        public Vect2(double[] data) {
            if (data.Length == 2) {
                _data = data;
            } else {
                throw new Exception("data.length muss 2 sein");
            }
        }

        public Vect2(Vect2 old) {
            double[] dat = old.GetData();
            _data = new[]{dat[0], dat[1]};
        }

        public Vect2() : this(new double[2]) { }

        public double[] GetData() {
            return _data;
        }

    
        public override string ToString() {
            var sb = new StringBuilder("(");

            foreach (double d in _data) {
                sb.Append(string.Format("{0:0.##} ", d));
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override bool Equals(Object o) {
            if (!(o is Vect2)) {
                return false;
            }

            for (int i = 0; i < 2; i++) {
                if (System.Math.Abs(_data[i] - ((Vect2) o).GetData()[i]) > Constants.EPS) {
                    return false;
                }
            }

            return true;
        }

        protected bool Equals(Vect2 other)
        {
            return Equals((Object) other);
        }

        public double Length() {
            return System.Math.Sqrt(_data[0] * _data[0] + _data[1] * _data[1]);
        }

        public double QuadLength() {
            return _data[0] * _data[0] + _data[1] * _data[1];
        }

        public void Normalize() {
            double len = Length();
            _data[0] /= len;
            _data[1] /= len;
        }

        public bool SameDirection(Vect2 v2) {
            double d = _data[0] / v2._data[0]; // scale-factor

            return System.Math.Abs(_data[1] / v2._data[1] - d) < Constants.EPS;
        }

        public double Get(int i) {
            return _data[i];
        }

        public void CopyDataTo(Vect2 v) {
            double[] vd = v.GetData();
            vd[0] = _data[0];
            vd[1] = _data[1];
        }



        static readonly Regex BasePatern = new Regex("\\[\\s*[+-]?[0-9]+(\\.[0-9]+)?\\s+[+-]?[0-9]+(\\.[0-9]+)?\\s*\\]");
        static readonly Regex NumPatern = new Regex("\\s+");

        public static Vect2 ParseVect2(String s) {
            s = s.Trim();
            
            // TODO: we can do this with a repetition group surely (X{n}), but I'm not sure about the whitespaces
            if (BasePatern.IsMatch(s))
            {
                s = s.Replace("[\\[\\]]", "").Trim(); // kill the braces
                String[] field = NumPatern.Split(s);
                return new Vect2(Double.Parse(field[0]),
                                 Double.Parse(field[1]));
            }

            throw new Exception("String " + s + " has wrong format");
        }
    }
}
