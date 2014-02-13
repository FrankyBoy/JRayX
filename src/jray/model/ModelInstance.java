package jray.model;

import java.util.HashMap;
import java.util.Map;

import jray.common.Matrix4;
import jray.common.Object3D;
import jray.common.Ray;
import jray.common.Vect3;
import jray.math.Vect;
import jray.math.intersections.RaySphere;
import jray.struct.CollisionDetails;
import jray.struct.RayPath;

public class ModelInstance extends Object3D{

	private TriangleMeshModel model;
	private Map<Thread, CollisionData> lastCollision = new HashMap<Thread, CollisionData>();
	
	public ModelInstance(Vect3 position, TriangleMeshModel model) {
		super(position, new Vect3());
		this.model = model;
	}

	@Override
	public void rotate(Matrix4 rotationMatrix) {
		throw new RuntimeException("not implemented");
	}

	@Override
	public double getHitPointDistance(Ray r) {
		double dist;
		Ray subRay;
		Vect3 tmp = new Vect3();
		Vect.add(position, model.getBoundingSphere().getPosition(), tmp);
		
		if(RaySphere.isRayOriginatingInSphere(r.getOrigin(), r.getDirection(), tmp, model.getBoundingSphere().getRadius())){
			Vect.subtract(r.getOrigin(), position, tmp);
			subRay = new Ray(tmp, r.getDirection());
			
			dist = 0;
		}else{
			dist=RaySphere.getHitPointRaySphereDistance(r.getOrigin(), r.getDirection(), tmp, model.getBoundingSphere().getRadius());
			
			if(Double.isInfinite(dist))
				return dist;
			
			Vect.addMultiple(r.getOrigin(), r.getDirection(), dist, tmp);
			Vect.subtract(tmp, position, tmp);
			subRay = new Ray(tmp, r.getDirection());
		}
		
		CollisionData d = new CollisionData();
		lastCollision.put(Thread.currentThread(), d);
		d.d = RayPath.getFirstCollision(model.getTree(), subRay);
		if(!Double.isInfinite(d.d.d)){
			Vect3 hitPointLocal = new Vect3();
			Vect.addMultiple(subRay.getOrigin(), subRay.getDirection(), d.d.d, hitPointLocal);
			d.hitPointLocal = hitPointLocal;
			Vect3 hitPointGlobal = new Vect3();
			Vect.add(hitPointLocal, position, hitPointGlobal);
			d.hitPointGlobal = hitPointGlobal;
			return d.d.d + dist;
		}else{
			return d.d.d;
		}
	}

	@Override
	public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
		CollisionData d = lastCollision.get(Thread.currentThread());
		
		if(!d.hitPointGlobal.equals(hitPoint, 1e-9))
			throw new RuntimeException("hitpoint not in cache: "+hitPoint);
		
		d.d.o.getNormalAt(d.hitPointLocal, normal);
	}

	@Override
	public boolean contains(Vect3 hitPoint) {
		throw new RuntimeException("not implemented");
	}
    
	@Override
    public Vect3 getBoundingSphereCenter(){
    	Vect3 pos = new Vect3();
    	Vect.add(position, model.getBoundingSphere().getPosition(), pos);
    	return pos;
    }
    
	@Override
    public double getBoundingSphereRadius(){
    	return model.getBoundingSphere().getRadius();
    }
}

class CollisionData{
	CollisionDetails d;
	Vect3 hitPointGlobal;
	Vect3 hitPointLocal;
}
