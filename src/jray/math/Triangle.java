package jray.math;

import jray.common.Vect3;

public class Triangle {
	public static Vect3 getCircumScribedCircleCenter(Vect3 a, Vect3 b, Vect3 c){
		Vect3 mab = new Vect3();
		Vect3 mac = new Vect3();
		Vect3 normal = new Vect3();
		Vect3 dab = new Vect3();
		Vect3 dac = new Vect3();
		
		Vect.subtract(b, a, mab);
		Vect.subtract(c, a, mac);
		Vect.crossProduct(mab, mac, normal);
		Vect.crossProduct(mab, normal, dab);
		Vect.crossProduct(mac, normal, dac);
		Vect.addMultiple(a, mab, 0.5, mab);
		Vect.addMultiple(a, mac, 0.5, mac);
		dab.normalize();
		dac.normalize();
		
		if(Math.abs(dac.data[0])>1e-9&&Math.abs(dac.data[1])>1e-9&&Math.abs(dab.data[0]/dac.data[0] - dab.data[1]/dac.data[1])>1e-9){
			double x = ((mab.data[1]-mac.data[1])/dac.data[1] - (mab.data[0]-mac.data[0])/dac.data[0])/(dab.data[0]/dac.data[0] - dab.data[1]/dac.data[1]);

			Vect.addMultiple(mab, dab, x);
			return mab;
		}else if(Math.abs(dac.data[0])>1e-9&&Math.abs(dac.data[2])>1e-9&&Math.abs(dab.data[2]/dac.data[2] - dab.data[0]/dac.data[0])>1e-9){
			double x = ((mab.data[0]-mac.data[0])/dac.data[0] - (mab.data[2]-mac.data[2])/dac.data[2])/(dab.data[2]/dac.data[2] - dab.data[0]/dac.data[0]);
			
			Vect.addMultiple(mab, dab, x);
			return mab;
		}else if(Math.abs(dac.data[1])>1e-9&&Math.abs(dac.data[2])>1e-9&&Math.abs(dab.data[1]/dac.data[1] - dab.data[2]/dac.data[2])>1e-9){
			double x = ((mab.data[2]-mac.data[2])/dac.data[2] - (mab.data[1]-mac.data[1])/dac.data[1])/(dab.data[1]/dac.data[1] - dab.data[2]/dac.data[2]);
			
			Vect.addMultiple(mab, dab, x);
			return mab;
		}else if(Math.abs(dab.data[1])>1e-9&&Math.abs(dab.data[0])>1e-9&&Math.abs(dac.data[0]/dab.data[0] - dac.data[1]/dab.data[1])>1e-9){
			double y = ((mac.data[1]-mab.data[1])/dab.data[1] - (mac.data[0]-mab.data[0])/dab.data[0])/(dac.data[0]/dab.data[0] - dac.data[1]/dab.data[1]);
			
			Vect.addMultiple(mac, dac, y);
			return mac;
		}else if(Math.abs(dab.data[2])>1e-9&&Math.abs(dab.data[0])>1e-9&&Math.abs(dac.data[2]/dab.data[2] - dac.data[0]/dab.data[0])>1e-9){
			double y = ((mac.data[0]-mab.data[0])/dab.data[0] - (mac.data[2]-mab.data[2])/dab.data[2])/(dac.data[2]/dab.data[2] - dac.data[0]/dab.data[0]);
			
			Vect.addMultiple(mac, dac, y);
			return mac;
		}else if(Math.abs(dab.data[2])>1e-9&&Math.abs(dab.data[1])>1e-9&&Math.abs(dac.data[1]/dab.data[1] - dac.data[2]/dab.data[2])>1e-9){
			double y = ((mac.data[2]-mab.data[2])/dab.data[2] - (mac.data[1]-mab.data[1])/dab.data[1])/(dac.data[1]/dab.data[1] - dac.data[2]/dab.data[2]);
			
			Vect.addMultiple(mac, dac, y);
			return mac;
		}else{
			throw new RuntimeException("implement more cases...");
		}
	}
}
