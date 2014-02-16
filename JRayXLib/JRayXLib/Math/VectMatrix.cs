using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class VectMatrix {
        
        public static void Multiply(Matrix4 m, Vect3 v, ref Vect3 erg) {
            double[,] md = m.GetData();

            double fourth = md[3,0] * v[0] + md[3,1] * v[1] + md[3,2] * v[2] + md[3,3];

            if (System.Math.Abs(fourth - 0) > Constants.EPS) {
                double x = (md[0,0] * v[0] + md[0,1] * v[1] + md[0,2] * v[2] + md[0,3]) / fourth;
                double y = (md[1,0] * v[0] + md[1,1] * v[1] + md[1,2] * v[2] + md[1,3]) / fourth;
                double z = (md[2,0] * v[0] + md[2,1] * v[1] + md[2,2] * v[2] + md[2,3]) / fourth;

                erg[0] = x;
                erg[1] = y;
                erg[2] = z;
            }
        }

        public static void Multiply(Vect3 v, Matrix4 m, ref Vect3 erg) {
            double[,] md = m.GetData();

            double fourth = md[0,3] * v[0] + md[1,3] * v[1] + md[2,3] * v[2] + md[3,3];

            if(System.Math.Abs(fourth - 0) > Constants.EPS) {
                double x = (md[0,0] * v[0] + md[1,0] * v[1] + md[2,0] * v[2] + md[3,0]) / fourth;
                double y = (md[0,1] * v[0] + md[1,1] * v[1] + md[2,1] * v[2] + md[3,1]) / fourth;
                double z = (md[0,2] * v[0] + md[1,2] * v[1] + md[2,2] * v[2] + md[3,2]) / fourth;

                erg[0] = x;
                erg[1] = y;
                erg[2] = z;
            }
        }
    }
}
