using System.Collections.Generic;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Scene
{
    public abstract class Scene
    {
        public string Name { protected set; get; }
        public Camera Camera { get; set; }
        public Sky Sky { get; set; }
        public Vect3 AmbientLightDirection { get; set; }
        
        protected Scene()
        {
            Name = GetType().Name;
            AmbientLightDirection = new Vect3{ X = 0, Y = -1, Z = .5}.Normalize();
        }

        public abstract CollisionDetails FindNearestHit(Shapes.Ray ray);
        public abstract void UpdateObjects(List<I3DObject> dObjects);
    }
}