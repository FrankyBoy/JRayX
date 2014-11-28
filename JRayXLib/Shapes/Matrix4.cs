using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public struct Matrix4
    {
        public double
            A0, A1, A2, A3,
            B0, B1, B2, B3,
            C0, C1, C2, C3,
            D0, D1, D2, D3;

        public Matrix4(Matrix4 m)
        {
            A0 = m.A0;
            A1 = m.A1;
            A2 = m.A2;
            A3 = m.A3;

            B0 = m.B0;
            B1 = m.B1;
            B2 = m.B2;
            B3 = m.B3;

            C0 = m.C0;
            C1 = m.C1;
            C2 = m.C2;
            C3 = m.C3;

            D0 = m.D0;
            D1 = m.D1;
            D2 = m.D2;
            D3 = m.D3;
        }

        public bool Equals(Matrix4 other)
        {
            return
                System.Math.Abs(A0 - other.A0) < Constants.EPS &&
                System.Math.Abs(A1 - other.A1) < Constants.EPS &&
                System.Math.Abs(A2 - other.A2) < Constants.EPS &&
                System.Math.Abs(A3 - other.A3) < Constants.EPS &&
                System.Math.Abs(B0 - other.B0) < Constants.EPS &&
                System.Math.Abs(B1 - other.B1) < Constants.EPS &&
                System.Math.Abs(B2 - other.B2) < Constants.EPS &&
                System.Math.Abs(B3 - other.B3) < Constants.EPS &&
                System.Math.Abs(C0 - other.C0) < Constants.EPS &&
                System.Math.Abs(C1 - other.C1) < Constants.EPS &&
                System.Math.Abs(C2 - other.C2) < Constants.EPS &&
                System.Math.Abs(C3 - other.C3) < Constants.EPS &&
                System.Math.Abs(D0 - other.D0) < Constants.EPS &&
                System.Math.Abs(D1 - other.D1) < Constants.EPS &&
                System.Math.Abs(D2 - other.D2) < Constants.EPS &&
                System.Math.Abs(D3 - other.D3) < Constants.EPS;

        }
    }
}