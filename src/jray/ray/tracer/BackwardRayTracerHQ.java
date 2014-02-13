package jray.ray.tracer;

import jray.common.Ray;
import jray.common.Vect3;
import jray.math.Vect;
import jray.math.intersections.IntColors;
import jray.math.intersections.LongColors;
import jray.ray.Scene;
import jray.struct.CollisionDetails;

/**
 * A backward ray tracer using surface normals for simple light effects.
 */
public class BackwardRayTracerHQ extends BackwardRayTracer{

	private static final Vect3 LIGHT_DIRECTION = new Vect3(2, -1, -2); //direction of the sun's light-rays
	private static final double DIFFUSE_LIGHT_INTENSITY = 0.8;
	private static final double AMBIENT_LIGHT_INTENSITY = 0.2;
    
    public BackwardRayTracerHQ(Scene scene) {
        super(scene);
        LIGHT_DIRECTION.normalize();
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
        
        //Calculate hit position
        Vect3 hitPoint = new Vect3();
        Vect.addMultiple(ray.getOrigin(), ray.getDirection(), c.d, hitPoint);
        
        //Get color and normal at hitpoint
        long color = IntColors.toLong(c.o.getColorAt(hitPoint)); 
        Vect3 normal = new Vect3();
        c.o.getNormalAt(hitPoint, normal);
        
        //Check if anything is blocking direct sunlight (go where the sunlight comes from)
        Vect3 lrDir = new Vect3();
        Vect.scale(LIGHT_DIRECTION, -1, lrDir);
        Ray lightRay = new Ray(hitPoint, lrDir);
        CollisionDetails lc = findNearestHit(lightRay);
        
        //if nothing blocks the sun's light, add ambient and diffuse light, otherwise ambient only  
        double lightScale = 0;
        if(lc.o==null)
        	lightScale = Vect.dotProduct(normal, LIGHT_DIRECTION);
        lightScale = AMBIENT_LIGHT_INTENSITY + DIFFUSE_LIGHT_INTENSITY*(lightScale<0?-lightScale:0);
        
        return LongColors.scale(color, lightScale);
    }
}
