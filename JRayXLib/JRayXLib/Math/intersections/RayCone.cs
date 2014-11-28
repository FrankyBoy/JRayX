using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class RayCone
    {
        public static double GetRayConeIntersectionDistance(
            Vect3 rayOrigin, Vect3 rayDirection,
            Vect3 position, Vect3 lookAt, double cos, double axisLength)
        {
            double add = lookAt * rayDirection;
            cos *= cos;
            Vect3 e = rayOrigin - position;

            double ade = lookAt * e;
            double dde = rayDirection * e;
            double ede = e * e;

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

            //project both solutions onto lookAt - must be between 0 and axisLength
            e = rayOrigin + rayDirection*t0 - position;
            cos = e * lookAt;

            if (cos <= 0 || cos > axisLength)
                t0 = -1;

            e = rayOrigin + rayDirection*t1 - position;
            cos = e * lookAt;

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

        /*
        // M = (A * A(trans) - cosPhi^2 * I )
        // A = cone direction
        // I = unity matrix
        private static Matrix4 CreateMatrix(Vect3 a, double cosPhi)
        {
            double xy = a.X*a.Y;
            double xz = a.X*a.Z;
            double yz = a.Y*a.Z;

            cosPhi *= cosPhi;

            return new Matrix4
                {
                    A0 = a.X * a.X - cosPhi,
                    A1 = xy,
                    A2 = xz,
                    B0 = xy,
                    B1 = a.Y * a.Y - cosPhi,
                    B2 = yz,
                    C0 = xz,
                    C1 = yz,
                    C2 = a.Z * a.Z - cosPhi,
                    D3 = 1
                };
        }*/
    }
}