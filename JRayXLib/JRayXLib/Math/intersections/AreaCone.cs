using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class AreaCone {
	
        public static bool IsAreaIntersectingCone(Vect3 planeNormal, Vect3 planePoint, double planeWidth2, Vect3 conePosition, Vect3 coneAxis, double coneAxisLength, double coneCosPhi){
            var p = new Vect3(0);
            var q = new Vect3(0);
            double len;
		
            //try "bending" axis towards plane normal 
            Vect.AddMultiple(conePosition, coneAxis, coneAxisLength, ref  q);
            Vect3 qx = Vect.Project(planeNormal, coneAxis);
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
		
            if(d<len){
                //check if Hitpoint is in the +/-width/2 - area of the plane
                Vect.AddMultiple(conePosition, p, d, ref p);
                if (System.Math.Abs(p.Data[0] - planePoint.Data[0]) < planeWidth2 * 2 &&
                    System.Math.Abs(p.Data[1] - planePoint.Data[1]) < planeWidth2 * 2 &&
                    System.Math.Abs(p.Data[2] - planePoint.Data[2]) < planeWidth2 * 2)
                    return true;
            }
            /*
		//check if area border lines intersect cone
		for(int i=0;i<2;i++){
			for(int j=0;j<2;j++){
				p.data[0] = 0;
				p.data[0] = 0;
				p.data[0] = 0;
			}
		}*/
		
            return false;
        }
    }
}
