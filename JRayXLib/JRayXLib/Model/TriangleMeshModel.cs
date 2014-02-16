using JRayXLib.Colors;
using JRayXLib.Math;
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

                if (p.Data[0] > max.Data[0]) max.Data[0] = p.Data[0];
                if (p.Data[1] > max.Data[1]) max.Data[1] = p.Data[1];
                if (p.Data[2] > max.Data[2]) max.Data[2] = p.Data[2];
                if (p.Data[0] < min.Data[0]) min.Data[0] = p.Data[0];
                if (p.Data[1] < min.Data[1]) min.Data[1] = p.Data[1];
                if (p.Data[2] < min.Data[2]) min.Data[2] = p.Data[2];
            }

            max.Data[0] = (max.Data[0] + min.Data[0]) / 2;
            max.Data[1] = (max.Data[1] + min.Data[1]) / 2;
            max.Data[2] = (max.Data[2] + min.Data[2]) / 2;

            double radius = 0;
            foreach (MinimalTriangle m in _triangles)
            {
                Vect3 p = m.GetBoundingSphere().Position;
                Vect.Subtract(p, max, ref min);
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
