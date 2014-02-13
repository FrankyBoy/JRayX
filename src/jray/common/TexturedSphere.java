package jray.common;

import jray.math.Vect;
import jray.math.intersections.IntColors;


public class TexturedSphere extends Sphere {

    private final Texture texture;

    public TexturedSphere(Vect3 position, double radius, double rotation, String imagePath) {
        this(position, radius, rotation, 0, 0, imagePath);
    }

    public TexturedSphere(Vect3 position, double radius, double rotation, int color, double reflectivity, String imagePath) {
        this(position, new Vect3(0, radius, 0), rotation, color, reflectivity, imagePath);
    }

    public TexturedSphere(Vect3 position, Vect3 lookAt, double rotation, String imagePath) {
        this(position, lookAt, rotation, 0, 0, imagePath);
    }

    public TexturedSphere(Vect3 position, Vect3 lookAt, double rotation, int color, double reflectivity, String imagePath) {
        super(position, lookAt, rotation, color, reflectivity);
        texture = Texture.loadTexture(imagePath);
    }

    @Override
    public int getColorAt(Vect3 hitPoint) {
        return IntColors.mixSurfaceSurface(getTextureColorAt(hitPoint), color);
    }

    @Override
    public double getReflectivityAt(Vect3 hitPoint) {
        double alpha = ((double) ((getTextureColorAt(hitPoint) >>> 24) & 0xFF)) / 256;
        return (1 - alpha) * reflectivity;
    }

    private int getTextureColorAt(Vect3 hitPoint) {
        // calculate x (longitude)
        Vect3 tmp = new Vect3();
        Vect.subtract(hitPoint, position, tmp);
        tmp.normalize();
        double y = Math.acos(Vect.dotProduct(tmp, lookAt)) / Math.PI;

        // project to equator plane
        double dist = - Vect.dotProduct(tmp, lookAt);
        Vect.addMultiple(tmp, lookAt, dist, tmp);
        tmp.normalize();

        double x = Math.acos(Vect.dotProduct(tmp, rotVect)) / (2 * Math.PI);
        Vect.crossProduct(tmp, rotVect, tmp);
        if(Vect.dotProduct(tmp, lookAt) < 0) {
            x = 0.5 + x;
        } else {
            x = 0.5 - x;
        }

        return texture.getColorAt(x, y);
    }
}
