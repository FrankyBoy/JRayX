using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class PlaneCone {
	
        public static bool IsPlaneIntersectingCone(Vect3 planeNormal, Vect3 planePoint, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
            double len;
		
            //try "bending" axis towards plane normal 
            var q  = conePosition + coneAxis*coneAxisLength;
            var qx = Vect3Extensions.Project(planeNormal, coneAxis);
            var p  = qx - planeNormal;
		
            if(System.Math.Abs(p.QuadLength() - 0) < Constants.EPS){// axis equals plane normal
                p = new Vect3(coneAxis);
                len = coneAxisLength;
            }else{//bend axis towards plane normal as far as sinPhi allows
                p = p.Normalize();
                q = q - (p * coneAxisLength * System.Math.Sin(System.Math.Acos(coneCosPhi)));
                q -= conePosition;
                len = q.Length();
                p = q / len;
            }

            double d = RayPlane.GetHitPointRayPlaneDistance(conePosition, p, planePoint, planeNormal);
		
            return d < len;
        }
    }
}
