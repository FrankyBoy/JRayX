package jray.common;

import java.io.IOException;

import jray.math.Vect;

public class Sky extends Object3D {

    Texture texture;

    public Sky(String texture) throws IOException {
        super(null, null);
        this.texture = Texture.loadTexture(texture);
    }

    public Sky(int color) {
        super(null, null, 0);
        this.color = color;
    }

    @Override
    public double getHitPointDistance(Ray r) {
        return Double.POSITIVE_INFINITY;
    }

    @Override
    public int getColorAt(Vect3 hitPoint) {
        double[] hpdat = hitPoint.getData();

        double x = Math.acos(hpdat[1] / hitPoint.length()) / Math.PI;
        double y = Math.acos(hpdat[2] / hitPoint.length()) / Math.PI;

        return texture.getColorAt(x, y);
    }

    @Override
    public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
        Vect.invert(hitPoint, normal); // the normal is everywhere the vect pt -> 0
    }

    @Override
    public boolean contains(Vect3 hitPoint) {
        return false;
    }

    @Override
    public void rotate(Matrix4 rotationMatrix) {
    }
}
