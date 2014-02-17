using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class VectMatrix
    {
        public static Vect3 Multiply(Matrix4 m, Vect3 v)
        {
            double[,] md = m.GetData();
            double fourth = md[3, 0] * v.X + md[3, 1] * v.Y + md[3, 2] * v.Z + md[3, 3];

            if (System.Math.Abs(fourth - 0) > Constants.EPS)
            {
                return new Vect3
                {
                    X = (md[0, 0] * v.X + md[0, 1] * v.Y + md[0, 2] * v.Z + md[0, 3]) / fourth,
                    Y = (md[1, 0] * v.X + md[1, 1] * v.Y + md[1, 2] * v.Z + md[1, 3]) / fourth,
                    Z = (md[2, 0] * v.X + md[2, 1] * v.Y + md[2, 2] * v.Z + md[2, 3]) / fourth
                };
            }
            return new Vect3();
        }

        public static Vect3 Multiply(Vect3 v, Matrix4 m)
        {
            double[,] md = m.GetData();

            double fourth = md[0, 3] * v.X + md[1, 3] * v.Y + md[2, 3] * v.Z + md[3, 3];

            if (System.Math.Abs(fourth - 0) > Constants.EPS)
            {
                return new Vect3
                    {
                        X = (md[0, 0] * v.X + md[1, 0] * v.Y + md[2, 0] * v.Z + md[3, 0]) / fourth,
                        Y = (md[0, 1] * v.X + md[1, 1] * v.Y + md[2, 1] * v.Z + md[3, 1]) / fourth,
                        Z = (md[0, 2] * v.X + md[1, 2] * v.Y + md[2, 2] * v.Z + md[3, 2]) / fourth
                    };
            }
            return new Vect3();
        }
    }
}