package jray.model;

import java.util.Arrays;

import jray.common.Sphere;
import jray.common.Vect3;
import jray.math.Vect;
import jray.struct.Octree;

public class TriangleMeshModel {
	private MinimalTriangle[] triangles;
	private Sphere bounds;
	private Octree tree;
	
	/**
	 * 3xnormal
	 * 3x v1
	 * 3x v2
	 * 3x v3
	 * 
	 * @param triangleEdgeData
	 */
	public TriangleMeshModel(float[] triangleEdgeData){
		int triangleCount = triangleEdgeData.length/12;
		
		triangles = new MinimalTriangle[triangleCount];
		
		for(int i=0;i<triangleCount;i++)
			triangles[i] = new MinimalTriangle(new Vect3(triangleEdgeData[i*12+0],triangleEdgeData[i*12+1],triangleEdgeData[i*12+2]),
					                           new Vect3(triangleEdgeData[i*12+3],triangleEdgeData[i*12+4],triangleEdgeData[i*12+5]),
					                           new Vect3(triangleEdgeData[i*12+6],triangleEdgeData[i*12+7],triangleEdgeData[i*12+8]),
					                           new Vect3(triangleEdgeData[i*12+9],triangleEdgeData[i*12+10],triangleEdgeData[i*12+11]));
		
		Vect3 max = new Vect3(Double.NEGATIVE_INFINITY,Double.NEGATIVE_INFINITY,Double.NEGATIVE_INFINITY);
		Vect3 min = new Vect3(Double.POSITIVE_INFINITY,Double.POSITIVE_INFINITY,Double.POSITIVE_INFINITY);
		
		for(MinimalTriangle m : triangles){
			Vect3 p = m.getBoundingSphere().getPosition();
			
			if(p.data[0]>max.data[0]) max.data[0] = p.data[0];
			if(p.data[1]>max.data[1]) max.data[1] = p.data[1];
			if(p.data[2]>max.data[2]) max.data[2] = p.data[2];
			if(p.data[0]<min.data[0]) min.data[0] = p.data[0];
			if(p.data[1]<min.data[1]) min.data[1] = p.data[1];
			if(p.data[2]<min.data[2]) min.data[2] = p.data[2];
		}
		
		max.data[0] = (max.data[0]+min.data[0])/2;
		max.data[1] = (max.data[1]+min.data[1])/2;
		max.data[2] = (max.data[2]+min.data[2])/2;
		
		double radius = 0;
		for(MinimalTriangle m : triangles){
			Vect3 p = m.getBoundingSphere().getPosition();
			Vect.subtract(p, max, min);
			double dist = m.getBoundingSphereRadius() + min.length();
			
			if(dist > radius){
				radius = dist;
			}
		}
		
		bounds = new Sphere(max, radius, 0xFF000000);
		
		tree = Octree.buildTree(bounds.getPosition(), Arrays.asList(triangles));
		
		for(MinimalTriangle m : triangles)
			m.purge();
	}
	
	public Sphere getBoundingSphere(){
    	return bounds;
    }

	public Octree getTree() {
		return tree;
	}
}
