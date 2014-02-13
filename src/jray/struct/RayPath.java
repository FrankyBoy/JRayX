package jray.struct;

import jray.common.Ray;
import jray.common.Vect3;
import jray.math.Vect;
import jray.math.intersections.RayCube;

/**
 * Contains the logic to determine the first intersection of a ray traveling
 * through objects stored in an octree. The ray's origin must be enclosed by the
 * octree.
 */
public class RayPath {
	public static CollisionDetails getFirstCollision(Octree tree, Ray r){
		CollisionDetails c = new CollisionDetails(r);
		
		double d=0, 
		       distanceTravelled=0; //distance traveled since the ray's origin
		
		//Algorithm starts at the ray's origin and in the root node.
		Vect3 pos = new Vect3(r.getOrigin());
		Node n = tree.getRoot();
		c.checkCollisionSet(n.content);
		
		if(!n.encloses(pos))
			throw new RuntimeException("Ray's origin is not located in the octree!");
		
		do{
			//March to the leaf containing "pos" and check collisions
			n = n.marchToCheckingCollisions(pos, c);
			
			//No leaf is containing "pos" -> left tree -> stop searching
			if(n==null)
				break;
			
			//march out of current node
			d = RayCube.getDistanceToBorderPlane(pos, r.getDirection(), n.center, n.width/2) + TreeInsertStrategy.EPS*1e4;
			Vect.addMultiple(pos, r.getDirection(), d, pos);
			distanceTravelled+=d;
			
			//in case eps was not enough
			while(n.encloses(pos)){
				d = TreeInsertStrategy.EPS*1e4;
				Vect.addMultiple(pos, r.getDirection(), d, pos);
				distanceTravelled+=d;
			}
			
			/**
			 * When passing a box-border, all intersection tests between the ray's origin and the
			 * box-border-intersection-point must have taken place (and some beyond the box-border too).
			 * Therefore, if an intersection located between the ray's origin and the box-border has been found, this
			 * must be the nearest intersection and the search may stop.
			 */
		}while(c.d>distanceTravelled);
		
		return c;
	}
}
