
using System.Linq;
using JRayXLib.Shapes;

namespace JRayXLib.Math
{
    public static class Vect {

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
        public static Vect3 CrossProduct(Vect3 vec1, Vect3 vec2) {
            double[] v1 = vec1.Data;
            double[] v2 = vec2.Data;

            return new Vect3(
                v1[1] * v2[2] - v1[2] * v2[1],
                -(v1[0] * v2[2] - v1[2] * v2[0]),
                v1[0] * v2[1] - v1[1] * v2[0]
            );
        }

        public static Vect3 Add(Vect3 vec1, Vect3 vec2) {
            double[] v1 = vec1.Data;
            double[] v2 = vec2.Data;

            return new Vect3(
                v1[0] + v2[0],
                v1[1] + v2[1],
                v1[2] + v2[2]
            );
        }

        public static Vect3 Scale(Vect3 vec, double d) {
            double[] vdat = vec.Data;

            return new Vect3(
                vdat[0] * d,
                vdat[1] * d,
                vdat[2] * d
            );
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
        public static Vect3 Project(Vect3 vect, Vect3 normedProjectionAxis) {
            double dot = DotProduct(vect, normedProjectionAxis);
            return Scale(normedProjectionAxis, dot);
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
        public static Vect3 Reflect(Vect3 incoming, Vect3 normal)
        {
            var result = Project(incoming, normal);
            result = Scale(result, -2);
            return Add(incoming, result);
        }
    
        /**
         * Refraction
         * 
         * @param incoming incoming vector
         * @param normal normal of the reflectio area
         * @param outgoing result of the computation
         */
        public static Vect3 Refract(Vect3 incoming, Vect3 normal, double refractionIndex) {
            var result = new Vect3();
            //test implementation - working but propably slow
            ProjectOnNormal(incoming, normal, ref result);
            result = Scale(result, 1 / refractionIndex);
            double quadLen = result.QuadLength();
        
            if(quadLen>=1){//total reflection
                result = Reflect(incoming, normal);
            }else{
                AddMultiple(result, normal, -System.Math.Sqrt(1 - quadLen), ref result);
                result.Normalize();
            }
            return result;
        }
    
        public static void AddMultiple(Vect3 ori, Vect3 addition, double scale, ref Vect3 erg) {
            double[] add = addition.Data;
            double[] ergdat = erg.Data;
            double[] odat = ori.Data;
        
            ergdat[0] = odat[0] + add[0] * scale;
            ergdat[1] = odat[1] + add[1] * scale;
            ergdat[2] = odat[2] + add[2] * scale;
        }
    
        public static Vect3 Invert(Vect3 vect) {
            return new Vect3(
                 - vect.Data[0],
                 - vect.Data[1],
                 - vect.Data[2]
                );
        }
    
        public static Vect3 InterpolateTriangle(Vect3 v1, Vect3 v2, Vect3 v3, Vect3 t1, Vect3 t2, Vect3 t3, Vect3 point){
            double i1 = InterpolateTriangleEdge(v1,v2,v3,point);
            double i2 = InterpolateTriangleEdge(v2,v3,v1,point);
            double i3 = InterpolateTriangleEdge(v3,v1,v2,point);
    	    
            var result = Scale( t1, i1);
            AddMultiple(result, t2, i2, ref result);
            AddMultiple(result, t3, i3, ref result);

            return result;
        }
    
        public static double InterpolateTriangleEdge(Vect3 v1, Vect3 v2, Vect3 v3, Vect3 point){
            var v23N = v3 - v2;
            v23N.Normalize();
    	
            var v21 = v1 - v2;
    	
// ReSharper disable InconsistentNaming
            var v1o = Project(v21, v23N); //punkt gegenüber der ecke v1 (o ... opposite)
// ReSharper restore InconsistentNaming
            
            v1o -= v21;
            
            double h1 = v1o.Length(); //höhe auf v1
            v1o = Scale(v1o, 1 / h1); //normieren
    	
            Vect3 v1P = point - v1;
            Vect3 p1 = Project(v1P, v1o);//projektion von v1p auf v1hn
    	
            return 1-(p1.Length()/h1);
        }
    
        public static Vect3 Avg(Vect3[] vects){
            var ret = new Vect3(0);

            ret = vects.Aggregate(ret, Add);
            ret = Scale(ret, 1/(float)vects.Length);
    	
            return ret;
        }    
    }
}
