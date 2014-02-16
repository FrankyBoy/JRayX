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

            double[] maxData = max.Data;
            double[] minData = min.Data;

            foreach (MinimalTriangle m in _triangles)
            {
                Vect3 p = m.GetBoundingSphere().Position;
                double[] pData = p.Data;

                if (pData[0] > maxData[0]) maxData[0] = pData[0];
                if (pData[1] > maxData[1]) maxData[1] = pData[1];
                if (pData[2] > maxData[2]) maxData[2] = pData[2];
                if (pData[0] < minData[0]) minData[0] = pData[0];
                if (pData[1] < minData[1]) minData[1] = pData[1];
                if (pData[2] < minData[2]) minData[2] = pData[2];
            }

            maxData[0] = (maxData[0] + minData[0]) / 2;
            maxData[1] = (maxData[1] + minData[1]) / 2;
            maxData[2] = (maxData[2] + minData[2]) / 2;

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
