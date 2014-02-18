using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using JRayXLib.Shapes;

namespace JRayXLib.Scene.Loaders
{
    public class BasicSceneLoader : ISceneLoader
    {
        private readonly string[] _searchPackages;
        private readonly string _filePath;

        public BasicSceneLoader(string filepath, string[] searchPaths = null)
        {
            _searchPackages = searchPaths ?? new[] {"JRayXLib.Shapes", "JRayXLib.Ray"};
            if(!File.Exists(filepath))
                throw new FileNotFoundException();

            _filePath = filepath;
        }

        public Scene LoadScene()
        {
            return null;

            var objects = new List<I3DObject>();
            var lines = File.ReadLines(_filePath);

            foreach (var line in lines)
            {
                var fragments = line.Split('#').Select(x => x.Trim()).ToArray();
                if(fragments[0].StartsWith("#") || string.IsNullOrEmpty(fragments[0]))
                    continue;

                fragments = fragments[0].Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                throw new NotImplementedException();
                //objects = GetInstanceFor(fragments[0], ) ???
            }
            /*
            return new Scene
                {
                    Objects = objects;
                };*/
        }

        public object[] ParseParameters(string[] data)
        {
            throw new NotImplementedException();
        }

        public I3DObject GetInstanceFor(string className, object[] parameters)
        {
            //Search for the class in listed packages
            foreach (string pckg in _searchPackages)
            {
                Type type = Type.GetType(pckg + "." + className);
                if (type != null)
                {
                    Activator.CreateInstance(type, parameters);
                    break;
                }
            }
            throw new Exception("Not found");
        }
    }
}