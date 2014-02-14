
/**
 * A node of an octree located at <code>center +/- width/2</code> (a cube).<p/>
 * 
 * If a point is contained in a leaf-node, it is guaranteed that any object intersecting this point can be found in the leaf node or it's parents.
 * It is NOT guaranteed that each object is stored only once in an octree (although a TreeInsertStrategy may try to do this to save memory).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public class Node{
        public static TreeInsertStrategy strategy = TreeInsertStrategy.DynamicTest;
        public static double MIN_NODE_WIDTH = 1e-9;

        private readonly Node parent; //parent node
        private Node[] _child; //child nodes
        public List<Object3D> Content = new List<Object3D>(); //objects contained in this node
        public readonly Vect3 Center; //central point of the nodes cube
        public readonly double Width; //node reaches from center-width/2 to center+width/2
	
        public Node(Vect3 center, Node parent, double width){
            this.parent = parent;
            Width = width;
            Center = center;
		
            if(width<MIN_NODE_WIDTH)
                throw new Exception("Node width too small: "+width);
        }
	
        /**
	 * @param v a <code>Vect3</code> representing a point
	 * @return true if, and only if, <code>v</code> is enclosed by this subtree.
	 */
        public bool encloses(Vect3 v){
            return PointCube.Encloses(Center, Width/2 + Constants.EPS, v);
        }
	
        /**
	 * Inserts an object into the subtree. The actual algorithms for building the tree are specified by TreeInsertStrategy. 
	 * 
	 * @param o the object to be added.
	 * @param s the bounding sphere of the object - precomputed for memory and performance reasons
	 * @return false if the object does not intersect/or is not enclosed by this subtree, true otherwise.
	 */
        public bool insert(Object3D o, Sphere s){
            if(s==null){ //objects without bounding sphere will potentially intersect every ray
                Content.Add(o);
                return true;
            }
		
            switch(strategy){
                case TreeInsertStrategy.LeafOnly:
                    if(!CubeSphere.IsSphereIntersectingCube(Center, Width/2, s.Position, s.GetRadius())){
                        return false;
                    }
			
                    if(_child!=null){
                        _child[0].insert(o,s);
                        _child[1].insert(o,s);
                        _child[2].insert(o,s);
                        _child[3].insert(o,s);
                        _child[4].insert(o,s);
                        _child[5].insert(o,s);
                        _child[6].insert(o,s);
                        _child[7].insert(o,s);
                    }else{
                        Content.Add(o);
                    }
			
                    if(_child==null
                        && Content.Count > TreeInsertStrategyConstants.MaxElements
                        && Width > TreeInsertStrategyConstants.MinWidth)
                    {
                        split();
                    }
			
                    return true;
                case TreeInsertStrategy.FitIntoBox:
                    if(!o.IsEnclosedByCube(Center, Width/2))
                        return false;
			
                    if(_child!=null){
                        if(_child[0].insert(o,s)||
                           _child[1].insert(o,s)||
                           _child[2].insert(o,s)||
                           _child[3].insert(o,s)||
                           _child[4].insert(o,s)||
                           _child[5].insert(o,s)||
                           _child[6].insert(o,s)||
                           _child[7].insert(o,s))
                            return true;
                    }
			
                    Content.Add(o);
			
                    if(_child==null
                        && Content.Count > TreeInsertStrategyConstants.MaxElements
                        && Width > TreeInsertStrategyConstants.MinWidth)
                    {
                        split();
                    }
			
                    return true;
                case TreeInsertStrategy.Dynamic:
                    //if sphere is not touching cube -> error
                    if(!CubeSphere.IsSphereIntersectingCube(Center, Width/2, s.Position, s.GetRadius())){
                        return false;
                    }
			
                    //if object is enclosed by any child, add it there
                    if(_child != null){
                        foreach (Node n in _child){
                            if(o.IsEnclosedByCube(n.Center, n.Width/2))
                                return n.insert(o, s);
                        }
                    }
			
                    //if object is very small (in relation to the box) - duplicate it to child nodes
                    //add it to this node otherwise
                    if(_child!=null && s.GetRadius()/Width<TreeInsertStrategyConstants.DynamicDuplicateMaxSizeRatio){
                        _child[0].insert(o,s);
                        _child[1].insert(o,s);
                        _child[2].insert(o,s);
                        _child[3].insert(o,s);
                        _child[4].insert(o,s);
                        _child[5].insert(o,s);
                        _child[6].insert(o,s);
                        _child[7].insert(o,s);
                    }else{
                        Content.Add(o);
                    }
			
                    //if node too full split it
                    if(_child==null&&Content.Count>2/*&&width>TreeInsertStrategy.DYNAMIC_MIN_WIDTH*/){
                        split();
                    }
			
                    return true;
                case TreeInsertStrategy.DynamicTest:
                    //if sphere is not touching cube -> error
                    if(!CubeSphere.IsSphereIntersectingCube(Center, Width/2, s.Position, s.GetRadius())){
                        return false;
                    }
			
                    //if object is enclosed by any child, add it there
                    if(_child != null){
                        foreach (Node n in _child){
                            if(o.IsEnclosedByCube(n.Center, n.Width/2))
                                return n.insert(o, s);
                        }
				
                        bool i0 = CubeSphere.IsSphereIntersectingCube(_child[0].Center, _child[0].Width/2, s.Position, s.GetRadius()),
                                i1 = CubeSphere.IsSphereIntersectingCube(_child[1].Center, _child[1].Width/2, s.Position, s.GetRadius()),
                                i2 = CubeSphere.IsSphereIntersectingCube(_child[2].Center, _child[2].Width/2, s.Position, s.GetRadius()),
                                i3 = CubeSphere.IsSphereIntersectingCube(_child[3].Center, _child[3].Width/2, s.Position, s.GetRadius()),
                                i4 = CubeSphere.IsSphereIntersectingCube(_child[4].Center, _child[4].Width/2, s.Position, s.GetRadius()),
                                i5 = CubeSphere.IsSphereIntersectingCube(_child[5].Center, _child[5].Width/2, s.Position, s.GetRadius()),
                                i6 = CubeSphere.IsSphereIntersectingCube(_child[6].Center, _child[6].Width/2, s.Position, s.GetRadius()),
                                i7 = CubeSphere.IsSphereIntersectingCube(_child[7].Center, _child[7].Width/2, s.Position, s.GetRadius());
		
                        int intersectionCount=0;
				
                        if(i0) intersectionCount++;
                        if(i1) intersectionCount++;
                        if(i2) intersectionCount++;
                        if(i3) intersectionCount++;
                        if(i4) intersectionCount++;
                        if(i5) intersectionCount++;
                        if(i6) intersectionCount++;
                        if(i7) intersectionCount++;
				
                        if(intersectionCount==1||intersectionCount*s.GetRadius()/Width<0.35){
                            if(i0) _child[0].insert(o, s);
                            if(i1) _child[1].insert(o, s);
                            if(i2) _child[2].insert(o, s);
                            if(i3) _child[3].insert(o, s);
                            if(i4) _child[4].insert(o, s);
                            if(i5) _child[5].insert(o, s);
                            if(i6) _child[6].insert(o, s);
                            if(i7) _child[7].insert(o, s);
					
                            return true;
                        }
                    }
			
                    Content.Add(o);
			
                    //if node too full split it
                    if(_child==null&&Content.Count>2){
                        split();
                    }
			
                    return true;
                case TreeInsertStrategy.FastBuildTest:
                    //if sphere is not touching cube -> error
                    if(!CubeSphere.IsSphereIntersectingCube(Center, Width/2, s.Position, s.GetRadius())){
                        return false;
                    }
			
                    //if object is enclosed by any child, add it there
                    if(_child != null){
                        bool i0 = CubeSphere.IsSphereIntersectingCube(_child[0].Center, _child[0].Width/2, s.Position, s.GetRadius()),
                                i1 = CubeSphere.IsSphereIntersectingCube(_child[1].Center, _child[1].Width/2, s.Position, s.GetRadius()),
                                i2 = CubeSphere.IsSphereIntersectingCube(_child[2].Center, _child[2].Width/2, s.Position, s.GetRadius()),
                                i3 = CubeSphere.IsSphereIntersectingCube(_child[3].Center, _child[3].Width/2, s.Position, s.GetRadius()),
                                i4 = CubeSphere.IsSphereIntersectingCube(_child[4].Center, _child[4].Width/2, s.Position, s.GetRadius()),
                                i5 = CubeSphere.IsSphereIntersectingCube(_child[5].Center, _child[5].Width/2, s.Position, s.GetRadius()),
                                i6 = CubeSphere.IsSphereIntersectingCube(_child[6].Center, _child[6].Width/2, s.Position, s.GetRadius()),
                                i7 = CubeSphere.IsSphereIntersectingCube(_child[7].Center, _child[7].Width/2, s.Position, s.GetRadius());
				
                        int intersectionCount=0;
				
                        if(i0) intersectionCount++;
                        if(i1) intersectionCount++;
                        if(i2) intersectionCount++;
                        if(i3) intersectionCount++;
                        if(i4) intersectionCount++;
                        if(i5) intersectionCount++;
                        if(i6) intersectionCount++;
                        if(i7) intersectionCount++;
				
                        if(intersectionCount==1||intersectionCount*s.GetRadius()/Width<0.5){
                            if(i0) _child[0].insert(o, s);
                            if(i1) _child[1].insert(o, s);
                            if(i2) _child[2].insert(o, s);
                            if(i3) _child[3].insert(o, s);
                            if(i4) _child[4].insert(o, s);
                            if(i5) _child[5].insert(o, s);
                            if(i6) _child[6].insert(o, s);
                            if(i7) _child[7].insert(o, s);
					
                            return true;
                        }
                    }
			
                    Content.Add(o);
		
                    //if node too full split it
                    if(_child==null&&Content.Count>TreeInsertStrategyConstants.MaxElements&&Width>TreeInsertStrategyConstants.DynamicMinWidth){
                        split();
                    }
			
                    return true;
                default:
                    throw new Exception("Mode not implemented: "+strategy);
            }
        }
	
        /**
	 * Splits this node into 8 child-nodes and re-inserts the content
	 */
        private void split(){
            if(_child!=null)
                throw new Exception("double split detected");
		
            _child = new Node[8];
            double w2 = Width/2;
            double w4 = Width/4;
		
            _child[0] = new Node(new Vect3(Center.Data[0] + w4, Center.Data[1] + w4, Center.Data[2] + w4),this,w2);
            _child[1] = new Node(new Vect3(Center.Data[0] + w4, Center.Data[1] + w4, Center.Data[2] - w4),this,w2);
            _child[2] = new Node(new Vect3(Center.Data[0] + w4, Center.Data[1] - w4, Center.Data[2] + w4),this,w2);
            _child[3] = new Node(new Vect3(Center.Data[0] + w4, Center.Data[1] - w4, Center.Data[2] - w4),this,w2);
            _child[4] = new Node(new Vect3(Center.Data[0] - w4, Center.Data[1] + w4, Center.Data[2] + w4),this,w2);
            _child[5] = new Node(new Vect3(Center.Data[0] - w4, Center.Data[1] + w4, Center.Data[2] - w4),this,w2);
            _child[6] = new Node(new Vect3(Center.Data[0] - w4, Center.Data[1] - w4, Center.Data[2] + w4),this,w2);
            _child[7] = new Node(new Vect3(Center.Data[0] - w4, Center.Data[1] - w4, Center.Data[2] - w4),this,w2);
		
            List<Object3D> oldContent = Content;
            Content = new List<Object3D>();
		
            foreach (Object3D o in oldContent){
                if(!insert(o, o.GetBoundingSphere())){
                    throw new Exception("could not insert: "+o);
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
                if(_child==null){
                    return this;
                }
			
                foreach(Node n in _child){
                    if(n.encloses(v)){
                        c.CheckCollisionSet(n.Content);
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
	
        public new String ToString(){
            return Center+"+/-"+Width/2;
        }
	
        public void addStringRep(StringBuilder sb, int layer){
            for(int i=0;i<layer;i++)
                sb.Append("  ");
            sb.Append(ToString());
            sb.Append(":");
            sb.Append(Content);
            sb.Append("\n");
		
            if(_child!=null)
                foreach(Node n in _child){
                    n.addStringRep(sb, layer+1);
                }
        }
	
        /**
	 * @return the number of objects stored in this subtree (duplicates are count multiple times)
	 */
        public int getSize()
        {
            int size = Content.Count;
		
            if(_child!=null)
                size += _child.Sum(n => n.getSize());

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
		
            sum += Content.Count*(long)layer;
		
            if(_child!=null)
                sum += _child.Sum(n => n.getContentDepthSum(layer + 1));

            return sum;
        }
	
        /**
	 * Removes child nodes in case none of them has any content
	 */
        public void compress(){
            if(getSize()-Content.Count==0&&_child!=null){
                _child=null;
            }
		
            if(_child!=null){
                foreach(Node n in _child)
                n.compress();
            }
        }
	
        /**
	 * @return number of nodes in this subtree
	 */
        public int getNodeCount(){
            int nc=1;
		
            if(_child!=null)
                nc += _child.Sum(n => n.getNodeCount());

            return nc;
        }
    }
}
