using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class AreaCone
    {
        public static bool IsAreaIntersectingCone(Vect3 planeNormal, Vect3 planePoint, double areaWidthHalf,
                                                  Vect3 conePosition, Vect3 coneAxis, double coneAxisLength,
                                                  double coneCosPhi)
        {
            double len;

            //try "bending" axis towards plane normal 
            Vect3 q = conePosition + coneAxis*coneAxisLength;
            Vect3 qx = Vect3Extensions.Project(planeNormal, coneAxis);
            Vect3 p = qx - planeNormal;

            if (System.Math.Abs(p.QuadLength() - 0) < Constants.EPS)
            {
// axis equals plane normal
                p = new Vect3(coneAxis);
                len = coneAxisLength;
            }
            else
            {
//bend axis towards plane normal as far as sinPhi allows
                p = p.Normalize();
                q = q + (p*coneAxisLength*System.Math.Sin(System.Math.Acos(coneCosPhi)));
                q -= conePosition;
                len = q.Length();
                p = q/len;
            }

            double d = RayPlane.GetHitPointRayPlaneDistance(conePosition, p, planePoint, planeNormal);

            if (d < len)
            {
                //check if Hitpoint is in the +/-width/2 - area of the plane
                p = conePosition + p*d;
                return
                    System.Math.Abs(p.X - planePoint.X) < areaWidthHalf*2 &&
                    System.Math.Abs(p.Y - planePoint.Y) < areaWidthHalf*2 &&
                    System.Math.Abs(p.Z - planePoint.Z) < areaWidthHalf*2;
            }

            return false;
        }
    }
}