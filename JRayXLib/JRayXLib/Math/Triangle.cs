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

            double[] dacData = dac.Data;
            double[] dabData = dab.Data;
            double[] mabData = mab.Data;
            double[] macData = mac.Data;


            if (System.Math.Abs(dacData[0]) > Constants.EPS &&
                System.Math.Abs(dacData[1]) > Constants.EPS &&
                System.Math.Abs(dabData[0] / dacData[0] - dabData[1] / dacData[1]) > Constants.EPS)
            {
                double x = ((mabData[1] - macData[1]) / dacData[1] - (mabData[0] - macData[0]) / dacData[0]) / (dabData[0] / dacData[0] - dabData[1] / dacData[1]);
                return mab + dab*x;
            }

            if (System.Math.Abs(dacData[0]) > Constants.EPS &&
                System.Math.Abs(dacData[2]) > Constants.EPS &&
                System.Math.Abs(dabData[2] / dacData[2] - dabData[0] / dacData[0]) > Constants.EPS)
            {
                double x = ((mabData[0] - macData[0]) / dacData[0] - (mabData[2] - macData[2]) / dacData[2]) / (dabData[2] / dacData[2] - dabData[0] / dacData[0]);
                return mab + dab * x;
            }

            if (System.Math.Abs(dacData[1]) > Constants.EPS &&
                System.Math.Abs(dacData[2]) > Constants.EPS &&
                System.Math.Abs(dabData[1] / dacData[1] - dabData[2] / dacData[2]) > Constants.EPS)
            {
                double x = ((mabData[2] - macData[2]) / dacData[2] - (mabData[1] - macData[1]) / dacData[1]) / (dabData[1] / dacData[1] - dabData[2] / dacData[2]);
                return mab + dab*x;
            }

            if (System.Math.Abs(dabData[1]) > Constants.EPS &&
                System.Math.Abs(dabData[0]) > Constants.EPS &&
                System.Math.Abs(dacData[0] / dabData[0] - dacData[1] / dabData[1]) > Constants.EPS)
            {
                double y = ((macData[1] - mabData[1]) / dabData[1] - (macData[0] - mabData[0]) / dabData[0]) / (dacData[0] / dabData[0] - dacData[1] / dabData[1]);
                return mac + dac * y;
            }

            if (System.Math.Abs(dabData[2]) > Constants.EPS &&
                System.Math.Abs(dabData[0]) > Constants.EPS &&
                System.Math.Abs(dacData[2] / dabData[2] - dacData[0] / dabData[0]) > Constants.EPS)
            {
                double y = ((macData[0] - mabData[0]) / dabData[0] - (macData[2] - mabData[2]) / dabData[2]) / (dacData[2] / dabData[2] - dacData[0] / dabData[0]);
                return mac + dac * y;
            }

            if (System.Math.Abs(dabData[2]) > Constants.EPS &&
                System.Math.Abs(dabData[1]) > Constants.EPS &&
                System.Math.Abs(dacData[1] / dabData[1] - dacData[2] / dabData[2]) > Constants.EPS)
            {
                double y = ((macData[2] - mabData[2]) / dabData[2] - (macData[1] - mabData[1]) / dabData[1]) / (dacData[1] / dabData[1] - dacData[2] / dabData[2]);
                return mac + dac*y;
            }

            throw new Exception("implement more cases...");
        }
    }
}
