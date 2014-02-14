using System;
using System.Collections.Generic;
using JRayXLib.Shapes;
using JRayXLib.Struct;

namespace JRayXLib.Ray
{
    abstract public class Scene
    {

        protected string Name;

        public Scene()
        {
            Name = GetType().Name;
        }

        public abstract Camera GetCamera();
        public abstract Sky GetSky();

        public abstract List<Object3D> GetObjects();

        public String GetName() {
            return Name;
        }
    
        public Octree GetSceneTree(){
            return null;
        }
    }
}
