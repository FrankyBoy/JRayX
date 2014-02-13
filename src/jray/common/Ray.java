package jray.common;

public class Ray {

    private Vect3 origin, direction;

    public Ray() {
        this(new Vect3(), new Vect3());
    }

    /**
     * Creates a new Ray
     * 
     * @param origin of the ray
     * @param direction of the ray - MUST be normed to length 1 
     */
    public Ray(Vect3 origin, Vect3 direction) {
        this.origin = origin;
        this.direction = direction;
    }

    public Vect3 getDirection() {
        return direction;
    }

    public Vect3 getOrigin() {
        return origin;
    }

    @Override
    public String toString() {
        return "Ray origin=" + origin + " dir=" + direction;
    }
}
