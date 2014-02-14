
/**
 * Contains the information built by RayPath when casting a Ray into an octree.
 * <p/>
 * The members of this object must always satisfy:
 *  <li><code>r.o + r.Distance*Distance</code> is a point on the hull of object <code>o</code></li>
 *  <li>or <code>Distance == Double.POSITIVE_INFINITY</code> and <code>o == null</code></li>
 * in addition:
 *  <li><code>checks</code> is the number of intersection tests made</li>
 */

using System.Collections.Generic;
using JRayXLib.Math;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public class CollisionDetails{
        public Shapes.Ray Ray;
        public Object3D Obj;
        public double Distance;
        public int Checks = 0;
	
        public CollisionDetails(Shapes.Ray ray) {
            Obj = null;
            Distance = double.PositiveInfinity;
            Ray = ray;
        }
	
        public void CheckCollisionSet(List<Object3D> objects){
            foreach (Object3D candidate in objects) {
                Checks++;
                double distanceCandidate = candidate.GetHitPointDistance(Ray);
                if (distanceCandidate > Constants.MinDistance && distanceCandidate < Distance) {
                    Obj = candidate;
                    Distance = distanceCandidate;
                }
            }
        }
	
        public bool HasHit(){
            return Obj!=null;
        }
	
        public int GetCheckCount(){
            return Checks;
        }
    }
}
