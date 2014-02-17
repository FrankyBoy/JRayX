using System;
using System.Diagnostics;
using System.Text;
using Common.Logging;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public class Octree
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private static readonly Stopwatch Sw = new Stopwatch();
        protected Node Root;

        public Octree(Vect3 center, double width)
        {
            Root = new Node(center, null, width);
        }

        public void Add(I3DObject o)
        {
            if (!Root.Insert(o, o.GetBoundingSphere()))
            {
                throw new Exception("Could not insert: " + o);
            }
        }

        public Node GetRoot()
        {
            return Root;
        }

        public static Octree BuildTree(Vect3 center, I3DObject[] objects)
        {
            double maxQuadDist = 0;

            Log.Debug("Building octree... ");

            foreach (I3DObject o in objects)
            {
                Sphere s = o.GetBoundingSphere();
                if (s != null)
                {
                    Vect3 dist = center - s.Position;
                    double qdist = s.GetRadius();
                    if (double.IsInfinity(qdist) || Double.IsNaN(qdist))
                        throw new Exception("Invalid BoundingSphere: " + s + " from " + o);
                    qdist = qdist*qdist + dist.QuadLength();
                    if (maxQuadDist < qdist)
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
            while (true)
            {
                t = new Octree(center, sizeHint);
                foreach (I3DObject o in objects)
                {
                    if (!t.Root.Insert(o, o.GetBoundingSphere()))
                    {
                        sizeHint *= 2;
                        Log.Warn("Warning - resize needed!");
                        goto cont;
                    }
                }
                break;

                cont:
                {
                }
            }

            t.GetRoot().Compress();
            Sw.Stop();

            Log.Debug(string.Format("{0} ms\n", Sw.ElapsedMilliseconds));
            Log.Debug(string.Format(" - contains {0} of {1} elements ({2:0.##}%)",
                                    t.GetRoot().GetSize(), objects.Length,
                                    t.GetRoot().GetSize()/(float) objects.Length*100));
            Log.Debug(" - avg depth: " + t.GetAverageObjectDepth());
            Log.Debug(" - node count: " + t.GetRoot().GetNodeCount());
            Log.Debug(" - objects per node: " + t.GetRoot().GetSize()/(float) t.GetRoot().GetNodeCount());

            return t;
        }

        public new string ToString()
        {
            var sb = new StringBuilder();
            Root.AddStringRep(sb, 0);
            return sb.ToString();
        }

        public double GetAverageObjectDepth()
        {
            return Root.GetContentDepthSum(0)/(double) Root.GetSize();
        }
    }
}