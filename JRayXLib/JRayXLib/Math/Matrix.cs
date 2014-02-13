using JRayXLib.Common;

namespace JRayXLib.Math
{
    public class Matrix {

        /**
     * creates a rotation matrix for affine transformation
     * @param axis axis to Rotate
     * @param angleRad angle to Rotate
     * @param erg Matrix4 object to store the result
     */
        public static void CreateRotationMatrix(Vect3 axis, double angleRad, Matrix4 erg) {
            double[,] data = erg.GetData();
            double[] axisdata = axis.GetData();
            double cosa = System.Math.Cos(angleRad);
            double sina = System.Math.Sin(angleRad);

            double icosa = 1 - cosa; // performance ... every bit matters
            double v1xicosa = axisdata[0] * icosa;

            data[0,0] = cosa + axisdata[0] * v1xicosa;
            data[0,1] = axisdata[1] * v1xicosa - axisdata[2] * sina;
            data[0,2] = axisdata[2] * v1xicosa + axisdata[1] * sina;
            data[0,3] = 0;
            data[1,0] = axisdata[1] * v1xicosa + axisdata[2] * sina;
            data[1,1] = cosa + axisdata[1] * axisdata[1] * icosa;
            data[1,2] = axisdata[1] * axisdata[2] * icosa - axisdata[0] * sina;
            data[1,3] = 0;
            data[2,0] = axisdata[2] * v1xicosa - axisdata[1] * sina;
            data[2,1] = axisdata[2] * axisdata[1] * icosa + axisdata[0] * sina;
            data[2,2] = cosa + axisdata[2] * axisdata[2] * icosa;
            data[2,3] = 0;
            data[3,0] = 0;
            data[3,1] = 0;
            data[3,2] = 0;
            data[3,3] = 1;
        }

        public static void CreateTranslationMatrix(Vect3 axis, Matrix4 erg) {
            double[,] data = erg.GetData();

            double[] axisdata = axis.GetData();

            data[1,0] = 0;
            data[2,0] = 0;
            data[3,0] = 0;
            data[0,1] = 0;
            data[2,1] = 0;
            data[3,1] = 0;
            data[0,2] = 0;
            data[1,2] = 0;
            data[3,2] = 0;

            data[0,0] = 1;
            data[1,1] = 1;
            data[2,2] = 1;
            data[3,3] = 1;
            data[0,3] = axisdata[0];
            data[1,3] = axisdata[1];
            data[2,3] = axisdata[2];
        }

        public static void CreateScaleMatrix(Vect3 axis, Matrix4 erg) {
            double[,] ergData = erg.GetData();
            double[] axisData = axis.GetData();

            ergData[0,0] = axisData[0];
            ergData[0,1] = 0;
            ergData[0,2] = 0;
            ergData[0,3] = 0;
            ergData[1,0] = 0;
            ergData[1,1] = axisData[1];
            ergData[1,2] = 0;
            ergData[1,3] = 0;
            ergData[2,0] = 0;
            ergData[2,1] = 0;
            ergData[2,2] = axisData[2];
            ergData[2,3] = 0;
            ergData[3,0] = 0;
            ergData[3,1] = 0;
            ergData[3,2] = 0;
            ergData[3,3] = 1;
        }

        public static void CreateUnitMatrix(Matrix4 erg) {
            double[,] data = erg.GetData();
            data[0,0] = 1;
            data[0,1] = 0;
            data[0,2] = 0;
            data[0,3] = 0;
            data[1,0] = 0;
            data[1,1] = 1;
            data[1,2] = 0;
            data[1,3] = 0;
            data[2,0] = 0;
            data[2,1] = 0;
            data[2,2] = 1;
            data[2,3] = 0;
            data[3,0] = 0;
            data[3,1] = 0;
            data[3,2] = 0;
            data[3,3] = 1;
        }

        public static void Invert(Matrix4 m, Matrix4 erg) {
            double[,] a = m.GetData();
            double detA = a[0,0] * (a[1,1] * a[2,2] * a[3,3] +
                                    a[1,2] * a[2,3] * a[3,1] +
                                    a[1,3] * a[2,1] * a[3,2]) +
                          a[0,1] * (a[1,0] * a[2,3] * a[3,2] +
                                    a[1,2] * a[2,0] * a[3,3] +
                                    a[1,3] * a[2,2] * a[3,0]) +
                          a[0,2] * (a[1,0] * a[2,1] * a[3,3] +
                                    a[1,1] * a[2,3] * a[3,0] +
                                    a[1,3] * a[2,0] * a[3,1]) +
                          a[0,3] * (a[1,0] * a[2,2] * a[3,1] +
                                    a[1,1] * a[2,0] * a[3,2] +
                                    a[1,2] * a[2,1] * a[3,0]) -
                          a[0,0] * (a[1,1] * a[2,3] * a[3,2] +
                                    a[1,2] * a[2,1] * a[3,3] +
                                    a[1,3] * a[2,2] * a[3,1]) -
                          a[0,1] * (a[1,0] * a[2,2] * a[3,3] +
                                    a[1,2] * a[2,3] * a[3,0] +
                                    a[1,3] * a[2,0] * a[3,2]) -
                          a[0,2] * (a[1,0] * a[2,3] * a[3,1] +
                                    a[1,1] * a[2,0] * a[3,3] +
                                    a[1,3] * a[2,1] * a[3,0]) -
                          a[0,3] * (a[1,0] * a[2,1] * a[3,2] +
                                    a[1,1] * a[2,2] * a[3,0] +
                                    a[1,2] * a[2,0] * a[3,1]);

            detA = 1 / detA;
            double[,] b = erg.GetData();
            b[0,0] = (a[1,1] * a[2,2] * a[3,3] + a[1,2] * a[2,3] * a[3,1] + a[1,3] * a[2,1] * a[3,2] - a[1,1] * a[2,3] * a[3,2] - a[1,2] * a[2,1] * a[3,3] - a[1,3] * a[2,2] * a[3,1]) * detA;
            b[0,1] = (a[0,1] * a[2,3] * a[3,2] + a[0,2] * a[2,1] * a[3,3] + a[0,3] * a[2,2] * a[3,1] - a[0,1] * a[2,2] * a[3,3] - a[0,2] * a[2,3] * a[3,1] - a[0,3] * a[2,1] * a[3,2]) * detA;
            b[0,2] = (a[0,1] * a[1,2] * a[3,3] + a[0,2] * a[1,3] * a[3,1] + a[0,3] * a[1,1] * a[3,2] - a[0,1] * a[1,3] * a[3,2] - a[0,2] * a[1,1] * a[3,3] - a[0,3] * a[1,2] * a[3,1]) * detA;
            b[0,3] = (a[0,1] * a[1,3] * a[2,2] + a[0,2] * a[1,1] * a[2,3] + a[0,3] * a[1,2] * a[2,1] - a[0,1] * a[1,2] * a[2,3] - a[0,2] * a[1,3] * a[2,1] - a[0,3] * a[1,1] * a[2,2]) * detA;
            b[1,0] = (a[1,0] * a[2,3] * a[3,2] + a[1,2] * a[2,0] * a[3,3] + a[1,3] * a[2,2] * a[3,0] - a[1,0] * a[2,2] * a[3,3] - a[1,2] * a[2,3] * a[3,0] - a[1,3] * a[2,0] * a[3,2]) * detA;
            b[1,1] = (a[0,0] * a[2,2] * a[3,3] + a[0,2] * a[2,3] * a[3,0] + a[0,3] * a[2,0] * a[3,2] - a[0,0] * a[2,3] * a[3,2] - a[0,2] * a[2,0] * a[3,3] - a[0,3] * a[2,2] * a[3,0]) * detA;
            b[1,2] = (a[0,0] * a[1,3] * a[3,2] + a[0,2] * a[1,0] * a[3,3] + a[0,3] * a[1,2] * a[3,0] - a[0,0] * a[1,2] * a[3,3] - a[0,2] * a[1,3] * a[3,0] - a[0,3] * a[1,0] * a[3,2]) * detA;
            b[1,3] = (a[0,0] * a[1,2] * a[2,3] + a[0,2] * a[1,3] * a[2,0] + a[0,3] * a[1,0] * a[2,2] - a[0,0] * a[1,3] * a[2,2] - a[0,2] * a[1,0] * a[2,3] - a[0,3] * a[1,2] * a[2,0]) * detA;
            b[2,0] = (a[1,0] * a[2,1] * a[3,3] + a[1,1] * a[2,3] * a[3,0] + a[1,3] * a[2,0] * a[3,1] - a[1,0] * a[2,3] * a[3,1] - a[1,1] * a[2,0] * a[3,3] - a[1,3] * a[2,1] * a[3,0]) * detA;
            b[2,1] = (a[0,0] * a[2,3] * a[3,1] + a[0,1] * a[2,0] * a[3,3] + a[0,3] * a[2,1] * a[3,0] - a[0,0] * a[2,1] * a[3,3] - a[0,1] * a[2,3] * a[3,0] - a[0,3] * a[2,0] * a[3,1]) * detA;
            b[2,2] = (a[0,0] * a[1,1] * a[3,3] + a[0,1] * a[1,3] * a[3,0] * a[0,3] * a[1,0] * a[3,1] - a[0,0] * a[1,3] * a[3,1] - a[0,1] * a[1,0] * a[3,3] - a[0,3] * a[1,1] * a[3,0]) * detA;
            b[2,3] = (a[0,0] * a[1,3] * a[2,1] + a[0,1] * a[1,0] * a[2,3] + a[0,3] * a[1,1] * a[2,0] - a[0,0] * a[1,1] * a[2,3] - a[0,1] * a[1,3] * a[2,0] - a[0,3] * a[1,0] * a[2,1]) * detA;
            b[3,0] = (a[1,0] * a[2,2] * a[3,1] + a[1,1] * a[2,0] * a[3,2] + a[1,2] * a[2,1] * a[3,0] - a[1,0] * a[2,1] * a[3,2] - a[1,1] * a[2,2] * a[3,0] - a[1,2] * a[2,0] * a[3,1]) * detA;
            b[3,1] = (a[0,0] * a[2,1] * a[3,2] + a[0,1] * a[2,2] * a[3,0] + a[0,2] * a[2,0] * a[3,1] - a[0,0] * a[2,2] * a[3,1] - a[0,1] * a[2,0] * a[3,2] - a[0,2] * a[2,1] * a[3,0]) * detA;
            b[3,2] = (a[0,0] * a[1,2] * a[3,1] + a[0,1] * a[1,0] * a[3,2] + a[0,2] * a[1,1] * a[3,0] - a[0,0] * a[1,1] * a[3,2] - a[0,1] * a[1,2] * a[3,0] - a[0,2] * a[1,0] * a[3,1]) * detA;
            b[3,3] = (a[0,0] * a[1,1] * a[2,2] + a[0,1] * a[1,2] * a[2,0] + a[0,2] * a[1,0] * a[2,1] - a[0,0] * a[1,2] * a[2,1] - a[0,1] * a[1,0] * a[2,2] - a[0,2] * a[1,1] * a[2,0]) * detA;

        }

        public static void Multiply(Matrix4 m1, Matrix4 m2, Matrix4 erg) {
            double[,] h = erg.GetData();

            double[,] a = m1.GetData();
            double[,] b = m2.GetData();

            h[0,0] = a[0,0] * b[0,0] + a[1,0] * b[0,1] + a[2,0] * b[0,2] + a[3,0] * b[0,3];
            h[1,0] = a[0,0] * b[1,0] + a[1,0] * b[1,1] + a[2,0] * b[1,2] + a[3,0] * b[1,3];
            h[2,0] = a[0,0] * b[2,0] + a[1,0] * b[2,1] + a[2,0] * b[2,2] + a[3,0] * b[2,3];
            h[3,0] = a[0,0] * b[3,0] + a[1,0] * b[3,1] + a[2,0] * b[3,2] + a[3,0] * b[3,3];

            h[0,1] = a[0,1] * b[0,0] + a[1,1] * b[0,1] + a[2,1] * b[0,2] + a[3,1] * b[0,3];
            h[1,1] = a[0,1] * b[1,0] + a[1,1] * b[1,1] + a[2,1] * b[1,2] + a[3,1] * b[1,3];
            h[2,1] = a[0,1] * b[2,0] + a[1,1] * b[2,1] + a[2,1] * b[2,2] + a[3,1] * b[2,3];
            h[3,1] = a[0,1] * b[3,0] + a[1,1] * b[3,1] + a[2,1] * b[3,2] + a[3,1] * b[3,3];

            h[0,2] = a[0,2] * b[0,0] + a[1,2] * b[0,1] + a[2,2] * b[0,2] + a[3,2] * b[0,3];
            h[1,2] = a[0,2] * b[1,0] + a[1,2] * b[1,1] + a[2,2] * b[1,2] + a[3,2] * b[1,3];
            h[2,2] = a[0,2] * b[2,0] + a[1,2] * b[2,1] + a[2,2] * b[2,2] + a[3,2] * b[2,3];
            h[3,2] = a[0,2] * b[3,0] + a[1,2] * b[3,1] + a[2,2] * b[3,2] + a[3,2] * b[3,3];

            h[0,3] = a[0,3] * b[0,0] + a[1,3] * b[0,1] + a[2,3] * b[0,2] + a[3,3] * b[0,3];
            h[1,3] = a[0,3] * b[1,0] + a[1,3] * b[1,1] + a[2,3] * b[1,2] + a[3,3] * b[1,3];
            h[2,3] = a[0,3] * b[2,0] + a[1,3] * b[2,1] + a[2,3] * b[2,2] + a[3,3] * b[2,3];
            h[3,3] = a[0,3] * b[3,0] + a[1,3] * b[3,1] + a[2,3] * b[3,2] + a[3,3] * b[3,3];
        }
    }
}
