using System;
using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class Triangle
    {
        public static Vect3 GetCircumScribedCircleCenter(Vect3 a, Vect3 b, Vect3 c)
        {
            var mab = new Vect3(0);
            var mac = new Vect3(0);

            Vect.Subtract(b, a, ref mab);
            Vect.Subtract(c, a, ref mac);
            Vect3 normal = Vect.CrossProduct(mab, mac);
            Vect3 dab = Vect.CrossProduct(mab, normal);
            Vect3 dac = Vect.CrossProduct(mac, normal);
            Vect.AddMultiple(a, mab, 0.5, ref mab);
            Vect.AddMultiple(a, mac, 0.5, ref mac);
            dab.Normalize();
            dac.Normalize();

            if (System.Math.Abs(dac.Data[0]) > 1e-9 && System.Math.Abs(dac.Data[1]) > 1e-9 && System.Math.Abs(dab.Data[0] / dac.Data[0] - dab.Data[1] / dac.Data[1]) > 1e-9)
            {
                double x = ((mab.Data[1] - mac.Data[1]) / dac.Data[1] - (mab.Data[0] - mac.Data[0]) / dac.Data[0]) / (dab.Data[0] / dac.Data[0] - dab.Data[1] / dac.Data[1]);
                Vect.AddMultiple(mab, dab, x, ref mab);
                return mab;
            }
            if (System.Math.Abs(dac.Data[0]) > 1e-9 && System.Math.Abs(dac.Data[2]) > 1e-9 && System.Math.Abs(dab.Data[2] / dac.Data[2] - dab.Data[0] / dac.Data[0]) > 1e-9)
            {
                double x = ((mab.Data[0] - mac.Data[0]) / dac.Data[0] - (mab.Data[2] - mac.Data[2]) / dac.Data[2]) / (dab.Data[2] / dac.Data[2] - dab.Data[0] / dac.Data[0]);
                Vect.AddMultiple(mab, dab, x, ref mab);
                return mab;
            }
            if (System.Math.Abs(dac.Data[1]) > 1e-9 && System.Math.Abs(dac.Data[2]) > 1e-9 && System.Math.Abs(dab.Data[1] / dac.Data[1] - dab.Data[2] / dac.Data[2]) > 1e-9)
            {
                double x = ((mab.Data[2] - mac.Data[2]) / dac.Data[2] - (mab.Data[1] - mac.Data[1]) / dac.Data[1]) / (dab.Data[1] / dac.Data[1] - dab.Data[2] / dac.Data[2]);
                Vect.AddMultiple(mab, dab, x, ref mab);
                return mab;
            }
            if (System.Math.Abs(dab.Data[1]) > 1e-9 && System.Math.Abs(dab.Data[0]) > 1e-9 && System.Math.Abs(dac.Data[0] / dab.Data[0] - dac.Data[1] / dab.Data[1]) > 1e-9)
            {
                double y = ((mac.Data[1] - mab.Data[1]) / dab.Data[1] - (mac.Data[0] - mab.Data[0]) / dab.Data[0]) / (dac.Data[0] / dab.Data[0] - dac.Data[1] / dab.Data[1]);
                Vect.AddMultiple(mac, dac, y, ref mab);
                return mac;
            }
            if (System.Math.Abs(dab.Data[2]) > 1e-9 && System.Math.Abs(dab.Data[0]) > 1e-9 && System.Math.Abs(dac.Data[2] / dab.Data[2] - dac.Data[0] / dab.Data[0]) > 1e-9)
            {
                double y = ((mac.Data[0] - mab.Data[0]) / dab.Data[0] - (mac.Data[2] - mab.Data[2]) / dab.Data[2]) / (dac.Data[2] / dab.Data[2] - dac.Data[0] / dab.Data[0]);
                Vect.AddMultiple(mac, dac, y, ref mab);
                return mac;
            }
            if (System.Math.Abs(dab.Data[2]) > 1e-9 && System.Math.Abs(dab.Data[1]) > 1e-9 && System.Math.Abs(dac.Data[1] / dab.Data[1] - dac.Data[2] / dab.Data[2]) > 1e-9)
            {
                double y = ((mac.Data[2] - mab.Data[2]) / dab.Data[2] - (mac.Data[1] - mab.Data[1]) / dab.Data[1]) / (dac.Data[1] / dab.Data[1] - dac.Data[2] / dab.Data[2]);
                Vect.AddMultiple(mac, dac, y, ref mab);
                return mac;
            }
            throw new Exception("implement more cases...");
        }
    }
}
