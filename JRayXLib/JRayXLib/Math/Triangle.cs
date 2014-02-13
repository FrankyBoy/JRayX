using System;
using JRayXLib.Common;

namespace JRayXLib.Math
{
    public class Triangle {
        public static Vect3 GetCircumScribedCircleCenter(Vect3 a, Vect3 b, Vect3 c){
            var mab = new Vect3();
            var mac = new Vect3();
            var normal = new Vect3();
            var dab = new Vect3();
            var dac = new Vect3();
		
            Vect.subtract(b, a, mab);
            Vect.subtract(c, a, mac);
            Vect.crossProduct(mab, mac, normal);
            Vect.crossProduct(mab, normal, dab);
            Vect.crossProduct(mac, normal, dac);
            Vect.AddMultiple(a, mab, 0.5, mab);
            Vect.AddMultiple(a, mac, 0.5, mac);
            dab.normalize();
            dac.normalize();
		
            if(System.Math.Abs(dac.Data[0])>1e-9&&System.Math.Abs(dac.Data[1])>1e-9&&System.Math.Abs(dab.Data[0]/dac.Data[0] - dab.Data[1]/dac.Data[1])>1e-9){
                double x = ((mab.Data[1]-mac.Data[1])/dac.Data[1] - (mab.Data[0]-mac.Data[0])/dac.Data[0])/(dab.Data[0]/dac.Data[0] - dab.Data[1]/dac.Data[1]);

                Vect.AddMultiple(mab, dab, x);
                return mab;
            }else if(System.Math.Abs(dac.Data[0])>1e-9&&System.Math.Abs(dac.Data[2])>1e-9&&System.Math.Abs(dab.Data[2]/dac.Data[2] - dab.Data[0]/dac.Data[0])>1e-9){
                double x = ((mab.Data[0]-mac.Data[0])/dac.Data[0] - (mab.Data[2]-mac.Data[2])/dac.Data[2])/(dab.Data[2]/dac.Data[2] - dab.Data[0]/dac.Data[0]);
			
                Vect.AddMultiple(mab, dab, x);
                return mab;
            }else if(System.Math.Abs(dac.Data[1])>1e-9&&System.Math.Abs(dac.Data[2])>1e-9&&System.Math.Abs(dab.Data[1]/dac.Data[1] - dab.Data[2]/dac.Data[2])>1e-9){
                double x = ((mab.Data[2]-mac.Data[2])/dac.Data[2] - (mab.Data[1]-mac.Data[1])/dac.Data[1])/(dab.Data[1]/dac.Data[1] - dab.Data[2]/dac.Data[2]);
			
                Vect.AddMultiple(mab, dab, x);
                return mab;
            }else if(System.Math.Abs(dab.Data[1])>1e-9&&System.Math.Abs(dab.Data[0])>1e-9&&System.Math.Abs(dac.Data[0]/dab.Data[0] - dac.Data[1]/dab.Data[1])>1e-9){
                double y = ((mac.Data[1]-mab.Data[1])/dab.Data[1] - (mac.Data[0]-mab.Data[0])/dab.Data[0])/(dac.Data[0]/dab.Data[0] - dac.Data[1]/dab.Data[1]);
			
                Vect.AddMultiple(mac, dac, y);
                return mac;
            }else if(System.Math.Abs(dab.Data[2])>1e-9&&System.Math.Abs(dab.Data[0])>1e-9&&System.Math.Abs(dac.Data[2]/dab.Data[2] - dac.Data[0]/dab.Data[0])>1e-9){
                double y = ((mac.Data[0]-mab.Data[0])/dab.Data[0] - (mac.Data[2]-mab.Data[2])/dab.Data[2])/(dac.Data[2]/dab.Data[2] - dac.Data[0]/dab.Data[0]);
			
                Vect.AddMultiple(mac, dac, y);
                return mac;
            }else if(System.Math.Abs(dab.Data[2])>1e-9&&System.Math.Abs(dab.Data[1])>1e-9&&System.Math.Abs(dac.Data[1]/dab.Data[1] - dac.Data[2]/dab.Data[2])>1e-9){
                double y = ((mac.Data[2]-mab.Data[2])/dab.Data[2] - (mac.Data[1]-mab.Data[1])/dab.Data[1])/(dac.Data[1]/dab.Data[1] - dac.Data[2]/dab.Data[2]);
			
                Vect.AddMultiple(mac, dac, y);
                return mac;
            }else{
                throw new Exception("implement more cases...");
            }
        }
    }
}
