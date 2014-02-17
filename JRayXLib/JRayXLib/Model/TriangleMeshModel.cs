using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Model
{
    public class TriangleMeshModel
    {
        private readonly Sphere _bounds;
        private readonly Octree _tree;
        private readonly I3DObject[] _triangles;

        public TriangleMeshModel(List<I3DObject> triangleEdgeData)
        {
            _triangles = triangleEdgeData.ToArray();

            var max = new Vect3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
            var min = new Vect3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

            foreach (MinimalTriangle m in _triangles)
            {
                Vect3 p = m.GetBoundingSphere().Position;

                if (p.X > max.X) max.X = p.X;
                if (p.Y > max.Y) max.Y = p.Y;
                if (p.Z > max.Z) max.Z = p.Z;
                if (p.X < min.X) min.X = p.X;
                if (p.Y < min.Y) min.Y = p.Y;
                if (p.Z < min.Z) min.Z = p.Z;
            }

            max = (max + min)/2;

            double radius = 0;
            foreach (MinimalTriangle m in _triangles)
            {
                Vect3 p = m.GetBoundingSphere().Position;
                min = p - max;
                double dist = m.GetBoundingSphereRadius() + min.Length();

                if (dist > radius)
                {
                    radius = dist;
                }
            }

            _bounds = new Sphere(max, radius, Color.Black);

            _tree = Octree.BuildTree(_bounds.Position, _triangles);

            foreach (MinimalTriangle m in _triangles)
                m.Purge();
        }


        public Sphere GetBoundingSphere()
        {
            return _bounds;
        }

        public Octree GetTree()
        {
            return _tree;
        }
    }
}