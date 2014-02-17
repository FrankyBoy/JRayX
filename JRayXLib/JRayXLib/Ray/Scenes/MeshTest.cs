using System.Collections.Generic;
using JRayXLib.Model;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Scenes
{
    public class MeshTest : Scene
    {
        private readonly Octree _tree;

        public MeshTest()
        {
            const double camheight = 50;
            Camera = new Camera(
                new Vect3
                {
                    Y = camheight * 2,
                    Z = camheight
                },

                    new Vect3
                {
                    Y = camheight * 2 - 1,
                    Z = camheight - 1
                },
                new Vect3
                {
                    Y = 0.707106781186547,
                    Z = -0.707106781186547
                }, 1, 1, 1);


            TriangleMeshModel model = BinarySTLParser.Parse("models/elka.stl");
            var objects = new List<I3DObject>();

            const int nx = 2;
            const int ny = 4;
            for (int i = -nx; i <= nx; i++)
                for (int j = -ny; j <= ny; j++)
                    objects.Add(new ModelInstance(new Vect3 { X = i * 15 + (j % 2) * 7, Z = (j + 4) * -15 }, model));
            //objects.add(new Plane(new Vect3(0,0,0),new Vect3(0,1,0),0xFFFFFFFF));

            objects.Add(Camera);

            Objects = objects.ToArray();
            _tree = Octree.BuildTree(new Vect3(), Objects);
        }

        public override Sky GetSky()
        {
            return null;
        }

        public override Octree GetSceneTree()
        {
            return _tree;
        }
    }
}