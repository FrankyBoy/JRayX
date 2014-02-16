using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RayCone {

        public static double GetRayConeIntersectionDistance(Vect3 ro, Vect3 rd, Vect3 v, Vect3 a, double cos, double axisLength)
        {
            var roData = ro.Data;
            var vData = v.Data;

            double add = a.DotProduct(rd);
            cos *= cos;
            var e = new Vect3(
                roData[0] - vData[0],
                roData[1] - vData[1],
                roData[2] - vData[2]
                );

            var eData = e.Data;

            double ade = a.DotProduct(e);
            double dde = rd.DotProduct(e);
            double ede = e.DotProduct(e);
		
            double c2 = add*add - cos;
            double c1 = add*ade - cos*dde;
            double c0 = ade*ade - cos*ede;
    	
            cos = c1*c1-c0*c2;
    	
            if(cos<0)
                return -1;
            cos = System.Math.Sqrt(cos);
    	
            //solutions
            double t0 = (-c1 + cos)/(c2);
            double t1 = (-c1 - cos)/(c2);
    	
            //project both solutions onto a - must be between 0 and 1
            eData[0] = roData[0] + t0 * roData[0] - vData[0];
            eData[1] = roData[1] + t0 * roData[1] - vData[1];
            eData[2] = roData[2] + t0 * roData[2] - vData[2];
            cos = e.DotProduct(a);

            if(cos <= 0 || cos > axisLength)
                t0 = -1;

            eData[0] = roData[0] + t1 * roData[0] - vData[0];
            eData[1] = roData[1] + t1 * roData[1] - vData[1];
            eData[2] = roData[2] + t1 * roData[2] - vData[2];
            cos = e.DotProduct(a);

            if(cos <= 0 || cos > axisLength)
                t1 = -1;
    	
            if(t0>0)
            {
                if(t1>0)
                {
                    if(t0<t1)
                        return t0;
                    return t1;
                }
                return t0;
            }
            if(t1>0) return t1;
    	
            return -1;
        }
    }
}
