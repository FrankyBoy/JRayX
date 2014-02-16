using System.Collections.Generic;
using JRayXLib.Model;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Scenes
{
    public class MeshTest : Scene {

        private readonly Octree _tree;
    
        public MeshTest() {
            const double camheight = 50;
            Camera = new Camera(new Vect3(0, camheight*2, camheight), new Vect3(0, camheight*2-1, camheight-1), new Vect3(0, 0.707106781186547, -0.707106781186547), 1, 1, 1);
            //c = new Camera(new Vect3(-camheight, camheight*2, 0), new Vect3(-camheight+1, camheight*2-1, 0), new Vect3(0.707106781186547, 0.707106781186547, 0), 1, 1, 1);
            //c = new Camera(new Vect3(0, camheight*2, -camheight), new Vect3(0, camheight*2-1, -camheight+1), new Vect3(0, 0.707106781186547, 0.707106781186547), 1, 1, 1);
            //c = new Camera(new Vect3(0, camheight, -10), new Vect3(0, camheight-1, -10), new Vect3(0, 0, 1), 1, 1, 1);

            TriangleMeshModel model = BinarySTLParser.Parse("models/elka.stl");
            var objects = new List<Object3D>();
            
            const int nx = 2;
            const int ny = 4;
            for(int i=-nx;i<=nx;i++)
                for(int j=-ny;j<=ny;j++)
                    objects.Add(new ModelInstance(new Vect3(i*15+(j%2)*7,0,(j+4)*-15),model));
            //objects.add(new Plane(new Vect3(0,0,0),new Vect3(0,1,0),0xFFFFFFFF));

            objects.Add(Camera);

            Objects = objects.ToArray();
            _tree = Octree.BuildTree(new Vect3(0,0,0),Objects);
        }

        public override Sky GetSky() {
            return null;
        }
    
        public override Octree GetSceneTree(){
            return _tree;
        }
    }
}
