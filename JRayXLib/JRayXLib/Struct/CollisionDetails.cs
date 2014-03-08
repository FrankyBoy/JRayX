using System.Collections.Generic;
using JRayXLib.Math;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public class CollisionDetails
    {
        public double Distance;
        public I3DObject Obj;
        public Shapes.Ray Ray;

        public CollisionDetails(Shapes.Ray ray)
        {
            Obj = null;
            Distance = double.PositiveInfinity;
            Ray = ray;
        }

        public void CheckCollisionSet(List<I3DObject> objects)
        {
            foreach (I3DObject candidate in objects)
            {
                double distanceCandidate = candidate.GetHitPointDistance(Ray);
                if (distanceCandidate > Constants.EPS && distanceCandidate < Distance)
                {
                    Obj = candidate;
                    Distance = distanceCandidate;
                }
            }
        }
    }
}