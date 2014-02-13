package jray.common;

import jray.math.Constants;

public class Matrix4 {

    private double[][] data;

    /**
     * Copyconstructor - all fields are copied
     * 
     * @param m the new matrix will be equal to m
     */
    public Matrix4(Matrix4 m) {
        this();
        double[][] dm = m.getData();

        data[0][0] = dm[0][0];
        data[1][0] = dm[1][0];
        data[2][0] = dm[2][0];
        data[3][0] = dm[3][0];
        data[0][1] = dm[0][1];
        data[1][1] = dm[1][1];
        data[2][1] = dm[2][1];
        data[3][1] = dm[3][1];
        data[0][2] = dm[0][2];
        data[1][2] = dm[1][2];
        data[2][2] = dm[2][2];
        data[3][2] = dm[3][2];
        data[0][3] = dm[0][3];
        data[1][3] = dm[1][3];
        data[2][3] = dm[2][3];
        data[3][3] = dm[3][3];
    }

    public Matrix4() {
        this.data = new double[4][4];
    }

    /**
     * @return the data
     */
    public double[][] getData() {
        return data;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder("(");

        for (double[] da : data) {
            for (double d : da) {
                sb.append(d + " ");
            }
            sb.append("\n ");
        }
        sb.append(")");

        return sb.toString();
    }

    @Override
    public boolean equals(Object o) {
        if (!(o instanceof Matrix4)) {
            return false;
        }

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (Math.abs(data[i][j] - ((Matrix4) o).getData()[i][j]) > Constants.EPS) {
                    return false;
                }
            }
        }

        return true;
    }
}
