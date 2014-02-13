package jray.math;

import jray.common.Vect2;
import jray.common.Vect3;

public class Vect {

    public static void getXAxis(Vect3 erg) {
        double[] d = erg.getData();

        d[0] = 1;
        d[1] = 0;
        d[2] = 0;
    }

    public static void getYAxis(Vect3 erg) {
        double[] d = erg.getData();
        d[0] = 0;
        d[1] = 1;
        d[2] = 0;
    }

    public static void getZAxis(Vect3 erg) {
        double[] d = erg.getData();

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
    public static double dotProduct(Vect3 vec1, Vect3 vec2) {
        double[] v1 = vec1.getData();
        double[] v2 = vec2.getData();
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
    public static void crossProduct(Vect3 vec1, Vect3 vec2, Vect3 erg) {
        double[] v1 = vec1.getData();
        double[] v2 = vec2.getData();
        double[] data = erg.getData();
        double x, y, z;

        x = v1[1] * v2[2] - v1[2] * v2[1];
        y = -(v1[0] * v2[2] - v1[2] * v2[0]);
        z = v1[0] * v2[1] - v1[1] * v2[0];

        data[0] = x;
        data[1] = y;
        data[2] = z;
    }

    public static void subtract(Vect3 vec1, Vect3 vec2, Vect3 erg) {
        double[] v1 = vec1.getData();
        double[] v2 = vec2.getData();
        double[] data = erg.getData();

        data[0] = v1[0] - v2[0];
        data[1] = v1[1] - v2[1];
        data[2] = v1[2] - v2[2];
    }

    public static void add(Vect3 vec1, Vect3 vec2, Vect3 erg) {
        double[] v1 = vec1.getData();
        double[] v2 = vec2.getData();
        double[] data = erg.getData();

        data[0] = v1[0] + v2[0];
        data[1] = v1[1] + v2[1];
        data[2] = v1[2] + v2[2];
    }

    public static void scale(Vect2 vec, double d, Vect2 erg) {
        double[] vdat = vec.getData();
        double[] data = erg.getData();

        data[0] = vdat[0] * d;
        data[1] = vdat[1] * d;
    }
    
    public static void scale(Vect3 vec, double d, Vect3 erg) {
        double[] vdat = vec.getData();
        double[] data = erg.getData();

        data[0] = vdat[0] * d;
        data[1] = vdat[1] * d;
        data[2] = vdat[2] * d;
    }

    public static double distance(Vect3 vec1, Vect3 vec2) {
        double[] v1data = vec1.getData();
        double[] v2data = vec2.getData();

        double x = v1data[0] - v2data[0];
        double y = v1data[1] - v2data[1];
        double z = v1data[2] - v2data[2];

        return Math.sqrt(x * x + y * y + z * z);
    }

    /**
     * Projects vect onto normedProjectionAxis. If the axis is not normed the result has to be scaled 
     * by 1/lengthOf(axis) for results to be valid after invokation of this method.
     * 
     * @param vect the vector which will be projected
     * @param normedProjectionAxis the axis onto which vect will b projected and which MUST BE NORMED 
     * @param projection the result of the projection
     */
    public static void project(Vect3 vect, Vect3 normedProjectionAxis, Vect3 projection) {
        double dot = Vect.dotProduct(vect, normedProjectionAxis);
        Vect.scale(normedProjectionAxis, dot, projection);
    }
    
    /**
     * Projects vect onto normedProjectionAxis. If the axis is not normed the result has to be scaled 
     * by 1/lengthOf(axis) for results to be valid after invokation of this method.
     * 
     * @param vect the vector which will be projected
     * @param normal of the surface onto which is projected (MUST BE NORMED) 
     * @param projection the result of the projection
     */
    public static void projectOnNormal(Vect3 vect, Vect3 normal, Vect3 projection) {
        double dot = Vect.dotProduct(vect, normal);
        Vect.addMultiple(vect, normal, -dot, projection);
    }

    /**
     * Reflects the incoming vector on the axis (must be normed).
     * 
     * @param incoming incoming vector
     * @param normal normal of the reflectio area
     * @param outgoing result of the computation
     */
    public static void reflect(Vect3 incoming, Vect3 normal, Vect3 outgoing) {
        project(incoming, normal, outgoing);
        scale(outgoing, -2, outgoing);
        add(incoming, outgoing, outgoing);
    }
    
    /**
     * Refraction
     * 
     * @param incoming incoming vector
     * @param normal normal of the reflectio area
     * @param outgoing result of the computation
     */
    public static void refract(Vect3 incoming, Vect3 normal, double refractionIndex, Vect3 outgoing) {
    	//test implementation - working but propably slow
    	projectOnNormal(incoming, normal, outgoing);
        scale(outgoing, 1/refractionIndex, outgoing);
        double quadLen = outgoing.quadLength();
        
        if(quadLen>=1){//total reflection
        	reflect(incoming,normal,outgoing);
        }else{
        	addMultiple(outgoing, normal, -Math.sqrt(1-quadLen), outgoing);
        	outgoing.normalize();
        }
    }

    public static void addMultiple(Vect2 ori, Vect2 addition, double scale, Vect2 erg) {
        double[] add = addition.getData();
        double[] ergdat = erg.getData();
        double[] odat = ori.getData();
        
        ergdat[0] = odat[0] + add[0] * scale;
        ergdat[1] = odat[1] + add[1] * scale;
    }
    
    public static void addMultiple(Vect3 ori, Vect3 addition, double scale, Vect3 erg) {
        double[] add = addition.getData();
        double[] ergdat = erg.getData();
        double[] odat = ori.getData();
        
        ergdat[0] = odat[0] + add[0] * scale;
        ergdat[1] = odat[1] + add[1] * scale;
        ergdat[2] = odat[2] + add[2] * scale;
    }
    
    public static void addMultiple(Vect3 addTo, Vect3 addition, double scale) {
        double[] add = addition.getData();
        double[] sum = addTo.getData();
        
        sum[0] += add[0] * scale;
        sum[1] += add[1] * scale;
        sum[2] += add[2] * scale;
    }
    
    public static void invert(Vect3 vect, Vect3 erg) {
        double[] vectdat = vect.getData();
        double[] ergdat = erg.getData();

        ergdat[0] = - vectdat[0];
        ergdat[1] = - vectdat[1];
        ergdat[2] = - vectdat[2];
    }
    
    public static void interpolateTriangle(Vect3 v1, Vect3 v2, Vect3 v3, Vect3 t1, Vect3 t2, Vect3 t3, Vect3 point, Vect3 result){
    	double i1 = interpolateTriangleEdge1(v1,v2,v3,point);
    	double i2 = interpolateTriangleEdge1(v2,v3,v1,point);
    	double i3 = interpolateTriangleEdge1(v3,v1,v2,point);
    	
    	Vect.scale(              t1, i1, result);
    	Vect.addMultiple(result, t2, i2, result);
    	Vect.addMultiple(result, t3, i3, result);
    }
    
    public static void interpolateTriangle(Vect3 v1, Vect3 v2, Vect3 v3, Vect2 t1, Vect2 t2, Vect2 t3, Vect3 point, Vect2 result){
    	double i1 = interpolateTriangleEdge1(v1,v2,v3,point);
    	double i2 = interpolateTriangleEdge1(v2,v3,v1,point);
    	double i3 = interpolateTriangleEdge1(v3,v1,v2,point);
    	
    	Vect.scale(              t1, i1, result);
    	Vect.addMultiple(result, t2, i2, result);
    	Vect.addMultiple(result, t3, i3, result);
    }
    
    /**
     * TODO: 3 vect3 allocated here...
     */
    public static double interpolateTriangleEdge1(Vect3 v1, Vect3 v2, Vect3 v3, Vect3 point){
    	Vect3 v23n = new Vect3();
    	Vect.subtract(v3, v2, v23n);
    	v23n.normalize();
    	
    	Vect3 v21 = new Vect3();
    	Vect.subtract(v1, v2, v21);
    	
    	Vect3 v1o = new Vect3(); //punkt gegenüber der ecke v1 (o ... opposite)
    	Vect.project(v21, v23n, v1o);
    	Vect.subtract(v1o, v21, v1o);
    	
    	Vect3 v1hn = v1o;//höhe auf v1 (h ... height) - von v1 nach v1o - normiert
    	
    	double h1 = v1hn.length(); //höhe auf v1
    	Vect.scale(v1hn, 1/h1, v1hn); //normieren
    	
    	Vect3 v1p = v21;//von v1 nach point
    	Vect.subtract(point, v1, v1p);
    	
    	Vect3 p1 = v23n;//projektion von v1p auf v1hn
    	Vect.project(v1p, v1hn, p1);
    	
    	return 1-(p1.length()/h1);
    }
    
    public static Vect3 avg(Vect3 ... vects){
    	Vect3 ret = new Vect3();
    	
    	for(Vect3 v : vects)
    		Vect.add(ret, v, ret);
    	
    	Vect.scale(ret, 1/(float)vects.length, ret);
    	
    	return ret;
    }    
}
