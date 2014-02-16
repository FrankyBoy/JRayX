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
            Vect3 normal = Vect.CrossProduct(mab, mac);
            Vect3 dab = Vect.CrossProduct(mab, normal);
            Vect3 dac = Vect.CrossProduct(mac, normal);
            mab = a + mab/2;
            mac = a + mac/2;
            dab.Normalize();
            dac.Normalize();

            if (System.Math.Abs(dac.Data[0]) > Constants.EPS &&
                System.Math.Abs(dac.Data[1]) > Constants.EPS &&
                System.Math.Abs(dab.Data[0] / dac.Data[0] - dab.Data[1] / dac.Data[1]) > Constants.EPS)
            {
                double x = ((mab.Data[1] - mac.Data[1]) / dac.Data[1] - (mab.Data[0] - mac.Data[0]) / dac.Data[0]) / (dab.Data[0] / dac.Data[0] - dab.Data[1] / dac.Data[1]);
                return mab + dab*x;
            }

            if (System.Math.Abs(dac.Data[0]) > Constants.EPS &&
                System.Math.Abs(dac.Data[2]) > Constants.EPS &&
                System.Math.Abs(dab.Data[2] / dac.Data[2] - dab.Data[0] / dac.Data[0]) > Constants.EPS)
            {
                double x = ((mab.Data[0] - mac.Data[0]) / dac.Data[0] - (mab.Data[2] - mac.Data[2]) / dac.Data[2]) / (dab.Data[2] / dac.Data[2] - dab.Data[0] / dac.Data[0]);
                return mab + dab * x;
            }

            if (System.Math.Abs(dac.Data[1]) > Constants.EPS &&
                System.Math.Abs(dac.Data[2]) > Constants.EPS &&
                System.Math.Abs(dab.Data[1] / dac.Data[1] - dab.Data[2] / dac.Data[2]) > Constants.EPS)
            {
                double x = ((mab.Data[2] - mac.Data[2]) / dac.Data[2] - (mab.Data[1] - mac.Data[1]) / dac.Data[1]) / (dab.Data[1] / dac.Data[1] - dab.Data[2] / dac.Data[2]);
                return mab + dab*x;
            }

            if (System.Math.Abs(dab.Data[1]) > Constants.EPS &&
                System.Math.Abs(dab.Data[0]) > Constants.EPS &&
                System.Math.Abs(dac.Data[0] / dab.Data[0] - dac.Data[1] / dab.Data[1]) > Constants.EPS)
            {
                double y = ((mac.Data[1] - mab.Data[1]) / dab.Data[1] - (mac.Data[0] - mab.Data[0]) / dab.Data[0]) / (dac.Data[0] / dab.Data[0] - dac.Data[1] / dab.Data[1]);
                return mac + dac * y;
            }

            if (System.Math.Abs(dab.Data[2]) > Constants.EPS &&
                System.Math.Abs(dab.Data[0]) > Constants.EPS &&
                System.Math.Abs(dac.Data[2] / dab.Data[2] - dac.Data[0] / dab.Data[0]) > Constants.EPS)
            {
                double y = ((mac.Data[0] - mab.Data[0]) / dab.Data[0] - (mac.Data[2] - mab.Data[2]) / dab.Data[2]) / (dac.Data[2] / dab.Data[2] - dac.Data[0] / dab.Data[0]);
                return mac + dac * y;
            }

            if (System.Math.Abs(dab.Data[2]) > Constants.EPS &&
                System.Math.Abs(dab.Data[1]) > Constants.EPS &&
                System.Math.Abs(dac.Data[1] / dab.Data[1] - dac.Data[2] / dab.Data[2]) > Constants.EPS)
            {
                double y = ((mac.Data[2] - mab.Data[2]) / dab.Data[2] - (mac.Data[1] - mab.Data[1]) / dab.Data[1]) / (dac.Data[1] / dab.Data[1] - dac.Data[2] / dab.Data[2]);
                return mac + dac*y;
            }

            throw new Exception("implement more cases...");
        }
    }
}
