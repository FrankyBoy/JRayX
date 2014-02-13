package jray.common;

import java.util.Arrays;

import jray.math.*;

public class Object3DStructure extends Object3D {

    private Object3D[] objects;

    public Object3DStructure(Vect3 position, Vect3 lookAt, double rotationRad, Object3D[] objects) {
        super(new Vect3(), lookAt);
        this.objects = objects;
        this.setPosition(position);
    }

    // TODO: can we optimize this?
    @Override
    public double getHitPointDistance(Ray r) {
        double dist_min = Double.POSITIVE_INFINITY;
        for (Object3D o3d : objects) {
            double dist = o3d.getHitPointDistance(r);
            if (dist < dist_min) {
                dist_min = dist;
            }
        }

        return dist_min;
    }

    protected Object3D getObjectAt(Vect3 hitPoint) {
        for (Object3D o3d : objects) {
            if (o3d.contains(hitPoint)) {
                return o3d;
            }
        }
        return null;
    }

    @Override
    public int getColorAt(Vect3 hitPoint) {
        Object3D ret = getObjectAt(hitPoint);
        if (ret != null) {
            return ret.getColorAt(hitPoint);
        } else {
            throw new RuntimeException("internal error!");
        }
    }

    @Override
    public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
        Object3D ret = getObjectAt(hitPoint);
        if (ret != null) {
            ret.getNormalAt(hitPoint, normal);
        } else {
            throw new RuntimeException("internal error!");
        }
    }

    @Override
    public boolean contains(Vect3 hitPoint) {
        for (Object3D o3d : objects) {
            if (o3d.contains(hitPoint)) {
                return true;
            }
        }
        return false;
    }

    @Override
    public void setPosition(Vect3 position) {
        for (Object3D o3d : objects) {
            Vect3 objPos = o3d.getPosition();
            Vect.subtract(objPos, this.position, objPos);
            Vect.add(objPos, position, objPos);
        }

        this.position = position;
    }

    @Override
    public void rotate(Matrix4 rotationMatrix) {
        for (Object3D o3d : objects) {
            Vect3 objPos = o3d.getPosition();

            // TODO: we have a matrix to combine these three, right?
            Vect.subtract(objPos, position, objPos);
            VectMatrix.multiply(objPos, rotationMatrix, objPos);
            Vect.add(objPos, position, objPos);

            o3d.rotate(rotationMatrix);
        }
    }

    @Override
    public double getReflectivityAt(Vect3 hitPoint) {
        Object3D ret = getObjectAt(hitPoint);
        if (ret != null) {
            return ret.getReflectivityAt(hitPoint);
        } else {
            throw new RuntimeException("internal error!");
        }
    }

    public String toString() {
        return super.toString() + " - " + Arrays.toString(objects);
    }
}
