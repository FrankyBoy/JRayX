
namespace JRayXLib.Math.intersections
{
    public class IntColors
    {

        public static uint
            A = 0xFF000000,
            R = 0x00FF0000,
            G = 0x0000FF00,
            B = 0x000000FF;

        public static int
            AO = 24,
            RO = 16,
            GO = 8,
            BO = 0,
            MASK = 0xFF;

        public static long toLong(uint i) {
            long a = (i >> AO) & MASK;
            long r = (i >> RO) & MASK;
            long g = (i >> GO) & MASK;
            long b = (i >> BO) & MASK;

            return (a << LongColors.AO) | (r << LongColors.RO) | (g << LongColors.GO) | (b << LongColors.BO);
        }

        public static uint mixLightLight(uint light1, uint light2) {
            uint r = (uint) (((light1 >> RO) & MASK) + ((light2 >> RO) & MASK));
            uint g = (uint) (((light1 >> GO) & MASK) + ((light2 >> GO) & MASK));
            uint b = (uint) (((light1 >> BO) & MASK) + ((light2 >> BO) & MASK));

            return A | (r > MASK ? R : r << RO) | (g > MASK ? G : g << GO) | (b > MASK ? B : b << BO);
        }

        /**
     * mix two surface colors, eg. for transparent texture overlays
     * @param topColor
     * @param bottomColor
     * @return
     */
        public static uint mixSurfaceSurface(uint topColor, uint bottomColor) {
            // the two most common cases
            uint a = (char) ((topColor >> AO) & MASK);
            if (a == MASK) {
                return topColor;
            }
            if (a == 0x00) {
                return bottomColor;
            }

            double alpha = ((double) a) / 256;

            a = (uint) (((topColor >> AO) & MASK) +
                       ((bottomColor >> AO) & MASK) * (1 - alpha));
            uint r = (uint) (((topColor >> RO) & MASK) * alpha +
                           ((bottomColor >> RO) & MASK) * (1 - alpha));
            uint g = (uint) (((topColor >> GO) & MASK) * alpha +
                           ((bottomColor >> GO) & MASK) * (1 - alpha));
            uint b = (uint) (((topColor >> BO) & MASK) * alpha +
                           ((bottomColor >> BO) & MASK) * (1 - alpha));

            return a << AO | r << RO | g << GO | b << BO;
        }

        /**
     * mix a surface color with a light color
     * @param surfaceColor
     * @param lightColor 
     * @return
     */
        public static uint mixSurfaceLight(uint surfaceColor, uint lightColor) {
            uint r = (uint) (((surfaceColor >> RO) & MASK) * ((lightColor >> RO) & MASK) / 256);
            uint g = (uint) (((surfaceColor >> GO) & MASK) * ((lightColor >> GO) & MASK) / 256);
            uint b = (uint) (((surfaceColor >> BO) & MASK) * ((lightColor >> BO) & MASK) / 256);

            return A | r << RO | g << GO |b << BO;
        }

        public static uint scale(uint c1, double d) {
            uint a = (c1 >> AO),
                r = (uint) ((c1 >> RO) & MASK),
                g = (uint) ((c1 >> GO) & MASK),
                b = (uint) ((c1 >> BO) & MASK);

            //a*=d;
            r = (uint)(r * d);
            g = (uint)(g * d);
            b = (uint)(b * d);

            return a << AO |
                   (r > MASK ? R : (r << RO)) |
                   (g > MASK ? G : (g << GO)) |
                   (b > MASK ? B : (b << BO));
        }
    }
}
