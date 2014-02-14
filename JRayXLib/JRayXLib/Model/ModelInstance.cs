using System;
using System.Collections.Generic;
using System.Threading;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Model
{
    public class ModelInstance : Object3D{

        private readonly TriangleMeshModel _model;
        private readonly Dictionary<Thread, CollisionData> _lastCollision = new Dictionary<Thread, CollisionData>();
	
        public ModelInstance(Vect3 position, TriangleMeshModel model) : base(position, new Vect3()) {
            _model = model;
        }

        public override void Rotate(Matrix4 rotationMatrix) {
            throw new Exception("not implemented");
        }

        public override double GetHitPointDistance(Ray r) {
            double dist;
            Ray subRay;
            var tmp = new Vect3();
            Vect.Add(Position, _model.GetBoundingSphere().Position, tmp);
		
            if(RaySphere.IsRayOriginatingInSphere(r.GetOrigin(), r.GetDirection(), tmp, _model.GetBoundingSphere().GetRadius())){
                Vect.subtract(r.GetOrigin(), Position, tmp);
                subRay = new Ray(tmp, r.GetDirection());
			
                dist = 0;
            }else{
                dist=RaySphere.GetHitPointRaySphereDistance(r.GetOrigin(), r.GetDirection(), tmp, _model.GetBoundingSphere().GetRadius());
			
                if(double.IsInfinity(dist))
                    return dist;
			
                Vect.AddMultiple(r.GetOrigin(), r.GetDirection(), dist, tmp);
                Vect.subtract(tmp, Position, tmp);
                subRay = new Ray(tmp, r.GetDirection());
            }
		
            var d = new CollisionData();
            _lastCollision.Add(Thread.CurrentThread, d);
            d.Details = RayPath.getFirstCollision(_model.GetTree(), subRay);
            if(!double.IsInfinity(d.Details.Distance)){
                var hitPointLocal = new Vect3();
                Vect.AddMultiple(subRay.GetOrigin(), subRay.GetDirection(), d.Details.Distance, hitPointLocal);
                d.HitPointLocal = hitPointLocal;
                var hitPointGlobal = new Vect3();
                Vect.Add(hitPointLocal, Position, hitPointGlobal);
                d.HitPointGlobal = hitPointGlobal;
                return d.Details.Distance + dist;
            }
            return d.Details.Distance;
        }

        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal) {
            CollisionData d = _lastCollision[Thread.CurrentThread];
		
            if(!d.HitPointGlobal.Equals(hitPoint, 1e-9))
                throw new Exception("hitpoint not in cache: "+hitPoint);
		
            d.Details.Obj.GetNormalAt(d.HitPointLocal, normal);
        }

        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("not implemented");
        }
    
        public new Vect3 GetBoundingSphereCenter(){
            var pos = new Vect3();
            Vect.Add(Position, _model.GetBoundingSphere().Position, pos);
            return pos;
        }
    
        public new double GetBoundingSphereRadius(){
            return _model.GetBoundingSphere().GetRadius();
        }
    }

    internal class CollisionData{
        internal CollisionDetails Details;
        internal Vect3 HitPointGlobal;
        internal Vect3 HitPointLocal;
    }
}