using System.Collections.Generic;
using System.Linq;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Scene
{
    public class OctreeScene : Scene
    {
        protected Octree2 Tree;
        
        public override void UpdateObjects(List<I3DObject> objects)
        {
            // finding actual bounds for the Octree...

            var spheres = objects.Select(x => x.GetBoundingSphere()).ToArray();

            var min = new Vect3
                {
                    X = spheres.Min(x => x.Position.X - x.Radius),
                    Y = spheres.Min(x => x.Position.Y - x.Radius),
                    Z = spheres.Min(x => x.Position.Z - x.Radius)
                };

            var max = new Vect3
                {
                    X = spheres.Max(x => x.Position.X + x.Radius),
                    Y = spheres.Max(x => x.Position.Y + x.Radius),
                    Z = spheres.Max(x => x.Position.Z + x.Radius)
                };

            var center = (min + max)/2;
            var tmp = max - center;
            var halfSize = System.Math.Max(tmp.X, System.Math.Max(tmp.Y, tmp.Z));

            Tree = new Octree2(center, halfSize);
            Tree.Insert(objects);
        }
        
        public override CollisionDetails FindNearestHit(Shapes.Ray ray)
        {
            return Tree.GetFirstCollision(ray);
        }
    }
}