using System;
using System.Collections.Generic;
using JRayXLib.Shapes;

namespace JRayXLib.Ray
{
    public class Object3DFactory
    {

        private static readonly List<string> SearchPackages = new List<string>();

        static Object3DFactory() {
            SearchPackages.Add("JRayXLib.Shapes");
            SearchPackages.Add("JRayXLib.Ray");
        }

        public static Object3D GetConstructorFor(string className, object[] parameterTypes)
        {

            //Search for the class in listed packages
            foreach (string pckg in SearchPackages)
            {
                var type = Type.GetType(pckg + "." + className);
                if (type != null)
                {
                    Activator.CreateInstance(type, parameterTypes);
                    break;
                }
            }
            throw new Exception("Not found");
        }
    }
}
