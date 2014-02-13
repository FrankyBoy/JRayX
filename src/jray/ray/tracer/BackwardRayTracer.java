package jray.ray.tracer;

import java.util.ArrayList;
import java.util.List;

import jray.common.Object3D;
import jray.common.Ray;
import jray.common.Vect3;
import jray.math.Vect;
import jray.math.intersections.IntColors;
import jray.ray.Scene;
import jray.struct.CollisionDetails;
import jray.struct.Octree;
import jray.struct.RayPath;


public class BackwardRayTracer {

	protected static final boolean USE_OCTREE = true;
	protected static final int MAX_RECURSION_DEPTH = 100;
	protected Object3D[] objects;
    
	protected Octree tree;
    
    public BackwardRayTracer(Scene scene) {
        List<Object3D> allObjects = new ArrayList<Object3D>();
        allObjects.addAll(scene.getObjects());
        
        objects = allObjects.toArray(new Object3D[allObjects.size()]);
        
        tree = scene.getSceneTree();
    }

    public long shoot(Ray ray) {
    	return shootRay(ray, 0);
    }

    protected long shootRay(Ray ray, int level) {
        if (level == MAX_RECURSION_DEPTH) {
            return 0xFFFF00FF00000000L;
        }

        //Find nearest Hit
        CollisionDetails c = findNearestHit(ray);

        //No hit
        if (c.o == null) {
            return 0xFFFF00FF00FF00FFL;
        }
        
        //Calculate hit point
        Vect3 hitPoint = new Vect3();
        Vect.addMultiple(ray.getOrigin(), ray.getDirection(), c.d, hitPoint); //Position des Treffers berechnen
        
        //Return object's color at hit point
        return IntColors.toLong(c.o.getColorAt(hitPoint));
    }

    protected CollisionDetails findNearestHit(Ray ray) {
    	if(!USE_OCTREE || tree==null){
    		CollisionDetails c = new CollisionDetails(ray);
    		
	        //Find nearest Hit
	        for (Object3D objectCandidate : objects) {
	            double distanceCandidate = objectCandidate.getHitPointDistance(ray);
	            if (distanceCandidate > 0 && distanceCandidate < c.d) {
	                c.o = objectCandidate;
	                c.d = distanceCandidate;
	                c.checks++;
	            }
	        }
	        
	        return c;
    	}else{
	    	return RayPath.getFirstCollision(tree, ray);
    	}
    }
}
