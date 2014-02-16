using System;
using System.Collections.Generic;
using System.Threading;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Model
{
    public class ModelInstance : Basic3DObject{

        private readonly TriangleMeshModel _model;
        private readonly Dictionary<Thread, CollisionData> _lastCollision = new Dictionary<Thread, CollisionData>();

        public ModelInstance(Vect3 position, TriangleMeshModel model) : base(position, new Vect3(0)) {
            _model = model;
        }

        public override void Rotate(Matrix4 rotationMatrix) {
            throw new Exception("not implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r) {
            double dist;
            Shapes.Ray subRay;
            var tmp = Vect.Add(Position, _model.GetBoundingSphere().Position);
		
            if(RaySphere.IsRayOriginatingInSphere(r.GetOrigin(), r.Direction, tmp, _model.GetBoundingSphere().GetRadius())){
                tmp = Vect.Subtract(r.GetOrigin(), Position);
                subRay = new Shapes.Ray(tmp, r.Direction);
			
                dist = 0;
            }else{
                dist = RaySphere.GetHitPointRaySphereDistance(r.GetOrigin(), r.Direction, tmp, _model.GetBoundingSphere().GetRadius());
			
                if(double.IsInfinity(dist))
                    return dist;

                Vect.AddMultiple(r.GetOrigin(), r.Direction, dist, ref tmp);
                tmp = Vect.Subtract(tmp, Position);
                subRay = new Shapes.Ray(tmp, r.Direction);
            }
		
            var d = new CollisionData
                {
                    Details = RayPath.GetFirstCollision(_model.GetTree(), subRay)
                };

            if(!double.IsInfinity(d.Details.Distance)){
                var hitPointLocal = new Vect3(0);
                Vect.AddMultiple(subRay.GetOrigin(), subRay.Direction, d.Details.Distance, ref hitPointLocal);
                d.HitPointLocal = hitPointLocal;
                var hitPointGlobal = Vect.Add(hitPointLocal, Position);
                d.HitPointGlobal = hitPointGlobal;
                return d.Details.Distance + dist;
            }
            _lastCollision[Thread.CurrentThread] = d;
            return d.Details.Distance;
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint) {
            CollisionData d;

            if (_lastCollision.TryGetValue(Thread.CurrentThread, out d)
                && d.HitPointGlobal.Equals(hitPoint))
            {
                return d.Details.Obj.GetNormalAt(d.HitPointLocal);
            }

            throw new Exception("hitpoint not in cache: " + hitPoint);
        }

        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("not implemented");
        }
    
        public new Vect3 GetBoundingSphereCenter(){
            var pos = Vect.Add(Position, _model.GetBoundingSphere().Position);
            return pos;
        }
    
        public override double GetBoundingSphereRadius(){
            return _model.GetBoundingSphere().GetRadius();
        }
    }

    internal class CollisionData{
        internal CollisionDetails Details;
        internal Vect3 HitPointGlobal;
        internal Vect3 HitPointLocal;
    }
}