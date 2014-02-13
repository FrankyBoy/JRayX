package jray.common;

import jray.math.Vect;
import jray.math.intersections.IntColors;


public class TexturedTriangle extends Triangle {

    /**
     * stores the texture
     */
    private Texture texture;
    /**
     * stores the texture coordinates
     */
    private Vect2 t1, t2, t3;

    public TexturedTriangle(Vect3 v1, Vect2 t1, Vect3 v2, Vect2 t2, Vect3 v3, Vect2 t3, String imagePath) {
        super(v1, v2, v3, 0xFF00FF00);
        this.t1 = t1;
        this.t2 = t2;
        this.t3 = t3;
        
        texture = Texture.loadTexture(imagePath);
    }

    /**
     * gets the color of the mapped texture at the given point
     * @param hitPoint a point on the surface of this triangle
     * @return the color of the mapped texture at the given point
     */
    @Override
    public int getColorAt(Vect3 hitPoint) {
        return IntColors.mixSurfaceSurface(getTextureColorAt(hitPoint), color);
    }

    @Override
    public double getReflectivityAt(Vect3 hitPoint) {
        double alpha = ((double)((getTextureColorAt(hitPoint) >>> 24) & 0xFF)) / 256;
        return (1 - alpha) * reflectivity;
    }

    private int getTextureColorAt(Vect3 hitPoint) {
        Vect2 texcoord = new Vect2();
        Vect.interpolateTriangle(position, v2, v3, t1, t2, t3, hitPoint, texcoord);
        return texture.getColorAt(texcoord);
    }
}
