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
            Vect3 normal = Vect3Extensions.CrossProduct(mab, mac);
            Vect3 dab = Vect3Extensions.CrossProduct(mab, normal);
            Vect3 dac = Vect3Extensions.CrossProduct(mac, normal);
            mab = a + mab/2;
            mac = a + mac/2;
            dab = dab.Normalize();
            dac = dac.Normalize();

            if (System.Math.Abs(dac[0]) > Constants.EPS &&
                System.Math.Abs(dac[1]) > Constants.EPS &&
                System.Math.Abs(dab[0] / dac[0] - dab[1] / dac[1]) > Constants.EPS)
            {
                double x = ((mab[1] - mac[1]) / dac[1] - (mab[0] - mac[0]) / dac[0]) / (dab[0] / dac[0] - dab[1] / dac[1]);
                return mab + dab*x;
            }

            if (System.Math.Abs(dac[0]) > Constants.EPS &&
                System.Math.Abs(dac[2]) > Constants.EPS &&
                System.Math.Abs(dab[2] / dac[2] - dab[0] / dac[0]) > Constants.EPS)
            {
                double x = ((mab[0] - mac[0]) / dac[0] - (mab[2] - mac[2]) / dac[2]) / (dab[2] / dac[2] - dab[0] / dac[0]);
                return mab + dab * x;
            }

            if (System.Math.Abs(dac[1]) > Constants.EPS &&
                System.Math.Abs(dac[2]) > Constants.EPS &&
                System.Math.Abs(dab[1] / dac[1] - dab[2] / dac[2]) > Constants.EPS)
            {
                double x = ((mab[2] - mac[2]) / dac[2] - (mab[1] - mac[1]) / dac[1]) / (dab[1] / dac[1] - dab[2] / dac[2]);
                return mab + dab*x;
            }

            if (System.Math.Abs(dab[1]) > Constants.EPS &&
                System.Math.Abs(dab[0]) > Constants.EPS &&
                System.Math.Abs(dac[0] / dab[0] - dac[1] / dab[1]) > Constants.EPS)
            {
                double y = ((mac[1] - mab[1]) / dab[1] - (mac[0] - mab[0]) / dab[0]) / (dac[0] / dab[0] - dac[1] / dab[1]);
                return mac + dac * y;
            }

            if (System.Math.Abs(dab[2]) > Constants.EPS &&
                System.Math.Abs(dab[0]) > Constants.EPS &&
                System.Math.Abs(dac[2] / dab[2] - dac[0] / dab[0]) > Constants.EPS)
            {
                double y = ((mac[0] - mab[0]) / dab[0] - (mac[2] - mab[2]) / dab[2]) / (dac[2] / dab[2] - dac[0] / dab[0]);
                return mac + dac * y;
            }

            if (System.Math.Abs(dab[2]) > Constants.EPS &&
                System.Math.Abs(dab[1]) > Constants.EPS &&
                System.Math.Abs(dac[1] / dab[1] - dac[2] / dab[2]) > Constants.EPS)
            {
                double y = ((mac[2] - mab[2]) / dab[2] - (mac[1] - mab[1]) / dab[1]) / (dac[1] / dab[1] - dac[2] / dab[2]);
                return mac + dac*y;
            }

            throw new Exception("implement more cases...");
        }
    }
}
