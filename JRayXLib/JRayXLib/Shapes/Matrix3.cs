

namespace JRayXLib.Shapes
{
    public class Matrix3 {
        public double[] Data;
	
        public Matrix3(double[] data){
            Data = data;
        }
	
        public Matrix3(){
            Data = new double[9];
        }
	
        public static void VectorMatrixProduct(Vect3 v, Matrix3 m, ref Vect3 erg){
            erg.Data[0] = v.Data[0]*m.Data[0] + v.Data[1]*m.Data[3] + v.Data[2]*m.Data[6];
            erg.Data[1] = v.Data[0]*m.Data[1] + v.Data[1]*m.Data[4] + v.Data[2]*m.Data[7];
            erg.Data[2] = v.Data[0]*m.Data[2] + v.Data[1]*m.Data[5] + v.Data[2]*m.Data[8];
        }
    }
}
