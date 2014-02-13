package jray.struct;

import java.util.List;

import jray.common.Object3D;
import jray.common.Sphere;
import jray.common.Vect3;
import jray.math.Vect;

/**
 * A classic octree with some additional constraints:
 *  - nodes are "complete" - they contain 0 or 8 child nodes.
 *  - objects may be stored in every node (not leaf nodes only)
 *  - some objects may be stored multiple times 
 */
public class Octree{
	protected Node root;
	
	public Octree(Vect3 center, double width){
		root = new Node(center, null, width);
	}
	
	public void add(Object3D o){
		if(!root.insert(o, o.getBoundingSphere())){
			throw new RuntimeException("Could not insert: "+o);
		}
	}
	
	public Node getRoot(){
		return root;
	}
	
	public static Octree buildTree(Vect3 center, List<? extends Object3D> objects){
		double maxQuadDist=0, qdist;
		Vect3 dist=new Vect3();
		
		System.out.print("Building octree... ");
		
		for(Object3D o : objects){
			Sphere s = o.getBoundingSphere();
			if(s!=null){
				Vect.subtract(center, s.getPosition(), dist);
				qdist = s.getRadius();
				if(Double.isInfinite(qdist)||Double.isNaN(qdist))
					throw new RuntimeException("Invalid BoundingSphere: "+s+" from "+o);
				qdist = qdist*qdist + dist.quadLength();
				if(maxQuadDist < qdist)
					maxQuadDist = qdist;
			}
		}
		
		/**
		 * Workaround: *2.1 instead of *2, because rays must always start inside an octree for RayPath.
		 * To fix this problem, a RayPath should allow ray-origins outside the octree, but this would need
		 * an additional ray-cube intersection test which is currently not implemented. Another downside of
		 * this is that the camera must always be located inside the octree - it must not be moved outside.
		 */
		double sizeHint = Math.sqrt(maxQuadDist)*2.1;
		
		long start=System.nanoTime();
		int resizeCount = 0;
		Octree t;
		outer:
		while(true){
			t = new Octree(center,sizeHint);
			for(Object3D o : objects){
				if(!t.root.insert(o, o.getBoundingSphere())){
					sizeHint *= 2;
					resizeCount++;
					System.out.println("Warning - resize needed!");
					continue outer;
				}
			}
			
			break;
		}
		t.getRoot().compress();
		
		System.out.printf(" %f seconds\n",(System.nanoTime()-start)*1e-9);
		System.out.println(" - contains "+t.getRoot().getSize()+" of "+objects.size()+" elements (x"+String.format("%.2f",t.getRoot().getSize()/(float)objects.size())+")");
        System.out.println(" - avg depth: "+t.getAverageObjectDepth());
        System.out.println(" - node count: "+t.getRoot().getNodeCount());
        System.out.println(" - objects per node: "+t.getRoot().getSize()/(float)t.getRoot().getNodeCount());
		
		return t;
	}
	
	@Override
	public String toString(){
		StringBuilder sb = new StringBuilder();
		
		root.addStringRep(sb, 0);
		
		return sb.toString();
	}
	
	public double getAverageObjectDepth(){
		return root.getContentDepthSum(0)/(double)root.getSize();
	}
}