
namespace JRayXLib.Math.intersections
{
    public class LongColors {

        public static ulong A = 0xFFFF000000000000L,
                            R = 0x0000FFFF00000000L,
                            G = 0x00000000FFFF0000L,
                            B = 0x000000000000FFFFL,
                            MASK = 0xFFFFL;
        public static  int AO = 48, RO = 32, GO = 16, BO = 0;

        public static int getMax(ulong l) {
            int r = (int) ((l >> RO) & MASK),
                g = (int) ((l >> GO) & MASK),
                b = (int) ((l >> BO) & MASK);

            int biggest = g > b ? g : b;

            if (r > biggest) {
                biggest = r;
            }

            return biggest;
        }

        public static uint toInt(ulong color) {
            uint a, r, g, b;
            a = (uint) ((color >> AO) & MASK);
            r = (uint) ((color >> RO) & MASK);
            g = (uint) ((color >> GO) & MASK);
            b = (uint) ((color >> BO) & MASK);

            return
                (a > IntColors.MASK ? IntColors.A : a << IntColors.AO) |
                (r > IntColors.MASK ? IntColors.R : r << IntColors.RO) |
                (g > IntColors.MASK ? IntColors.G : g << IntColors.GO) |
                (b > IntColors.MASK ? IntColors.B : b << IntColors.BO);
        }

        public static ulong mixLightLight(ulong light1, ulong light2) {
            return light1 + light2;
        }

        /**
     * mix two surface colors, eg. for transparent texture overlays
     * @param topColor
     * @param bottomColor
     * @return
     */
        public static ulong mixSurfaceSurface(ulong topColor, ulong bottomColor) {
            ulong a, r, g, b;
            a = (topColor >> AO) & MASK;

            // the two most common cases: top color is fully opaque or fully transparent
            if (a >= 0xFF) {
                return topColor;
            }
            if (a == 0x00) {
                return bottomColor;
            }
            // we know that a is smaller than 255 already so we don't have to check
            double alpha = a / 256.0;

            a = (ulong) (((topColor >> AO) & MASK) + ((bottomColor >> AO) & MASK) * (1 - alpha));
            r = (ulong) (((topColor >> RO) & MASK) * alpha + ((bottomColor >> RO) & MASK) * (1 - alpha));
            g = (ulong) (((topColor >> GO) & MASK) * alpha + ((bottomColor >> GO) & MASK) * (1 - alpha));
            b = (ulong) (((topColor >> BO) & MASK) * alpha + ((bottomColor >> BO) & MASK) * (1 - alpha));

            return a << AO | r << RO | g << GO | b << BO;
        }

        /**
     * mix a surface color with a light color
     * @param surfaceColor
     * @param lightColor
     * @return
     */
        public static ulong mixSurfaceLight(ulong surfaceColor, ulong lightColor) {
            ulong r, g, b;
            // weight the surface colors with the light colors (multiply, divide by normalized maximum)
            r = ((surfaceColor >> RO) & MASK) * ((lightColor >> RO) & MASK) / 256;
            g = (((surfaceColor >> GO) & MASK) * ((lightColor >> GO) & MASK) / 256);
            b = (((surfaceColor >> BO) & MASK) * ((lightColor >> BO) & MASK) / 256);

            return
                (surfaceColor  & A) |
                (r > MASK ? R : r << RO) |
                (g > MASK ? G : g << GO) |
                (b > MASK ? B : b << BO);
        }

        public static ulong scale(ulong c1, double d) {
            ulong a = (c1 >> AO),
                 r = ((c1 >> RO) & MASK),
                 g = ((c1 >> GO) & MASK),
                 b = ((c1 >> BO) & MASK);

            r = (ulong)(r * d);
            g = (ulong)(g * d);
            b = (ulong)(b * d);

            return a << AO |
                   (r > MASK ? R : (r << RO)) |
                   (g > MASK ? G : (g << GO)) |
                   (b > MASK ? B : (b << BO));
        }
    }
}
