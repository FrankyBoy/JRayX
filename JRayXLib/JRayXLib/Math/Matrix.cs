using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class Matrix
    {
        /**
     * creates a rotation matrix for affine transformation
     * @param axis axis to Rotate
     * @param angleRad angle to Rotate
     * @param erg Matrix4 object to store the result
     */

        public static void CreateRotationMatrix(Vect3 axis, double angleRad, ref Matrix4 erg)
        {
            double[,] data = erg.GetData();
            double cosa = System.Math.Cos(angleRad);
            double sina = System.Math.Sin(angleRad);

            double icosa = 1 - cosa; // performance ... every bit matters

            double v1xICosA = axis.X*icosa;

            data[0, 0] = cosa + axis.X*v1xICosA;
            data[0, 1] = axis.Y*v1xICosA - axis.Z*sina;
            data[0, 2] = axis.Z*v1xICosA + axis.Y*sina;
            data[0, 3] = 0;
            data[1, 0] = axis.Y*v1xICosA + axis.Z*sina;
            data[1, 1] = cosa + axis.Y*axis.Y*icosa;
            data[1, 2] = axis.Y*axis.Z*icosa - axis.X*sina;
            data[1, 3] = 0;
            data[2, 0] = axis.Z*v1xICosA - axis.Y*sina;
            data[2, 1] = axis.Z*axis.Y*icosa + axis.X*sina;
            data[2, 2] = cosa + axis.Z*axis.Z*icosa;
            data[2, 3] = 0;
            data[3, 0] = 0;
            data[3, 1] = 0;
            data[3, 2] = 0;
            data[3, 3] = 1;
        }

        public static void CreateTranslationMatrix(Vect3 axis, ref Matrix4 erg)
        {
            double[,] data = erg.GetData();

            data[1, 0] = 0;
            data[2, 0] = 0;
            data[3, 0] = 0;
            data[0, 1] = 0;
            data[2, 1] = 0;
            data[3, 1] = 0;
            data[0, 2] = 0;
            data[1, 2] = 0;
            data[3, 2] = 0;

            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;
            data[3, 3] = 1;
            data[0, 3] = axis.X;
            data[1, 3] = axis.Y;
            data[2, 3] = axis.Z;
        }

        public static void CreateScaleMatrix(Vect3 axis, ref Matrix4 erg)
        {
            double[,] ergData = erg.GetData();

            ergData[0, 0] = axis.X;
            ergData[0, 1] = 0;
            ergData[0, 2] = 0;
            ergData[0, 3] = 0;
            ergData[1, 0] = 0;
            ergData[1, 1] = axis.Y;
            ergData[1, 2] = 0;
            ergData[1, 3] = 0;
            ergData[2, 0] = 0;
            ergData[2, 1] = 0;
            ergData[2, 2] = axis.Z;
            ergData[2, 3] = 0;
            ergData[3, 0] = 0;
            ergData[3, 1] = 0;
            ergData[3, 2] = 0;
            ergData[3, 3] = 1;
        }

        public static void CreateUnitMatrix(ref Matrix4 erg)
        {
            double[,] data = erg.GetData();
            data[0, 0] = 1;
            data[0, 1] = 0;
            data[0, 2] = 0;
            data[0, 3] = 0;
            data[1, 0] = 0;
            data[1, 1] = 1;
            data[1, 2] = 0;
            data[1, 3] = 0;
            data[2, 0] = 0;
            data[2, 1] = 0;
            data[2, 2] = 1;
            data[2, 3] = 0;
            data[3, 0] = 0;
            data[3, 1] = 0;
            data[3, 2] = 0;
            data[3, 3] = 1;
        }

        public static void Invert(Matrix4 m, ref Matrix4 erg)
        {
            double[,] a = m.GetData();
            double detA = a[0, 0]*(a[1, 1]*a[2, 2]*a[3, 3] +
                                   a[1, 2]*a[2, 3]*a[3, 1] +
                                   a[1, 3]*a[2, 1]*a[3, 2]) +
                          a[0, 1]*(a[1, 0]*a[2, 3]*a[3, 2] +
                                   a[1, 2]*a[2, 0]*a[3, 3] +
                                   a[1, 3]*a[2, 2]*a[3, 0]) +
                          a[0, 2]*(a[1, 0]*a[2, 1]*a[3, 3] +
                                   a[1, 1]*a[2, 3]*a[3, 0] +
                                   a[1, 3]*a[2, 0]*a[3, 1]) +
                          a[0, 3]*(a[1, 0]*a[2, 2]*a[3, 1] +
                                   a[1, 1]*a[2, 0]*a[3, 2] +
                                   a[1, 2]*a[2, 1]*a[3, 0]) -
                          a[0, 0]*(a[1, 1]*a[2, 3]*a[3, 2] +
                                   a[1, 2]*a[2, 1]*a[3, 3] +
                                   a[1, 3]*a[2, 2]*a[3, 1]) -
                          a[0, 1]*(a[1, 0]*a[2, 2]*a[3, 3] +
                                   a[1, 2]*a[2, 3]*a[3, 0] +
                                   a[1, 3]*a[2, 0]*a[3, 2]) -
                          a[0, 2]*(a[1, 0]*a[2, 3]*a[3, 1] +
                                   a[1, 1]*a[2, 0]*a[3, 3] +
                                   a[1, 3]*a[2, 1]*a[3, 0]) -
                          a[0, 3]*(a[1, 0]*a[2, 1]*a[3, 2] +
                                   a[1, 1]*a[2, 2]*a[3, 0] +
                                   a[1, 2]*a[2, 0]*a[3, 1]);

            detA = 1/detA;
            double[,] b = erg.GetData();
            b[0, 0] = (a[1, 1]*a[2, 2]*a[3, 3] + a[1, 2]*a[2, 3]*a[3, 1] + a[1, 3]*a[2, 1]*a[3, 2] -
                       a[1, 1]*a[2, 3]*a[3, 2] - a[1, 2]*a[2, 1]*a[3, 3] - a[1, 3]*a[2, 2]*a[3, 1])*detA;
            b[0, 1] = (a[0, 1]*a[2, 3]*a[3, 2] + a[0, 2]*a[2, 1]*a[3, 3] + a[0, 3]*a[2, 2]*a[3, 1] -
                       a[0, 1]*a[2, 2]*a[3, 3] - a[0, 2]*a[2, 3]*a[3, 1] - a[0, 3]*a[2, 1]*a[3, 2])*detA;
            b[0, 2] = (a[0, 1]*a[1, 2]*a[3, 3] + a[0, 2]*a[1, 3]*a[3, 1] + a[0, 3]*a[1, 1]*a[3, 2] -
                       a[0, 1]*a[1, 3]*a[3, 2] - a[0, 2]*a[1, 1]*a[3, 3] - a[0, 3]*a[1, 2]*a[3, 1])*detA;
            b[0, 3] = (a[0, 1]*a[1, 3]*a[2, 2] + a[0, 2]*a[1, 1]*a[2, 3] + a[0, 3]*a[1, 2]*a[2, 1] -
                       a[0, 1]*a[1, 2]*a[2, 3] - a[0, 2]*a[1, 3]*a[2, 1] - a[0, 3]*a[1, 1]*a[2, 2])*detA;
            b[1, 0] = (a[1, 0]*a[2, 3]*a[3, 2] + a[1, 2]*a[2, 0]*a[3, 3] + a[1, 3]*a[2, 2]*a[3, 0] -
                       a[1, 0]*a[2, 2]*a[3, 3] - a[1, 2]*a[2, 3]*a[3, 0] - a[1, 3]*a[2, 0]*a[3, 2])*detA;
            b[1, 1] = (a[0, 0]*a[2, 2]*a[3, 3] + a[0, 2]*a[2, 3]*a[3, 0] + a[0, 3]*a[2, 0]*a[3, 2] -
                       a[0, 0]*a[2, 3]*a[3, 2] - a[0, 2]*a[2, 0]*a[3, 3] - a[0, 3]*a[2, 2]*a[3, 0])*detA;
            b[1, 2] = (a[0, 0]*a[1, 3]*a[3, 2] + a[0, 2]*a[1, 0]*a[3, 3] + a[0, 3]*a[1, 2]*a[3, 0] -
                       a[0, 0]*a[1, 2]*a[3, 3] - a[0, 2]*a[1, 3]*a[3, 0] - a[0, 3]*a[1, 0]*a[3, 2])*detA;
            b[1, 3] = (a[0, 0]*a[1, 2]*a[2, 3] + a[0, 2]*a[1, 3]*a[2, 0] + a[0, 3]*a[1, 0]*a[2, 2] -
                       a[0, 0]*a[1, 3]*a[2, 2] - a[0, 2]*a[1, 0]*a[2, 3] - a[0, 3]*a[1, 2]*a[2, 0])*detA;
            b[2, 0] = (a[1, 0]*a[2, 1]*a[3, 3] + a[1, 1]*a[2, 3]*a[3, 0] + a[1, 3]*a[2, 0]*a[3, 1] -
                       a[1, 0]*a[2, 3]*a[3, 1] - a[1, 1]*a[2, 0]*a[3, 3] - a[1, 3]*a[2, 1]*a[3, 0])*detA;
            b[2, 1] = (a[0, 0]*a[2, 3]*a[3, 1] + a[0, 1]*a[2, 0]*a[3, 3] + a[0, 3]*a[2, 1]*a[3, 0] -
                       a[0, 0]*a[2, 1]*a[3, 3] - a[0, 1]*a[2, 3]*a[3, 0] - a[0, 3]*a[2, 0]*a[3, 1])*detA;
            b[2, 2] = (a[0, 0]*a[1, 1]*a[3, 3] + a[0, 1]*a[1, 3]*a[3, 0]*a[0, 3]*a[1, 0]*a[3, 1] -
                       a[0, 0]*a[1, 3]*a[3, 1] - a[0, 1]*a[1, 0]*a[3, 3] - a[0, 3]*a[1, 1]*a[3, 0])*detA;
            b[2, 3] = (a[0, 0]*a[1, 3]*a[2, 1] + a[0, 1]*a[1, 0]*a[2, 3] + a[0, 3]*a[1, 1]*a[2, 0] -
                       a[0, 0]*a[1, 1]*a[2, 3] - a[0, 1]*a[1, 3]*a[2, 0] - a[0, 3]*a[1, 0]*a[2, 1])*detA;
            b[3, 0] = (a[1, 0]*a[2, 2]*a[3, 1] + a[1, 1]*a[2, 0]*a[3, 2] + a[1, 2]*a[2, 1]*a[3, 0] -
                       a[1, 0]*a[2, 1]*a[3, 2] - a[1, 1]*a[2, 2]*a[3, 0] - a[1, 2]*a[2, 0]*a[3, 1])*detA;
            b[3, 1] = (a[0, 0]*a[2, 1]*a[3, 2] + a[0, 1]*a[2, 2]*a[3, 0] + a[0, 2]*a[2, 0]*a[3, 1] -
                       a[0, 0]*a[2, 2]*a[3, 1] - a[0, 1]*a[2, 0]*a[3, 2] - a[0, 2]*a[2, 1]*a[3, 0])*detA;
            b[3, 2] = (a[0, 0]*a[1, 2]*a[3, 1] + a[0, 1]*a[1, 0]*a[3, 2] + a[0, 2]*a[1, 1]*a[3, 0] -
                       a[0, 0]*a[1, 1]*a[3, 2] - a[0, 1]*a[1, 2]*a[3, 0] - a[0, 2]*a[1, 0]*a[3, 1])*detA;
            b[3, 3] = (a[0, 0]*a[1, 1]*a[2, 2] + a[0, 1]*a[1, 2]*a[2, 0] + a[0, 2]*a[1, 0]*a[2, 1] -
                       a[0, 0]*a[1, 2]*a[2, 1] - a[0, 1]*a[1, 0]*a[2, 2] - a[0, 2]*a[1, 1]*a[2, 0])*detA;
        }

        public static void Multiply(Matrix4 m1, Matrix4 m2, ref Matrix4 erg)
        {
            double[,] h = erg.GetData();

            double[,] a = m1.GetData();
            double[,] b = m2.GetData();

            h[0, 0] = a[0, 0]*b[0, 0] + a[1, 0]*b[0, 1] + a[2, 0]*b[0, 2] + a[3, 0]*b[0, 3];
            h[1, 0] = a[0, 0]*b[1, 0] + a[1, 0]*b[1, 1] + a[2, 0]*b[1, 2] + a[3, 0]*b[1, 3];
            h[2, 0] = a[0, 0]*b[2, 0] + a[1, 0]*b[2, 1] + a[2, 0]*b[2, 2] + a[3, 0]*b[2, 3];
            h[3, 0] = a[0, 0]*b[3, 0] + a[1, 0]*b[3, 1] + a[2, 0]*b[3, 2] + a[3, 0]*b[3, 3];

            h[0, 1] = a[0, 1]*b[0, 0] + a[1, 1]*b[0, 1] + a[2, 1]*b[0, 2] + a[3, 1]*b[0, 3];
            h[1, 1] = a[0, 1]*b[1, 0] + a[1, 1]*b[1, 1] + a[2, 1]*b[1, 2] + a[3, 1]*b[1, 3];
            h[2, 1] = a[0, 1]*b[2, 0] + a[1, 1]*b[2, 1] + a[2, 1]*b[2, 2] + a[3, 1]*b[2, 3];
            h[3, 1] = a[0, 1]*b[3, 0] + a[1, 1]*b[3, 1] + a[2, 1]*b[3, 2] + a[3, 1]*b[3, 3];

            h[0, 2] = a[0, 2]*b[0, 0] + a[1, 2]*b[0, 1] + a[2, 2]*b[0, 2] + a[3, 2]*b[0, 3];
            h[1, 2] = a[0, 2]*b[1, 0] + a[1, 2]*b[1, 1] + a[2, 2]*b[1, 2] + a[3, 2]*b[1, 3];
            h[2, 2] = a[0, 2]*b[2, 0] + a[1, 2]*b[2, 1] + a[2, 2]*b[2, 2] + a[3, 2]*b[2, 3];
            h[3, 2] = a[0, 2]*b[3, 0] + a[1, 2]*b[3, 1] + a[2, 2]*b[3, 2] + a[3, 2]*b[3, 3];

            h[0, 3] = a[0, 3]*b[0, 0] + a[1, 3]*b[0, 1] + a[2, 3]*b[0, 2] + a[3, 3]*b[0, 3];
            h[1, 3] = a[0, 3]*b[1, 0] + a[1, 3]*b[1, 1] + a[2, 3]*b[1, 2] + a[3, 3]*b[1, 3];
            h[2, 3] = a[0, 3]*b[2, 0] + a[1, 3]*b[2, 1] + a[2, 3]*b[2, 2] + a[3, 3]*b[2, 3];
            h[3, 3] = a[0, 3]*b[3, 0] + a[1, 3]*b[3, 1] + a[2, 3]*b[3, 2] + a[3, 3]*b[3, 3];
        }
    }
}