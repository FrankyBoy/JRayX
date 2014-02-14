using System;
using System.Collections.Generic;

namespace JRayXLib.Ray
{
    public class Object3DFactory
    {

        private static List<string> searchPackages = new List<string>();

        static Object3DFactory {
            searchPackages.Add("org.cs.jray.common");
            searchPackages.Add("org.cs.jray.ray");
        }

        public static Constructor<?> getConstructorFor(string className, Class<?>... parameterTypes) {
            try {
                //Search for the class in listed packages
                Class<?> clasz = null;
                for (string pckg : searchPackages) {
                    try {
                        clasz = Class.forName(pckg + "." + className);
                        break;
                    } catch (Exception e) {
                    }
                }

                if (clasz == null) {
                    throw new ClassNotFoundException(className);
                }

                //Search for the constructor in clasz
                csearch:
                for (Constructor<?> c : clasz.getConstructors()) {

                    if (parameterTypes.length == c.getParameterTypes().length) {
                        for (int i = 0; i < c.getParameterTypes().length; i++) {
                            if (!(c.getParameterTypes()[i].isAssignableFrom(parameterTypes[i]) ||
                                  (c.getParameterTypes()[i].equals(double.class) && parameterTypes[i].equals(int.class)))) //int is assignable to double without cast!
                            {
                                continue csearch;
                            }
                        }

                        return c;
                    }
                }

                throw new RuntimeException("Constructor for " + className + "(" + Arrays.tostring(parameterTypes) + ") not found!");
            } catch (Exception e) {
                throw new RuntimeException("Constructor for " + className + "(" + Arrays.tostring(parameterTypes) + ") not found!", e);
            }
        }
    }
}
