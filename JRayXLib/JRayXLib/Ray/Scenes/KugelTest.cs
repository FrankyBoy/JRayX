using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray.Scenes
{
    public class KugelTest : Scene {

        public KugelTest() {
            var objects = new List<I3DObject>
                {
                    //new Sphere(new Vect3(0, 0, 0), 1, Color.Yellow, 0.5),
                    new Plane(new Vect3(0, -2, 0), new Vect3(0, 1, 0), Color.Green),
                    new Cone(new Vect3(0,0,0), new Vect3(0,1,0), 0, Color.Red)
                };
            Objects = objects.ToArray();

            Camera = new Camera(new Vect3(0,0,-10),new Vect3(0,0,-8),new Vect3(0,1,0));
        }
	

        public override Sky GetSky() {
            return null;
        }

        public override Octree GetSceneTree()
        {
            return null;
        }
    }
}
