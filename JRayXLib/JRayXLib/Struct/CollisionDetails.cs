using System.Collections.Generic;
using JRayXLib.Math;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    public class CollisionDetails
    {
        public int Checks = 0;
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
                Checks++;
                double distanceCandidate = candidate.GetHitPointDistance(Ray);
                if (distanceCandidate > Constants.MinDistance && distanceCandidate < Distance)
                {
                    Obj = candidate;
                    Distance = distanceCandidate;
                }
            }
        }

        public bool HasHit()
        {
            return Obj != null;
        }

        public int GetCheckCount()
        {
            return Checks;
        }
    }
}