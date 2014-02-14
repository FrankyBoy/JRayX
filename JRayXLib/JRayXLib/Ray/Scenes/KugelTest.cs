using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Shapes;

namespace JRayXLib.Ray.Scenes
{
    public class KugelTest : Scene {

        private List<Object3D> objects;
        private Camera cam;
	
        public KugelTest(){
            objects = new List<Object3D>();
            var color = Color.White;
            color.B = 0;
            objects.Add(new Sphere(new Vect3(0,0,0),1,color));
            cam = new Camera(new Vect3(0,0,-5),new Vect3(0,0,-3),new Vect3(0,1,0));
        }
	
        public override Camera GetCamera() {
            return cam;
        }

        public override Sky GetSky() {
            return null;
        }

	
        public override List<Object3D> GetObjects() {
            return objects;
        }

    }
}
