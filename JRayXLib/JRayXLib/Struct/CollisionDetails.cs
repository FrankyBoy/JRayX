
/**
 * Contains the information built by RayPath when casting a Ray into an octree.
 * <p/>
 * The members of this object must always satisfy:
 *  <li><code>r.o + r.d*d</code> is a point on the hull of object <code>o</code></li>
 *  <li>or <code>d == Double.POSITIVE_INFINITY</code> and <code>o == null</code></li>
 * in addition:
 *  <li><code>checks</code> is the number of intersection tests made</li>
 */

using System.Collections.Generic;
using JRayXLib.Common;
using JRayXLib.Math;

namespace JRayXLib.Struct
{
    public class CollisionDetails{
        public Ray r;
        public Object3D o;
        public double d;
        public int checks = 0;
	
        public CollisionDetails(Ray r) {
            this.o = null;
            this.d = double.PositiveInfinity;
            this.r = r;
        }
	
        public void checkCollisionSet(List<Object3D> objects){
            foreach (Object3D candidate in objects) {
                checks++;
                double distanceCandidate = candidate.GetHitPointDistance(r);
                if (distanceCandidate > Constants.MinDistance && distanceCandidate < d) {
                    o = candidate;
                    d = distanceCandidate;
                }
            }
        }
	
        public bool hasHit(){
            return o!=null;
        }
	
        public int getCheckCount(){
            return checks;
        }
    }
}
