using System.Collections.Generic;
using JRayXLib.Math;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public struct CollisionDetails
    {
        public double Distance { get; set; }
        public I3DObject Obj { get; set; }

        public void CheckCollisionSet(List<I3DObject> objects, Shapes.Ray ray)
        {
            foreach (I3DObject candidate in objects)
            {
                double distanceCandidate = candidate.GetHitPointDistance(ray);
                if (distanceCandidate > Constants.EPS && distanceCandidate < Distance)
                {
                    Obj = candidate;
                    Distance = distanceCandidate;
                }
            }
        }
    }
}