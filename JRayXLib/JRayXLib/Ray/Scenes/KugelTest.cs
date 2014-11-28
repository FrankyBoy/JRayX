using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Scene;
using JRayXLib.Shapes;

namespace JRayXLib.Ray.Scenes
{
    public class KugelTest : OctreeScene
    {
        public KugelTest()
        {
            var objects = new List<I3DObject>
                {
                    //new Sphere(new Vect3(), 1, Color.Yellow, 0.5),
                    //new Plane(new Vect3{Y = -2}, new Vect3{Y = 1}, Color.Green),
                    new Cone(new Vect3{Y = 1}, new Vect3{X=-1, Y=-1, Z=-1}, 32, Color.Green),
                    new Sphere(new Vect3{X = 1, Z = 2}, 1, Color.Blue)
                };

            UpdateObjects(objects);
            Camera = new Camera(new Vect3 { Z = -10 }, new Vect3 { Z = -8 }, new Vect3 { Y = 1 });
        }
    }
}