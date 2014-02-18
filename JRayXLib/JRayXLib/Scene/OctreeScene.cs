using System.Collections.Generic;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Scene
{
    public class OctreeScene : Scene
    {
        protected Octree Tree;
        
        public override void UpdateObjects(List<I3DObject> objects)
        {
            Tree = Octree.BuildTree(new Vect3 { X = Camera.ViewPaneHeightVector.Length() * 1.5, Y = -3.1 }, objects);
        }
        
        public override CollisionDetails FindNearestHit(Shapes.Ray ray)
        {
            return RayPath.GetFirstCollision(Tree, ray);
        }
    }
}