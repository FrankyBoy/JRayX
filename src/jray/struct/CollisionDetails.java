package jray.struct;

import java.util.List;

import jray.common.Object3D;
import jray.common.Ray;
import jray.math.Constants;

/**
 * Contains the information built by RayPath when casting a Ray into an octree.
 * <p/>
 * The members of this object must always satisfy:
 *  <li><code>r.o + r.d*d</code> is a point on the hull of object <code>o</code></li>
 *  <li>or <code>d == Double.POSITIVE_INFINITY</code> and <code>o == null</code></li>
 * in addition:
 *  <li><code>checks</code> is the number of intersection tests made</li>
 */
public class CollisionDetails{
	public Ray r;
	public Object3D o;
	public double d;
	public int checks = 0;
	
	public CollisionDetails(Ray r) {
		this.o = null;
		this.d = Double.POSITIVE_INFINITY;
		this.r = r;
	}
	
	public void checkCollisionSet(List<Object3D> objects){
        for (Object3D candidate : objects) {
        	checks++;
            double distanceCandidate = candidate.getHitPointDistance(r);
            if (distanceCandidate > Constants.MIN_DISTANCE && distanceCandidate < d) {
                o = candidate;
                d = distanceCandidate;
            }
        }
	}
	
	public boolean hasHit(){
		return o!=null;
	}
	
	public int getCheckCount(){
		return checks;
	}
}
