
/**
 * A classic octree with some additional constraints:
 *  - nodes are "complete" - they contain 0 or 8 child nodes.
 *  - objects may be stored in every node (not leaf nodes only)
 *  - some objects may be stored multiple times 
 */

using System;
using System.Collections.Generic;
using System.Text;
using JRayXLib.Math;
using Common.Logging;
using System.Diagnostics;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public class Octree{
        protected Node Root;
        private static ILog _log = LogManager.GetCurrentClassLogger();
        private static readonly Stopwatch Sw = new Stopwatch();

        public Octree(Vect3 center, double width){
            Root = new Node(center, null, width);
        }
	
        public void add(Object3D o){
            if(!Root.insert(o, o.GetBoundingSphere())){
                throw new Exception("Could not insert: "+o);
            }
        }
	
        public Node getRoot(){
            return Root;
        }
	
        public static Octree buildTree(Vect3 center, List<Object3D> objects){
            double maxQuadDist=0;
            var dist=new Vect3();
		
            _log.Debug("Building octree... ");
		
            foreach(Object3D o in objects){
                Sphere s = o.GetBoundingSphere();
                if(s!=null){
                    Vect.subtract(center, s.Position, dist);
                    double qdist = s.GetRadius();
                    if(double.IsInfinity(qdist)||Double.IsNaN(qdist))
                        throw new Exception("Invalid BoundingSphere: "+s+" from "+o);
                    qdist = qdist*qdist + dist.QuadLength();
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
            double sizeHint = System.Math.Sqrt(maxQuadDist)*2.1;
		
            Sw.Restart();
            Octree t;
            while(true){
                t = new Octree(center,sizeHint);
                foreach(Object3D o in objects){
                    if(!t.Root.insert(o, o.GetBoundingSphere())){
                        sizeHint *= 2;
                        _log.Warn("Warning - resize needed!");
                        goto cont;
                    }
                }
                break;

                cont:{}
            }

            t.getRoot().compress();
            Sw.Stop();
		
            _log.Debug(string.Format("{0} ms\n",Sw.ElapsedMilliseconds));
            _log.Debug(string.Format(" - contains {0} of {1} elements ({2:0.##}%)",
                t.getRoot().getSize(), objects.Count, t.getRoot().getSize()/(float)objects.Count*100));
            _log.Debug(" - avg depth: "+t.getAverageObjectDepth());
            _log.Debug(" - node count: "+t.getRoot().getNodeCount());
            _log.Debug(" - objects per node: "+t.getRoot().getSize()/(float)t.getRoot().getNodeCount());
		
            return t;
        }
	
        public new string ToString(){
            var sb = new StringBuilder();
            Root.addStringRep(sb, 0);
            return sb.ToString();
        }
	
        public double getAverageObjectDepth(){
            return Root.getContentDepthSum(0)/(double)Root.getSize();
        }
    }
}