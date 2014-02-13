package jray.struct;

import java.util.ArrayList;
import java.util.List;

import jray.common.Object3D;
import jray.common.Sphere;
import jray.common.Vect3;
import jray.math.intersections.CubeSphere;
import jray.math.intersections.PointCube;

/**
 * A node of an octree located at <code>center +/- width/2</code> (a cube).<p/>
 * 
 * If a point is contained in a leaf-node, it is guaranteed that any object intersecting this point can be found in the leaf node or it's parents.
 * It is NOT guaranteed that each object is stored only once in an octree (although a TreeInsertStrategy may try to do this to save memory).
 */
public class Node{
	public static TreeInsertStrategy strategy = TreeInsertStrategy.DYNAMIC_TEST;
	public static final double MIN_NODE_WIDTH = 1e-9;
	
	private final Node parent; //parent node
	private Node[] child; //child nodes
	List<Object3D> content = new ArrayList<Object3D>(); //objects contained in this node
	final Vect3 center; //central point of the nodes cube
	final double width; //node reaches from center-width/2 to center+width/2
	
	public Node(Vect3 center, Node parent, double width){
		this.parent = parent;
		this.width = width;
		this.center = center;
		
		if(width<MIN_NODE_WIDTH)
			throw new RuntimeException("Node width too small: "+width);
	}
	
	/**
	 * @param v a <code>Vect3</code> representing a point
	 * @return true if, and only if, <code>v</code> is enclosed by this subtree.
	 */
	public boolean encloses(Vect3 v){
		return PointCube.encloses(center, width/2 + TreeInsertStrategy.EPS, v);
	}
	
	/**
	 * Inserts an object into the subtree. The actual algorithms for building the tree are specified by TreeInsertStrategy. 
	 * 
	 * @param o the object to be added.
	 * @param s the bounding sphere of the object - precomputed for memory and performance reasons
	 * @return false if the object does not intersect/or is not enclosed by this subtree, true otherwise.
	 */
	public boolean insert(Object3D o, Sphere s){
		if(s==null){ //objects without bounding sphere will potentially intersect every ray
			content.add(o);
			return true;
		}
		
		switch(strategy){
		case LEAF_ONLY:
			if(!CubeSphere.isSphereIntersectingCube(center, width/2, s.getPosition(), s.getRadius())){
				return false;
			}
			
			if(child!=null){
				child[0].insert(o,s);
				child[1].insert(o,s);
				child[2].insert(o,s);
				child[3].insert(o,s);
				child[4].insert(o,s);
				child[5].insert(o,s);
				child[6].insert(o,s);
				child[7].insert(o,s);
			}else{
				content.add(o);
			}
			
			if(child==null&&content.size()>TreeInsertStrategy.MAX_ELEMENTS&&width>TreeInsertStrategy.MIN_WIDTH){
				split();
			}
			
			return true;
		case FIT_INTO_BOX:
			if(!o.isEnclosedByCube(center, width/2))
				return false;
			
			if(child!=null){
				if(child[0].insert(o,s)||
				   child[1].insert(o,s)||
				   child[2].insert(o,s)||
				   child[3].insert(o,s)||
				   child[4].insert(o,s)||
				   child[5].insert(o,s)||
				   child[6].insert(o,s)||
				   child[7].insert(o,s))
					return true;
			}
			
			content.add(o);
			
			if(child==null&&content.size()>TreeInsertStrategy.MAX_ELEMENTS&&width>TreeInsertStrategy.MIN_WIDTH)
				split();
			
			return true;
		case DYNAMIC:
			//if sphere is not touching cube -> error
			if(!CubeSphere.isSphereIntersectingCube(center, width/2, s.getPosition(), s.getRadius())){
				return false;
			}
			
			//if object is enclosed by any child, add it there
			if(child != null){
				for(Node n : child){
					if(o.isEnclosedByCube(n.center, n.width/2))
						return n.insert(o, s);
				}
			}
			
			//if object is very small (in relation to the box) - duplicate it to child nodes
			//add it to this node otherwise
			if(child!=null && s.getRadius()/width<TreeInsertStrategy.DYNAMIC_DUPLICATE_MAX_SIZE_RATIO){
				child[0].insert(o,s);
				child[1].insert(o,s);
				child[2].insert(o,s);
				child[3].insert(o,s);
				child[4].insert(o,s);
				child[5].insert(o,s);
				child[6].insert(o,s);
				child[7].insert(o,s);
			}else{
				content.add(o);
			}
			
			//if node too full split it
			if(child==null&&content.size()>2/*&&width>TreeInsertStrategy.DYNAMIC_MIN_WIDTH*/){
				split();
			}
			
			return true;
		case DYNAMIC_TEST:
			//if sphere is not touching cube -> error
			if(!CubeSphere.isSphereIntersectingCube(center, width/2, s.getPosition(), s.getRadius())){
				return false;
			}
			
			//if object is enclosed by any child, add it there
			if(child != null){
				for(Node n : child){
					if(o.isEnclosedByCube(n.center, n.width/2))
						return n.insert(o, s);
				}
				
				boolean i0 = CubeSphere.isSphereIntersectingCube(child[0].center, child[0].width/2, s.getPosition(), s.getRadius()),
		        i1 = CubeSphere.isSphereIntersectingCube(child[1].center, child[1].width/2, s.getPosition(), s.getRadius()),
		        i2 = CubeSphere.isSphereIntersectingCube(child[2].center, child[2].width/2, s.getPosition(), s.getRadius()),
		        i3 = CubeSphere.isSphereIntersectingCube(child[3].center, child[3].width/2, s.getPosition(), s.getRadius()),
		        i4 = CubeSphere.isSphereIntersectingCube(child[4].center, child[4].width/2, s.getPosition(), s.getRadius()),
		        i5 = CubeSphere.isSphereIntersectingCube(child[5].center, child[5].width/2, s.getPosition(), s.getRadius()),
		        i6 = CubeSphere.isSphereIntersectingCube(child[6].center, child[6].width/2, s.getPosition(), s.getRadius()),
		        i7 = CubeSphere.isSphereIntersectingCube(child[7].center, child[7].width/2, s.getPosition(), s.getRadius());
		
				int intersectionCount=0;
				
				if(i0) intersectionCount++;
				if(i1) intersectionCount++;
				if(i2) intersectionCount++;
				if(i3) intersectionCount++;
				if(i4) intersectionCount++;
				if(i5) intersectionCount++;
				if(i6) intersectionCount++;
				if(i7) intersectionCount++;
				
				if(intersectionCount==1||intersectionCount*s.getRadius()/width<0.35){
					if(i0) child[0].insert(o, s);
					if(i1) child[1].insert(o, s);
					if(i2) child[2].insert(o, s);
					if(i3) child[3].insert(o, s);
					if(i4) child[4].insert(o, s);
					if(i5) child[5].insert(o, s);
					if(i6) child[6].insert(o, s);
					if(i7) child[7].insert(o, s);
					
					return true;
				}
			}
			
			content.add(o);
			
			//if node too full split it
			if(child==null&&content.size()>2){
				split();
			}
			
			return true;
		case FAST_BUILD_TEST:
			//if sphere is not touching cube -> error
			if(!CubeSphere.isSphereIntersectingCube(center, width/2, s.getPosition(), s.getRadius())){
				return false;
			}
			
			//if object is enclosed by any child, add it there
			if(child != null){
				boolean i0 = CubeSphere.isSphereIntersectingCube(child[0].center, child[0].width/2, s.getPosition(), s.getRadius()),
				        i1 = CubeSphere.isSphereIntersectingCube(child[1].center, child[1].width/2, s.getPosition(), s.getRadius()),
				        i2 = CubeSphere.isSphereIntersectingCube(child[2].center, child[2].width/2, s.getPosition(), s.getRadius()),
				        i3 = CubeSphere.isSphereIntersectingCube(child[3].center, child[3].width/2, s.getPosition(), s.getRadius()),
				        i4 = CubeSphere.isSphereIntersectingCube(child[4].center, child[4].width/2, s.getPosition(), s.getRadius()),
				        i5 = CubeSphere.isSphereIntersectingCube(child[5].center, child[5].width/2, s.getPosition(), s.getRadius()),
				        i6 = CubeSphere.isSphereIntersectingCube(child[6].center, child[6].width/2, s.getPosition(), s.getRadius()),
				        i7 = CubeSphere.isSphereIntersectingCube(child[7].center, child[7].width/2, s.getPosition(), s.getRadius());
				
				int intersectionCount=0;
				
				if(i0) intersectionCount++;
				if(i1) intersectionCount++;
				if(i2) intersectionCount++;
				if(i3) intersectionCount++;
				if(i4) intersectionCount++;
				if(i5) intersectionCount++;
				if(i6) intersectionCount++;
				if(i7) intersectionCount++;
				
				if(intersectionCount==1||intersectionCount*s.getRadius()/width<0.5){
					if(i0) child[0].insert(o, s);
					if(i1) child[1].insert(o, s);
					if(i2) child[2].insert(o, s);
					if(i3) child[3].insert(o, s);
					if(i4) child[4].insert(o, s);
					if(i5) child[5].insert(o, s);
					if(i6) child[6].insert(o, s);
					if(i7) child[7].insert(o, s);
					
					return true;
				}
			}
			
			content.add(o);
		
			//if node too full split it
			if(child==null&&content.size()>TreeInsertStrategy.MAX_ELEMENTS&&width>TreeInsertStrategy.DYNAMIC_MIN_WIDTH){
				split();
			}
			
			return true;
		default:
			throw new RuntimeException("Mode not implemented: "+strategy);
		}
	}
	
	/**
	 * Splits this node into 8 child-nodes and re-inserts the content
	 */
	private void split(){
		if(child!=null)
			throw new RuntimeException("double split detected");
		
		child = new Node[8];
		double w2 = width/2;
		double w4 = width/4;
		
		child[0] = new Node(new Vect3(center.data[0] + w4, center.data[1] + w4, center.data[2] + w4),this,w2);
		child[1] = new Node(new Vect3(center.data[0] + w4, center.data[1] + w4, center.data[2] - w4),this,w2);
		child[2] = new Node(new Vect3(center.data[0] + w4, center.data[1] - w4, center.data[2] + w4),this,w2);
		child[3] = new Node(new Vect3(center.data[0] + w4, center.data[1] - w4, center.data[2] - w4),this,w2);
		child[4] = new Node(new Vect3(center.data[0] - w4, center.data[1] + w4, center.data[2] + w4),this,w2);
		child[5] = new Node(new Vect3(center.data[0] - w4, center.data[1] + w4, center.data[2] - w4),this,w2);
		child[6] = new Node(new Vect3(center.data[0] - w4, center.data[1] - w4, center.data[2] + w4),this,w2);
		child[7] = new Node(new Vect3(center.data[0] - w4, center.data[1] - w4, center.data[2] - w4),this,w2);
		
		List<Object3D> oldContent = content;
		content = new ArrayList<Object3D>();
		
		for(Object3D o : oldContent){
			if(!insert(o, o.getBoundingSphere())){
				throw new RuntimeException("could not insert: "+o);
			}
		}
	}
	
	/**
	 * March through Octree checking collisions. Every node on the way (when traversing to leaf direction) is checked for hits.
	 * 
	 * @param v point to search for
	 * @param c collision details
	 * @return smallest node containing v, or null if (and only if) v is not inside the tree
	 */
	public Node marchToCheckingCollisions(Vect3 v, CollisionDetails c){
		if(encloses(v)){
			if(child==null){
				return this;
			}
			
			for(Node n : child){
				if(n.encloses(v)){
					c.checkCollisionSet(n.content);
					return n.marchToCheckingCollisions(v, c);
				}
			}
			
			return this;
		}else{
			if(parent!=null)
				return parent.marchToCheckingCollisions(v, c);
			else
				return null;
		}
	}
	
	@Override
	public String toString(){
		return center+"+/-"+width/2;
	}
	
	public void addStringRep(StringBuilder sb, int layer){
		for(int i=0;i<layer;i++)
			sb.append("  ");
		sb.append(toString());
		sb.append(":");
		sb.append(content);
		sb.append("\n");
		
		if(child!=null)
			for(Node n : child){
				n.addStringRep(sb, layer+1);
			}
	}
	
	/**
	 * @return the number of objects stored in this subtree (duplicates are count multiple times)
	 */
	public int getSize(){
		int size = content.size();
		
		if(child!=null)
			for(Node n : child)
				size += n.getSize();
			
		return size;
	}
	
	/**
	 * Calculates: <code>s = sum(layer)</code> where sum is the sum over all objects stored in 
	 * this subtree and layer at which the object is stored (the root has layer 0).
	 * <p/>
	 * <code>s/getSize()</code> defines the average layer of the object.  
	 * @param layer
	 * @return s
	 */
	public long getContentDepthSum(int layer){
		long sum = 0;
		
		sum += content.size()*(long)layer;
		
		if(child!=null)
			for(Node n : child)
				sum += n.getContentDepthSum(layer+1);
		
		return sum;
	}
	
	/**
	 * Removes child nodes in case none of them has any content
	 */
	public void compress(){
		if(getSize()-content.size()==0&&child!=null){
			child=null;
		}
		
		if(child!=null){
			for(Node n : child)
				n.compress();
		}
	}
	
	/**
	 * @return number of nodes in this subtree
	 */
	public int getNodeCount(){
		int nc=1;
		
		if(child!=null)
			for(Node n : child)
				nc += n.getNodeCount();
		
		return nc;
	}
}
