package jray.math.intersections;

public class IntColors {

    public static final int
            A = 0xFF000000,
            R = 0x00FF0000,
            G = 0x0000FF00,
            B = 0x000000FF,
            AO = 24, RO = 16, GO = 8, BO = 0, MASK = 0xFF;

    public static long toLong(int i) {
        long a, r, g, b;
        a = (i >>> AO) & MASK;
        r = (i >>> RO) & MASK;
        g = (i >>> GO) & MASK;
        b = (i >>> BO) & MASK;

        return (a << LongColors.AO) | (r << LongColors.RO) | (g << LongColors.GO) | (b << LongColors.BO);
    }

    public static int mixLightLight(int light1, int light2) {
        int r, g, b;

        r = ((light1 >>> RO) & MASK) + ((light2 >>> RO) & MASK);
        g = ((light1 >>> GO) & MASK) + ((light2 >>> GO) & MASK);
        b = ((light1 >>> BO) & MASK) + ((light2 >>> BO) & MASK);

        return A | (r > MASK ? R : r << RO) | (g > MASK ? G : g << GO) | (b > MASK ? B : b << BO);
    }

    /**
     * mix two surface colors, eg. for transparent texture overlays
     * @param topColor
     * @param bottomColor
     * @return
     */
    public static int mixSurfaceSurface(int topColor, int bottomColor) {
        int a, r, g, b;

        // the two most common cases
        a = (char) ((topColor >>> AO) & MASK);
        if (a == MASK) {
            return topColor;
        }
        if (a == 0x00) {
            return bottomColor;
        }

        double alpha = ((double) a) / 256;

        a = (int) (((topColor >>> AO) & MASK) +
                ((bottomColor >>> AO) & MASK) * (1 - alpha));
        r = (int) (((topColor >>> RO) & MASK) * alpha +
                ((bottomColor >>> RO) & MASK) * (1 - alpha));
        g = (int) (((topColor >>> GO) & MASK) * alpha +
                ((bottomColor >>> GO) & MASK) * (1 - alpha));
        b = (int) (((topColor >>> BO) & MASK) * alpha +
                ((bottomColor >>> BO) & MASK) * (1 - alpha));

        return a << AO | r << RO | g << GO | b << BO;
    }

    /**
     * mix a surface color with a light color
     * @param surfaceColor
     * @param lightColor 
     * @return
     */
    public static int mixSurfaceLight(int surfaceColor, int lightColor) {
        int r, g, b;
        r = ((surfaceColor >>> RO) & MASK) * ((lightColor >>> RO) & MASK) / 256;
        g = ((surfaceColor >>> GO) & MASK) * ((lightColor >>> GO) & MASK) / 256;
        b = ((surfaceColor >>> BO) & MASK) * ((lightColor >>> BO) & MASK) / 256;

        return A | r << RO | g << GO |b << BO;
    }

    public static int scale(int c1, double d) {
        int a = (c1 >>> AO),
                r = (c1 >>> RO) & MASK,
                g = (c1 >>> GO) & MASK,
                b = (c1 >>> BO) & MASK;

        //a*=d;
        r *= d;
        g *= d;
        b *= d;

        return a << AO |
                (r > MASK ? R : (r << RO)) |
                (g > MASK ? G : (g << GO)) |
                (b > MASK ? B : (b << BO));
    }
}
