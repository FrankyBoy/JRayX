using System;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray
{
    public abstract class Scene
    {
        protected string Name;

        protected Scene()
        {
            Name = GetType().Name;
            LightDirection = new Vect3(0, -1, .5).Normalize();
        }

        public I3DObject[] Objects { get; set; }
        public Camera Camera { get; set; }
        public Vect3 LightDirection { get; set; }

        public abstract Sky GetSky();
        public abstract Octree GetSceneTree();

        public String GetName()
        {
            return Name;
        }
    }
}