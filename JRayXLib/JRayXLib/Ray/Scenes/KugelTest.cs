using System.Collections.Generic;
using JRayXLib.Colors;
using JRayXLib.Scene;
using JRayXLib.Shapes;

namespace JRayXLib.Ray.Scenes
{
    public class KugelTest : BasicScene
    {
        public KugelTest()
        {
            var objects = new List<I3DObject>
                {
                    //new Sphere(new Vect3(0, 0, 0), 1, Color.Yellow, 0.5),
                    new Plane(new Vect3{Y = -2}, new Vect3{Y = 1}, Color.Green),
                    new Cone(new Vect3(), new Vect3{Y = 1}, 0, Color.Red)
                };

            UpdateObjects(objects);
            Camera = new Camera(new Vect3 { Z = -10 }, new Vect3 { Z = -8 }, new Vect3 { Y = 1 });
        }
    }
}