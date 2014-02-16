using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class PlaneCone {
	
        public static bool IsPlaneIntersectingCone(Vect3 planeNormal, Vect3 planePoint, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
            var qx = new Vect3();
            var p = new Vect3();
            var q = new Vect3();
            double len;
		
            //try "bending" axis towards plane normal 
            Vect.AddMultiple(conePosition, coneAxis, coneAxisLength, ref q);
            Vect.Project(planeNormal, coneAxis, ref qx);
            Vect.AddMultiple(qx, planeNormal, -1, ref p);
		
            if(System.Math.Abs(p.QuadLength() - 0) < Constants.EPS){// axis equals plane normal
                coneAxis.CopyDataTo(p);
                len = coneAxisLength;
            }else{//bend axis towards plane normal as far as sinPhi allows
                p.Normalize();
                Vect.AddMultiple(q, p, coneAxisLength * System.Math.Sin(System.Math.Acos(coneCosPhi)), ref q);
                Vect.Subtract(q, conePosition, ref q);
                len = q.Length();
                Vect.Scale(q, 1 / len, ref p);
            }

            double d = RayPlane.GetHitPointRayPlaneDistance(conePosition, p, planePoint, planeNormal);
		
            return d < len;
        }
    }
}
