using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RayCone
    {
        public static double GetRayConeIntersectionDistance(Vect3 ro, Vect3 rd, Vect3 v, Vect3 a, double cos,
                                                            double axisLength)
        {
            double add = a.DotProduct(rd);
            cos *= cos;
            Vect3 e = ro - v;

            double ade = a.DotProduct(e);
            double dde = rd.DotProduct(e);
            double ede = e.DotProduct(e);

            double c2 = add*add - cos;
            double c1 = add*ade - cos*dde;
            double c0 = ade*ade - cos*ede;

            cos = c1*c1 - c0*c2;

            if (cos < 0)
                return -1;
            cos = System.Math.Sqrt(cos);

            //solutions
            double t0 = (-c1 + cos)/c2;
            double t1 = (-c1 - cos)/c2;

            //project both solutions onto a - must be between 0 and 1
            e = ro + rd*t0 - v;
            cos = e.DotProduct(a);

            if (cos <= 0 || cos > axisLength)
                t0 = -1;

            e = ro + rd*t1 - v;
            cos = e.DotProduct(a);

            if (cos <= 0 || cos > axisLength)
                t1 = -1;

            if (t0 > 0)
            {
                if (t1 > 0)
                {
                    if (t0 < t1)
                        return t0;
                    return t1;
                }
                return t0;
            }
            if (t1 > 0) return t1;

            return -1;
        }
    }
}