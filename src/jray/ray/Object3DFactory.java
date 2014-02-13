package jray.ray;

import java.lang.reflect.Constructor;
import java.util.Arrays;
import java.util.List;
import java.util.Vector;

public class Object3DFactory {

    private static List<String> searchPackages = new Vector<String>();

    static {
        searchPackages.add("org.cs.jray.common");
        searchPackages.add("org.cs.jray.ray");
    }

    public static Constructor<?> getConstructorFor(String className, Class<?>... parameterTypes) {
        try {
            //Search for the class in listed packages
            Class<?> clasz = null;
            for (String pckg : searchPackages) {
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

            throw new RuntimeException("Constructor for " + className + "(" + Arrays.toString(parameterTypes) + ") not found!");
        } catch (Exception e) {
            throw new RuntimeException("Constructor for " + className + "(" + Arrays.toString(parameterTypes) + ") not found!", e);
        }
    }
}
