using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class VectMatrix
    {
        public static Vect3 Multiply(Matrix4 m, Vect3 v)
        {
            double fourth = m.D0 * v.X + m.D1 * v.Y + m.D2 * v.Z + m.D3;

            if (System.Math.Abs(fourth - 0) > Constants.EPS)
            {
                return new Vect3
                {
                    X = (m.A0 * v.X + m.A1 * v.Y + m.A2 * v.Z + m.A3) / fourth,
                    Y = (m.B0 * v.X + m.B1 * v.Y + m.B2 * v.Z + m.B3) / fourth,
                    Z = (m.C0 * v.X + m.C1 * v.Y + m.C2 * v.Z + m.C3) / fourth
                };
            }
            return new Vect3();
        }

        public static Vect3 Multiply(Vect3 v, Matrix4 m)
        {
            double fourth = m.A3 * v.X + m.B3 * v.Y + m.C3 * v.Z + m.D3;

            if (System.Math.Abs(fourth - 0) > Constants.EPS)
            {
                return new Vect3
                    {
                        X = (m.A0 * v.X + m.B0 * v.Y + m.C0 * v.Z + m.D0) / fourth,
                        Y = (m.A1 * v.X + m.B1 * v.Y + m.C1 * v.Z + m.D1) / fourth,
                        Z = (m.A2 * v.X + m.B2 * v.Y + m.C2 * v.Z + m.D2) / fourth
                    };
            }
            return new Vect3();
        }
    }
}