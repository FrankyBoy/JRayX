package jray.common;

import jray.math.Constants;

/**
 * Structure containing 2 double - used for texture-coordinates
 */
public class Vect2 {

    private double[] data;

    public Vect2(double a, double b) {
        this(new double[]{a, b});
    }

    public Vect2(double[] data) {
        if (data.length == 2) {
            this.data = data;
        } else {
            throw new InstantiationError("data.length muss 2 sein");
        }
    }

    public Vect2(Vect2 old) {
        double[] dat = old.getData();
        this.data = new double[]{dat[0], dat[1]};
    }

    public Vect2() {
        this(new double[2]);
    }

    public double[] getData() {
        return data;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder("(");

        for (double d : data) {
            sb.append(d + " ");
        }
        sb.append(")");
        return sb.toString();
    }

    @Override
    public boolean equals(Object o) {
        if (!(o instanceof Vect2)) {
            return false;
        }

        for (int i = 0; i < 2; i++) {
            if (Math.abs(data[i] - ((Vect2) o).getData()[i]) > Constants.EPS) {
                return false;
            }
        }

        return true;
    }

    public double length() {
        return Math.sqrt(data[0] * data[0] + data[1] * data[1]);
    }

    public double quadLength() {
        return data[0] * data[0] + data[1] * data[1];
    }

    public void normalize() {
        double len = length();
        data[0] /= len;
        data[1] /= len;
    }

    public boolean sameDirection(Vect2 v2) {
        double d = data[0] / v2.data[0]; // scale-factor

        return data[1] / v2.data[1] == d;
    }

    public double get(int i) {
        return data[i];
    }

    public void copyDataTo(Vect2 v) {
        double[] vd = v.getData();
        vd[0] = data[0];
        vd[1] = data[1];
    }

    public static Vect2 parseVect2(String s) {
        s = s.trim();
        // TODO: we can do this with a repetition group surely (X{n}), but I'm not sure about the whitespaces
        if (s.matches("\\[\\s*[+-]?[0-9]+(\\.[0-9]+)?\\s+[+-]?[0-9]+(\\.[0-9]+)?\\s*\\]")) {
            s = s.replaceAll("[\\[\\]]", "").trim(); // kill the braces
            String[] field = s.split("\\s+");
            return new Vect2(Double.parseDouble(field[0]),
                    Double.parseDouble(field[1]));
        } else {
            throw new IllegalArgumentException("String " + s + " has wrong format");
        }
    }
}
