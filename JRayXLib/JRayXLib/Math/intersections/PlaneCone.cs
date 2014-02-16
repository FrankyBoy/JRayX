using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class PlaneCone {
	
        public static bool IsPlaneIntersectingCone(Vect3 planeNormal, Vect3 planePoint, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
            var p = new Vect3(0);
            var q = new Vect3(0);
            double len;
		
            //try "bending" axis towards plane normal 
            Vect.AddMultiple(conePosition, coneAxis, coneAxisLength, ref q);
            var qx = Vect.Project(planeNormal, coneAxis);
            Vect.AddMultiple(qx, planeNormal, -1, ref p);
		
            if(System.Math.Abs(p.QuadLength() - 0) < Constants.EPS){// axis equals plane normal
                p = new Vect3(coneAxis);
                len = coneAxisLength;
            }else{//bend axis towards plane normal as far as sinPhi allows
                p.Normalize();
                Vect.AddMultiple(q, p, coneAxisLength * System.Math.Sin(System.Math.Acos(coneCosPhi)), ref q);
                q -= conePosition;
                len = q.Length();
                p = Vect.Scale(q, 1 / len);
            }

            double d = RayPlane.GetHitPointRayPlaneDistance(conePosition, p, planePoint, planeNormal);
		
            return d < len;
        }
    }
}
