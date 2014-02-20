using System;
using System.Collections.Generic;
using System.Threading;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Model
{
    public class ModelInstance : Basic3DObject
    {
        private readonly Dictionary<Thread, CollisionData> _lastCollision = new Dictionary<Thread, CollisionData>();
        private readonly TriangleMeshModel _model;

        public ModelInstance(Vect3 position, TriangleMeshModel model) : base(position, new Vect3())
        {
            _model = model;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            throw new Exception("not implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r)
        {
            double dist;
            Shapes.Ray subRay;
            Vect3 tmp = Position + _model.GetBoundingSphere().Position;

            if (RaySphere.IsRayOriginatingInSphere(r.Origin, r.Direction, tmp,
                                                   _model.GetBoundingSphere().Radius))
            {
                tmp = r.Origin - Position;
                subRay = new Shapes.Ray
                    {
                        Origin = tmp,
                        Direction = r.Direction
                    };

                dist = 0;
            }
            else
            {
                dist = RaySphere.GetHitPointRaySphereDistance(r.Origin, r.Direction, tmp,
                                                              _model.GetBoundingSphere().Radius);

                if (double.IsInfinity(dist))
                    return dist;

                tmp = r.Origin + r.Direction*dist;
                tmp -= Position;
                subRay = new Shapes.Ray
                {
                    Origin = tmp,
                    Direction = r.Direction
                };
            }

            var d = new CollisionData
                {
                    Details = RayPath.GetFirstCollision(_model.GetTree(), subRay)
                };

            if (!double.IsInfinity(d.Details.Distance))
            {
                Vect3 hitPointLocal = subRay.Origin + subRay.Direction*d.Details.Distance;
                d.HitPointLocal = hitPointLocal;
                Vect3 hitPointGlobal = hitPointLocal + Position;
                d.HitPointGlobal = hitPointGlobal;
                return d.Details.Distance + dist;
            }
            _lastCollision[Thread.CurrentThread] = d;
            return d.Details.Distance;
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            CollisionData d;

            if (_lastCollision.TryGetValue(Thread.CurrentThread, out d)
                && d.HitPointGlobal.Equals(hitPoint))
            {
                return d.Details.Obj.GetNormalAt(d.HitPointLocal);
            }

            throw new Exception("hitpoint not in cache: " + hitPoint);
        }

        public override bool Contains(Vect3 hitPoint)
        {
            throw new Exception("not implemented");
        }

        public new Vect3 GetBoundingSphereCenter()
        {
            return Position + _model.GetBoundingSphere().Position;
        }

        public override double GetBoundingSphereRadius()
        {
            return _model.GetBoundingSphere().Radius;
        }
    }

    internal class CollisionData
    {
        internal CollisionDetails Details;
        internal Vect3 HitPointGlobal;
        internal Vect3 HitPointLocal;
    }
}