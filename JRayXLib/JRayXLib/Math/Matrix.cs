using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public static class Matrix
    {
        /**
     * creates a rotation matrix for affine transformation
     * @param axis axis to Rotate
     * @param angleRad angle to Rotate
     * @param erg Matrix4 object to store the result
     */

        public static Matrix4 CreateRotationMatrix(Vect3 axis, double angleRad)
        {
            double cosa = System.Math.Cos(angleRad);
            double sina = System.Math.Sin(angleRad);

            double icosa = 1 - cosa; // performance ... every bit matters

            double v1xICosA = axis.X*icosa;

            return new Matrix4
                {
                    A0 = cosa + axis.X*v1xICosA,
                    A1 = axis.Y*v1xICosA - axis.Z*sina,
                    A2 = axis.Z*v1xICosA + axis.Y*sina,
                    B0 = axis.Y*v1xICosA + axis.Z*sina,
                    B1 = cosa + axis.Y*axis.Y*icosa,
                    B2 = axis.Y*axis.Z*icosa - axis.X*sina,
                    C0 = axis.Z*v1xICosA - axis.Y*sina,
                    C1 = axis.Z*axis.Y*icosa + axis.X*sina,
                    C2 = cosa + axis.Z*axis.Z*icosa,
                    D3 = 1
                };
        }

        public static Matrix4 CreateTranslationMatrix(Vect3 axis)
        {
            return new Matrix4
                {
                    A0 = 1,
                    B1 = 1,
                    C2 = 1,
                    D3 = 1,
                    A3 = axis.X,
                    B3 = axis.Y,
                    C3 = axis.Z
                };
        }

        public static Matrix4 CreateScaleMatrix(Vect3 axis)
        {
            return new Matrix4
                {
                    A0 = axis.X,
                    B1 = axis.Y,
                    C2 = axis.Z,
                    D3 = 1
                };
        }

        public static Matrix4 CreateUnitMatrix()
        {
            return new Matrix4
                {
                    A0 = 1,
                    B1 = 1,
                    C2 = 1,
                    D3 = 1
                };
        }

        public static Matrix4 Invert(Matrix4 m)
        {

            double detA = m.A0* ( m.B1 *m.C2 *m.D3 + m.B2 *m.C3 *m.D1 + m.B3 *m.C1 *m.D2 ) +
                          m.A1* ( m.B0 *m.C3 *m.D2 + m.B2 *m.C0 *m.D3 + m.B3 *m.C2 *m.D0 ) +
                          m.A2* ( m.B0 *m.C1 *m.D3 + m.B1 *m.C3 *m.D0 + m.B3 *m.C0 *m.D1 ) +
                          m.A3* ( m.B0 *m.C2 *m.D1 + m.B1 *m.C0 *m.D2 + m.B2 *m.C1 *m.D0 ) -
                          m.A0* ( m.B1 *m.C3 *m.D2 + m.B2 *m.C1 *m.D3 + m.B3 *m.C2 *m.D1 ) -
                          m.A1* ( m.B0 *m.C2 *m.D3 + m.B2 *m.C3 *m.D0 + m.B3 *m.C0 *m.D2 ) -
                          m.A2* ( m.B0 *m.C3 *m.D1 + m.B1 *m.C0 *m.D3 + m.B3 *m.C1 *m.D0 ) -
                          m.A3* ( m.B0 *m.C1 *m.D2 + m.B1 *m.C2 *m.D0 + m.B2 *m.C0 *m.D1 );
            
            detA = 1/detA;

            var erg = new Matrix4
                {
                    A0 = (m.B1*m.C2*m.D3 + m.B2*m.C3*m.D1 + m.B3*m.C1*m.D2 -
                          m.B1*m.C3*m.D2 - m.B2*m.C1*m.D3 - m.B3*m.C2*m.D1)*detA,
                    A1 = (m.A1*m.C3*m.D2 + m.A2*m.C1*m.D3 + m.A3*m.C2*m.D1 -
                          m.A1*m.C2*m.D3 - m.A2*m.C3*m.D1 - m.A3*m.C1*m.D2)*detA,
                    A2 = (m.A1*m.B2*m.D3 + m.A2*m.B3*m.D1 + m.A3*m.B1*m.D2 -
                          m.A1*m.B3*m.D2 - m.A2*m.B1*m.D3 - m.A3*m.B2*m.D1)*detA,
                    A3 = (m.A1*m.B3*m.C2 + m.A2*m.B1*m.C3 + m.A3*m.B2*m.C1 -
                          m.A1*m.B2*m.C3 - m.A2*m.B3*m.C1 - m.A3*m.B1*m.C2)*detA,
                    B0 = (m.B0*m.C3*m.D2 + m.B2*m.C0*m.D3 + m.B3*m.C2*m.D0 -
                          m.B0*m.C2*m.D3 - m.B2*m.C3*m.D0 - m.B3*m.C0*m.D2)*detA,
                    B1 = (m.A0*m.C2*m.D3 + m.A2*m.C3*m.D0 + m.A3*m.C0*m.D2 -
                          m.A0*m.C3*m.D2 - m.A2*m.C0*m.D3 - m.A3*m.C2*m.D0)*detA,
                    B2 = (m.A0*m.B3*m.D2 + m.A2*m.B0*m.D3 + m.A3*m.B2*m.D0 -
                          m.A0*m.B2*m.D3 - m.A2*m.B3*m.D0 - m.A3*m.B0*m.D2)*detA,
                    B3 = (m.A0*m.B2*m.C3 + m.A2*m.B3*m.C0 + m.A3*m.B0*m.C2 -
                          m.A0*m.B3*m.C2 - m.A2*m.B0*m.C3 - m.A3*m.B2*m.C0)*detA,
                    C0 = (m.B0*m.C1*m.D3 + m.B1*m.C3*m.D0 + m.B3*m.C0*m.D1 -
                          m.B0*m.C3*m.D1 - m.B1*m.C0*m.D3 - m.B3*m.C1*m.D0)*detA,
                    C1 = (m.A0*m.C3*m.D1 + m.A1*m.C0*m.D3 + m.A3*m.C1*m.D0 -
                          m.A0*m.C1*m.D3 - m.A1*m.C3*m.D0 - m.A3*m.C0*m.D1)*detA,
                    C2 = (m.A0*m.B1*m.D3 + m.A1*m.B3*m.D0 + m.A3*m.B0*m.D1 -
                          m.A0*m.B3*m.D1 - m.A1*m.B0*m.D3 - m.A3*m.B1*m.D0)*detA,
                    C3 = (m.A0*m.B3*m.C1 + m.A1*m.B0*m.C3 + m.A3*m.B1*m.C0 -
                          m.A0*m.B1*m.C3 - m.A1*m.B3*m.C0 - m.A3*m.B0*m.C1)*detA,
                    D0 = (m.B0*m.C2*m.D1 + m.B1*m.C0*m.D2 + m.B2*m.C1*m.D0 -
                          m.B0*m.C1*m.D2 - m.B1*m.C2*m.D0 - m.B2*m.C0*m.D1)*detA,
                    D1 = (m.A0*m.C1*m.D2 + m.A1*m.C2*m.D0 + m.A2*m.C0*m.D1 -
                          m.A0*m.C2*m.D1 - m.A1*m.C0*m.D2 - m.A2*m.C1*m.D0)*detA,
                    D2 = (m.A0*m.B2*m.D1 + m.A1*m.B0*m.D2 + m.A2*m.B1*m.D0 -
                          m.A0*m.B1*m.D2 - m.A1*m.B2*m.D0 - m.A2*m.B0*m.D1)*detA,
                    D3 = (m.A0*m.B1*m.C2 + m.A1*m.B2*m.C0 + m.A2*m.B0*m.C1 -
                          m.A0*m.B2*m.C1 - m.A1*m.B0*m.C2 - m.A2*m.B1*m.C0)*detA
                };

            return erg;
        }

        public static Matrix4 Multiply(Matrix4 m1, Matrix4 m2)
        {
            return new Matrix4
                {
                    A0 = m1.A0*m2.A0 + m1.B0*m2.A1 + m1.C0*m2.A2 + m1.D0*m2.A3,
                    B0 = m1.A0*m2.B0 + m1.B0*m2.B1 + m1.C0*m2.B2 + m1.D0*m2.B3,
                    C0 = m1.A0*m2.C0 + m1.B0*m2.C1 + m1.C0*m2.C2 + m1.D0*m2.C3,
                    D0 = m1.A0*m2.D0 + m1.B0*m2.D1 + m1.C0*m2.D2 + m1.D0*m2.D3,
                    A1 = m1.A1*m2.A0 + m1.B1*m2.A1 + m1.C1*m2.A2 + m1.D1*m2.A3,
                    B1 = m1.A1*m2.B0 + m1.B1*m2.B1 + m1.C1*m2.B2 + m1.D1*m2.B3,
                    C1 = m1.A1*m2.C0 + m1.B1*m2.C1 + m1.C1*m2.C2 + m1.D1*m2.C3,
                    D1 = m1.A1*m2.D0 + m1.B1*m2.D1 + m1.C1*m2.D2 + m1.D1*m2.D3,
                    A2 = m1.A2*m2.A0 + m1.B2*m2.A1 + m1.C2*m2.A2 + m1.D2*m2.A3,
                    B2 = m1.A2*m2.B0 + m1.B2*m2.B1 + m1.C2*m2.B2 + m1.D2*m2.B3,
                    C2 = m1.A2*m2.C0 + m1.B2*m2.C1 + m1.C2*m2.C2 + m1.D2*m2.C3,
                    D2 = m1.A2*m2.D0 + m1.B2*m2.D1 + m1.C2*m2.D2 + m1.D2*m2.D3,
                    A3 = m1.A3*m2.A0 + m1.B3*m2.A1 + m1.C3*m2.A2 + m1.D3*m2.A3,
                    B3 = m1.A3*m2.B0 + m1.B3*m2.B1 + m1.C3*m2.B2 + m1.D3*m2.B3,
                    C3 = m1.A3*m2.C0 + m1.B3*m2.C1 + m1.C3*m2.C2 + m1.D3*m2.C3,
                    D3 = m1.A3*m2.D0 + m1.B3*m2.D1 + m1.C3*m2.D2 + m1.D3*m2.D3
                };
        }
    }
}