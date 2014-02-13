package jray.math;

import jray.common.*;

public class VectMatrix {

    public static void multiply(Matrix4 m, Vect3 v, Vect3 erg) {
        double[][] md = m.getData();
        double[] vd = v.getData();
        double[] ed = erg.getData();

        double fourth = md[3][0] * vd[0] + md[3][1] * vd[1] + md[3][2] * vd[2] + md[3][3];

        if (fourth != 0) {
            double x = (md[0][0] * vd[0] + md[0][1] * vd[1] + md[0][2] * vd[2] + md[0][3]) / fourth;
            double y = (md[1][0] * vd[0] + md[1][1] * vd[1] + md[1][2] * vd[2] + md[1][3]) / fourth;
            double z = (md[2][0] * vd[0] + md[2][1] * vd[1] + md[2][2] * vd[2] + md[2][3]) / fourth;

            ed[0] = x;
            ed[1] = y;
            ed[2] = z;
        }
    }

    public static void multiply(Vect3 v, Matrix4 m, Vect3 erg) {
        double[] vd = v.getData();
        double[][] md = m.getData();
        double[] ed = erg.getData();

        double fourth = md[0][3] * vd[0] + md[1][3] * vd[1] + md[2][3] * vd[2] + md[3][3];

        if(fourth != 0) {
            double x = (md[0][0] * vd[0] + md[1][0] * vd[1] + md[2][0] * vd[2] + md[3][0]) / fourth;
            double y = (md[0][1] * vd[0] + md[1][1] * vd[1] + md[2][1] * vd[2] + md[3][1]) / fourth;
            double z = (md[0][2] * vd[0] + md[1][2] * vd[1] + md[2][2] * vd[2] + md[3][2]) / fourth;

            ed[0] = x;
            ed[1] = y;
            ed[2] = z;
        }
    }
}
