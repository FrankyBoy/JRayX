using JRayXLib.Common;

namespace JRayXLib.Math.intersections
{
    public class RayCone {

        public static double GetRayConeIntersectionDistance(Vect3 ro, Vect3 rd, Vect3 v, Vect3 a, double cos, double axisLength) {
            double add = Vect.dotProduct(a, rd);
            cos *= cos;
            var e = new Vect3(ro.Data[0] - v.Data[0], ro.Data[1] - v.Data[1], ro.Data[2] - v.Data[2]);
            double ade = Vect.dotProduct(a, e);
            double dde = Vect.dotProduct(rd, e);
            double ede = Vect.dotProduct(e, e);
		
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
            e.Data[0] = ro.Data[0] + t0 * rd.Data[0] - v.Data[0];
            e.Data[1] = ro.Data[1] + t0 * rd.Data[1] - v.Data[1];
            e.Data[2] = ro.Data[2] + t0 * rd.Data[2] - v.Data[2];
            cos = Vect.dotProduct(e, a);
            if(cos <= 0 || cos > axisLength) t0 = -1;

            e.Data[0] = ro.Data[0] + t1 * rd.Data[0] - v.Data[0];
            e.Data[1] = ro.Data[1] + t1 * rd.Data[1] - v.Data[1];
            e.Data[2] = ro.Data[2] + t1 * rd.Data[2] - v.Data[2];
            cos = Vect.dotProduct(e, a);
            if(cos <= 0 || cos > axisLength) t1 = -1;
    	
            if(t0>0)
            {
                if(t1>0)
                {
                    if(t0<t1) return t0;
                    return t1;
                }
                return t0;
            }
            if(t1>0) return t1;
    	
            return -1;
        }
    }
}
