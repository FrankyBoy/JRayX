namespace JRayXLib.Colors
{

    public struct Color
    {
        public byte A, R, G, B;

        public static Color Green = new Color { A = byte.MaxValue, G = byte.MaxValue };
        public static Color Red = new Color { A = byte.MaxValue, R = byte.MaxValue };
        public static Color Blue = new Color { A = byte.MaxValue, B = byte.MaxValue };
        public static Color Black = new Color { A = byte.MaxValue };

        public static Color operator +(Color c, Color other)
        {
            return new Color
            {
                A = (byte)(c.A + other.A),
                R = (byte)(c.R + other.R),
                G = (byte)(c.G + other.G),
                B = (byte)(c.G + other.G)
            };
        }

        public static Color operator *(Color c, double d)
        {
            return new Color
            {
                A = c.A,
                R = (byte)(c.R * d),
                G = (byte)(c.G * d),
                B = (byte)(c.B * d)
            };
        }

    }

    public static class ColorExtensions
    {
        public static WideColor To16Bit(this Color c) {
            return new WideColor
                {
                    A = c.A,
                    R = c.R,
                    G = c.G,
                    B = c.B
                };
        }

        public static Color MixLightLight(this Color light1, Color light2)
        {
            return light1 + light2;
        }

        /**
         * mix two surface colors, eg. for transparent texture overlays
         * @param topColor
         * @param bottomColor
         * @return
         */
        public static Color MixSurfaceSurface(this Color topColor, Color bottomColor)
        {
            
            if (topColor.A == byte.MaxValue) {
                return topColor;
            }
            if (topColor.A == 0) {
                return bottomColor;
            }

            double alpha = topColor.A / 256.0;
            double invAlpha = 1 - alpha;

            return new Color {
                    A = (byte)(topColor.A + bottomColor.A * invAlpha),
                    R = (byte)(topColor.R * alpha + bottomColor.R * invAlpha),
                    G = (byte)(topColor.G * alpha + bottomColor.G * invAlpha),
                    B = (byte)(topColor.B * alpha + bottomColor.B * invAlpha)
                };
        }

        /**
         * mix a surface color with a light color
         * @param surfaceColor
         * @param lightColor 
         * @return
         */
        public static Color MixSurfaceLight(Color surfaceColor, Color lightColor)
        {
            return new Color
            {
                A = surfaceColor.A,
                R = (byte)(surfaceColor.R * lightColor.R / 256),
                G = (byte)(surfaceColor.G * lightColor.G / 256),
                B = (byte)(surfaceColor.B * lightColor.B / 256)
            };
        }

        public static Color Scale(Color c1, double d)
        {
            return c1*d;
        }
    }
}
