using System.Collections.Generic;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Scene
{
    public class BasicScene : Scene
    {
        private I3DObject[] _objects;

        public override CollisionDetails FindNearestHit(Shapes.Ray ray)
        {
            var c = new CollisionDetails(ray);

            //Find nearest Hit
            foreach (I3DObject objectCandidate in _objects)
            {
                double distanceCandidate = objectCandidate.GetHitPointDistance(ray);
                if (distanceCandidate > 0 && distanceCandidate < c.Distance)
                {
                    c.Obj = objectCandidate;
                    c.Distance = distanceCandidate;
                }
            }

            return c;
        }

        public override void UpdateObjects(List<I3DObject> objects)
        {
            _objects = objects.ToArray();
        }
    }
}
