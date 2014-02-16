using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Model
{
    public class TriangleMeshModel
    {
        private readonly I3DObject[] _triangles;
        private readonly Sphere _bounds;
        private readonly Octree _tree;

        /**
     * 3xnormal
     * 3x v1
     * 3x v2
     * 3x v3
     * 
     * @param triangleEdgeData
     */
        public TriangleMeshModel(I3DObject[] triangleEdgeData)
        {
            _triangles = triangleEdgeData;

            var max = new Vect3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
            var min = new Vect3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

            foreach (MinimalTriangle m in _triangles)
            {
                Vect3 p = m.GetBoundingSphere().Position;

                if (p[0] > max[0]) max[0] = p[0];
                if (p[1] > max[1]) max[1] = p[1];
                if (p[2] > max[2]) max[2] = p[2];
                if (p[0] < min[0]) min[0] = p[0];
                if (p[1] < min[1]) min[1] = p[1];
                if (p[2] < min[2]) min[2] = p[2];
            }

            max[0] = (max[0] + min[0]) / 2;
            max[1] = (max[1] + min[1]) / 2;
            max[2] = (max[2] + min[2]) / 2;

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
                m.purge();
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
