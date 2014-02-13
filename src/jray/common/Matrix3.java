package jray.common;

public class Matrix3 {
	public double[] data;
	
	public Matrix3(double[] data){
		this.data = data;
	}
	
	public Matrix3(){
		data = new double[9];
	}
	
	public static void vmm(Vect3 v, Matrix3 m, Vect3 erg){
		erg.data[0] = v.data[0]*m.data[0] + v.data[1]*m.data[3] + v.data[2]*m.data[6];
		erg.data[1] = v.data[0]*m.data[1] + v.data[1]*m.data[4] + v.data[2]*m.data[7];
		erg.data[2] = v.data[0]*m.data[2] + v.data[1]*m.data[5] + v.data[2]*m.data[8];
	}
}
