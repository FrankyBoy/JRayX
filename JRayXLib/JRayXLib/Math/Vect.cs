
using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public class Vect {

        public static void GetXAxis(Vect3 erg) {
            double[] d = erg.Data;

            d[0] = 1;
            d[1] = 0;
            d[2] = 0;
        }

        public static void GetYAxis(Vect3 erg) {
            double[] d = erg.Data;
            d[0] = 0;
            d[1] = 1;
            d[2] = 0;
        }

        public static void GetZAxis(Vect3 erg) {
            double[] d = erg.Data;

            d[0] = 0;
            d[1] = 0;
            d[2] = 1;
        }

        /**
     * calculates the dot product of the given vectors <vec1,vec2>
     * @author Hari
     * @param vec1
     * @param vec2
     * @return result of the dot product
     */
        public static double DotProduct(Vect3 vec1, Vect3 vec2) {
            double[] v1 = vec1.Data;
            double[] v2 = vec2.Data;
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }

        /**
     * calculates the cross product of the given vectors and store the result in
     * the given erg vector: erg = vec1 x vec2
     * @author Hari
     * @param vec1
     * @param vec2
     * @param erg
     */
        public static void CrossProduct(Vect3 vec1, Vect3 vec2, ref Vect3 erg) {
            double[] v1 = vec1.Data;
            double[] v2 = vec2.Data;
            double[] data = erg.Data;

            double x = v1[1] * v2[2] - v1[2] * v2[1];
            double y = -(v1[0] * v2[2] - v1[2] * v2[0]);
            double z = v1[0] * v2[1] - v1[1] * v2[0];

            data[0] = x;
            data[1] = y;
            data[2] = z;
        }

        public static void Subtract(Vect3 vec1, Vect3 vec2, ref Vect3 erg) {
            double[] v1 = vec1.Data;
            double[] v2 = vec2.Data;
            double[] data = erg.Data;

            data[0] = v1[0] - v2[0];
            data[1] = v1[1] - v2[1];
            data[2] = v1[2] - v2[2];
        }

        public static void Add(Vect3 vec1, Vect3 vec2, ref Vect3 erg) {
            double[] v1 = vec1.Data;
            double[] v2 = vec2.Data;
            double[] data = erg.Data;

            data[0] = v1[0] + v2[0];
            data[1] = v1[1] + v2[1];
            data[2] = v1[2] + v2[2];
        }

        public static void Scale(Vect3 vec, double d, ref Vect3 erg) {
            double[] vdat = vec.Data;
            double[] data = erg.Data;

            data[0] = vdat[0] * d;
            data[1] = vdat[1] * d;
            data[2] = vdat[2] * d;
        }

        public static double Distance(Vect3 vec1, ref Vect3 vec2) {
            double[] v1Data = vec1.Data;
            double[] v2Data = vec2.Data;

            double x = v1Data[0] - v2Data[0];
            double y = v1Data[1] - v2Data[1];
            double z = v1Data[2] - v2Data[2];

            return System.Math.Sqrt(x * x + y * y + z * z);
        }

        /**
     * Projects vect onto normedProjectionAxis. If the axis is not normed the result has to be scaled 
     * by 1/LengthOf(axis) for results to be valid after invokation of this method.
     * 
     * @param vect the vector which will be projected
     * @param normedProjectionAxis the axis onto which vect will b projected and which MUST BE NORMED 
     * @param projection the result of the projection
     */
        public static void Project(Vect3 vect, Vect3 normedProjectionAxis, ref Vect3 projection) {
            double dot = DotProduct(vect, normedProjectionAxis);
            Scale(normedProjectionAxis, dot, ref projection);
        }
    
        /**
     * Projects vect onto normedProjectionAxis. If the axis is not normed the result has to be scaled 
     * by 1/LengthOf(axis) for results to be valid after invokation of this method.
     * 
     * @param vect the vector which will be projected
     * @param normal of the surface onto which is projected (MUST BE NORMED) 
     * @param projection the result of the projection
     */
        public static void ProjectOnNormal(Vect3 vect, Vect3 normal, ref Vect3 projection) {
            double dot = DotProduct(vect, normal);
            AddMultiple(vect, normal, -dot, ref projection);
        }

        /**
     * Reflects the incoming vector on the axis (must be normed).
     * 
     * @param incoming incoming vector
     * @param normal normal of the reflectio area
     * @param outgoing result of the computation
     */
        public static void Reflect(Vect3 incoming, Vect3 normal, ref Vect3 outgoing) {
            Project(incoming, normal, ref outgoing);
            Scale(outgoing, -2, ref outgoing);
            Add(incoming, outgoing, ref outgoing);
        }
    
        /**
     * Refraction
     * 
     * @param incoming incoming vector
     * @param normal normal of the reflectio area
     * @param outgoing result of the computation
     */
        public static void Refract(Vect3 incoming, Vect3 normal, double refractionIndex, ref Vect3 outgoing) {
            //test implementation - working but propably slow
            ProjectOnNormal(incoming, normal, ref outgoing);
            Scale(outgoing, 1/refractionIndex, ref outgoing);
            double quadLen = outgoing.QuadLength();
        
            if(quadLen>=1){//total reflection
                Reflect(incoming,normal,ref outgoing);
            }else{
                AddMultiple(outgoing, normal, -System.Math.Sqrt(1-quadLen), ref outgoing);
                outgoing.Normalize();
            }
        }
    
        public static void AddMultiple(Vect3 ori, Vect3 addition, double scale, ref Vect3 erg) {
            double[] add = addition.Data;
            double[] ergdat = erg.Data;
            double[] odat = ori.Data;
        
            ergdat[0] = odat[0] + add[0] * scale;
            ergdat[1] = odat[1] + add[1] * scale;
            ergdat[2] = odat[2] + add[2] * scale;
        }
    
        public static void Invert(Vect3 vect, ref Vect3 erg) {
            double[] vectdat = vect.Data;
            double[] ergdat = erg.Data;

            ergdat[0] = - vectdat[0];
            ergdat[1] = - vectdat[1];
            ergdat[2] = - vectdat[2];
        }
    
        public static void InterpolateTriangle(Vect3 v1, Vect3 v2, Vect3 v3, Vect3 t1, Vect3 t2, Vect3 t3, Vect3 point, ref Vect3 result){
            double i1 = InterpolateTriangleEdge1(v1,v2,v3,point);
            double i2 = InterpolateTriangleEdge1(v2,v3,v1,point);
            double i3 = InterpolateTriangleEdge1(v3,v1,v2,point);
    	
            Scale(              t1, i1, ref result);
            AddMultiple(result, t2, i2, ref result);
            AddMultiple(result, t3, i3, ref result);
        }
    
        /**
         * TODO: 3 vect3 allocated here...
         */
        public static double InterpolateTriangleEdge1(Vect3 v1, Vect3 v2, Vect3 v3, Vect3 point){
            var v23N = new Vect3();
            Subtract(v3, v2, ref v23N);
            v23N.Normalize();
    	
            var v21 = new Vect3();
            Subtract(v1, v2, ref v21);
    	
            var v1O = new Vect3(); //punkt gegenüber der ecke v1 (o ... opposite)
            Project(v21, v23N, ref v1O);
            Subtract(v1O, v21, ref v1O);
    	
            var v1Hn = v1O;//höhe auf v1 (h ... height) - von v1 nach v1o - normiert
    	
            double h1 = v1Hn.Length(); //höhe auf v1
            Scale(v1Hn, 1/h1, ref v1Hn); //normieren
    	
            Vect3 v1P = v21;//von v1 nach point
            Subtract(point, v1, ref v1P);
    	
            Vect3 p1 = v23N;//projektion von v1p auf v1hn
            Project(v1P, v1Hn, ref p1);
    	
            return 1-(p1.Length()/h1);
        }
    
        public static Vect3 Avg(Vect3[] vects){
            var ret = new Vect3();
    	
            foreach (Vect3 v in vects)
                Add(ret, v, ref ret);
    	
            Scale(ret, 1/(float)vects.Length, ref ret);
    	
            return ret;
        }    
    }
}
