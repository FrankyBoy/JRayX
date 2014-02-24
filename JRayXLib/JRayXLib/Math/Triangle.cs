using System;
using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class Triangle
    {
        public static Vect3 GetCircumScribedCircleCenter(Vect3 a, Vect3 b, Vect3 c)
        {
            Vect3 mab = b - a;
            Vect3 mac = c - a;
            Vect3 normal = mab.CrossProduct(mac);
            Vect3 dab = mab.CrossProduct(normal);
            Vect3 dac = mac.CrossProduct(normal);
            mab = a + mab/2;
            mac = a + mac/2;
            dab = dab.Normalize();
            dac = dac.Normalize();


            if (System.Math.Abs(dac.X) > Constants.EPS &&
                System.Math.Abs(dac.Y) > Constants.EPS &&
                System.Math.Abs(dab.X/dac.X - dab.Y/dac.Y) > Constants.EPS)
            {
                double x = ((mab.Y - mac.Y)/dac.Y - (mab.X - mac.X)/dac.X)/(dab.X/dac.X - dab.Y/dac.Y);
                return mab + dab*x;
            }

            if (System.Math.Abs(dac.X) > Constants.EPS &&
                System.Math.Abs(dac.Z) > Constants.EPS &&
                System.Math.Abs(dab.Z/dac.Z - dab.X/dac.X) > Constants.EPS)
            {
                double x = ((mab.X - mac.X)/dac.X - (mab.Z - mac.Z)/dac.Z)/(dab.Z/dac.Z - dab.X/dac.X);
                return mab + dab*x;
            }

            if (System.Math.Abs(dac.Y) > Constants.EPS &&
                System.Math.Abs(dac.Z) > Constants.EPS &&
                System.Math.Abs(dab.Y/dac.Y - dab.Z/dac.Z) > Constants.EPS)
            {
                double x = ((mab.Z - mac.Z)/dac.Z - (mab.Y - mac.Y)/dac.Y)/(dab.Y/dac.Y - dab.Z/dac.Z);
                return mab + dab*x;
            }

            if (System.Math.Abs(dab.Y) > Constants.EPS &&
                System.Math.Abs(dab.X) > Constants.EPS &&
                System.Math.Abs(dac.X/dab.X - dac.Y/dab.Y) > Constants.EPS)
            {
                double y = ((mac.Y - mab.Y)/dab.Y - (mac.X - mab.X)/dab.X)/(dac.X/dab.X - dac.Y/dab.Y);
                return mac + dac*y;
            }

            if (System.Math.Abs(dab.Z) > Constants.EPS &&
                System.Math.Abs(dab.X) > Constants.EPS &&
                System.Math.Abs(dac.Z/dab.Z - dac.X/dab.X) > Constants.EPS)
            {
                double y = ((mac.X - mab.X)/dab.X - (mac.Z - mab.Z)/dab.Z)/(dac.Z/dab.Z - dac.X/dab.X);
                return mac + dac*y;
            }

            if (System.Math.Abs(dab.Z) > Constants.EPS &&
                System.Math.Abs(dab.Y) > Constants.EPS &&
                System.Math.Abs(dac.Y/dab.Y - dac.Z/dab.Z) > Constants.EPS)
            {
                double y = ((mac.Z - mab.Z)/dab.Z - (mac.Y - mab.Y)/dab.Y)/(dac.Y/dab.Y - dac.Z/dab.Z);
                return mac + dac*y;
            }

            throw new Exception("implement more cases...");
        }
    }
}