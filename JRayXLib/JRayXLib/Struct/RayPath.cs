using System;
using JRayXLib.Math;
using JRayXLib.Math.intersections;

/**
 * Contains the logic to determine the first intersection of a ray traveling
 * through objects stored in an octree. The ray's origin must be enclosed by the
 * octree.
 */

namespace JRayXLib.Struct
{
    public class RayPath
    {
        public static CollisionDetails GetFirstCollision(Octree tree, Shapes.Ray r)
        {
            var c = new CollisionDetails(r);

            double distanceTravelled = 0; //distance traveled since the ray's origin

            //Algorithm starts at the ray's origin and in the root node.
            
            Node n = tree.GetRoot();
            c.CheckCollisionSet(n.Content);

            var pos = r.Origin;
            if (!n.Encloses(pos))
                throw new Exception("Ray's origin is not located in the octree!");

            do
            {
                //March to the leaf containing "pos" and check collisions
                n = n.MarchToCheckingCollisions(pos, c);

                //No leaf is containing "pos" -> left tree -> stop searching
                if (n == null)
                    break;

                //march out of current node
                double d = RayCube.GetDistanceToBorderPlane(pos, r.Direction, n.Center, n.Width / 2) + Constants.EPS * 1e4;
                    //distance traveled since the ray's origin
                pos = pos + r.Direction * d;
                distanceTravelled += d;

                //in case eps was not enough
                while (n.Encloses(pos))
                {
                    d = Constants.EPS*1e4;
                    pos = pos + r.Direction*d;
                    distanceTravelled += d;
                }

                /**
             * When passing a box-border, all intersection tests between the ray's origin and the
             * box-border-intersection-point must have taken place (and some beyond the box-border too).
             * Therefore, if an intersection located between the ray's origin and the box-border has been found, this
             * must be the nearest intersection and the search may stop.
             */
            } while (c.Distance > distanceTravelled);

            return c;
        }
    }
}