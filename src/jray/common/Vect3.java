package jray.common;

import jray.math.Constants;

public class Vect3 {

    public double[] data;

    public Vect3(double a, double b, double c) {
        this(new double[]{a, b, c});
    }

    public Vect3(double[] data) {
        if (data.length == 3) {
            this.data = data;
        } else {
            throw new InstantiationError("data.length muss 3 sein");
        }
    }

    public Vect3(Vect3 old) {
        double[] dat = old.getData();
        this.data = new double[]{dat[0], dat[1], dat[2]};
    }

    public Vect3() {
        this(new double[3]);
    }

    public double[] getData() {
        return data;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder("(");

        for (double d : data) {
            sb.append(String.format("%.2f",d) + " ");
        }
        sb.append(")");
        return sb.toString();
    }

    public boolean equals(Vect3 v, double eps){
    	return Math.abs(data[0]-v.data[0])<eps && Math.abs(data[1]-v.data[1])<eps && Math.abs(data[2]-v.data[2])<eps;
    }
    
    @Override
    public boolean equals(Object o) {
        if (!(o instanceof Vect3)) {
            return false;
        }

        for (int i = 0; i < 3; i++) {
            if (Math.abs(data[i] - ((Vect3) o).getData()[i]) > Constants.EPS) {
                return false;
            }
        }

        return true;
    }

    public double length() {
        return Math.sqrt(data[0] * data[0] + data[1] * data[1] + data[2] * data[2]);
    }

    public double quadLength() {
        return data[0] * data[0] + data[1] * data[1] + data[2] * data[2];
    }

    public void normalize() {
        double len = length();
        data[0] /= len;
        data[1] /= len;
        data[2] /= len;
    }

    public boolean sameDirection(Vect3 v2) {
        double d = data[0] / v2.data[0]; // scale-factor

        return data[1] / v2.data[1] == d &&
                data[2] / v2.data[2] == d;
    }

    public double get(int i) {
        return data[i];
    }

    public void copyDataTo(Vect3 v) {
        double[] vd = v.getData();
        vd[0] = data[0];
        vd[1] = data[1];
        vd[2] = data[2];
    }

    public static Vect3 parseVect3(String s) {
        s = s.trim();
        // TODO: we can do this with a repetition group surely (X{n}), but I'm not sure about the whitespaces
        if (s.matches("\\[\\s*[+-]?[0-9]+(\\.[0-9]+)?\\s+[+-]?[0-9]+(\\.[0-9]+)?\\s+[+-]?[0-9]+(\\.[0-9]+)?\\s*\\]")) {
            s = s.replaceAll("[\\[\\]]", "").trim(); // kill the braces
            String[] field = s.split("\\s+");
            return new Vect3(Double.parseDouble(field[0]),
                    Double.parseDouble(field[1]),
                    Double.parseDouble(field[2]));
        } else {
            throw new IllegalArgumentException("String " + s + " has wrong format");
        }
    }
}
