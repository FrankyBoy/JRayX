using System;
using JRayXLib.Util;

namespace JRayXLib.Colors
{
    public struct WideColor
    {
        public ushort A, R, G, B;

        public static WideColor operator +(WideColor c, WideColor other)
        {
            return new WideColor
                {
                    A = (ushort) (c.A + other.A),
                    R = (ushort) (c.R + other.R),
                    G = (ushort) (c.G + other.G),
                    B = (ushort) (c.G + other.G)
                };
        }

        public static WideColor operator *(WideColor c, double d)
        {
            return new WideColor
                {
                    A = c.A,
                    R = (ushort) (c.R * d),
                    G = (ushort) (c.G * d),
                    B = (ushort) (c.B * d)
                };
        }
    }
    

    public static class WideColorExtensions {

        public static ushort GetMax(this WideColor c)
        {
            return System.Math.Max(c.R, System.Math.Max(c.G, c.B));
        }

        public static Color To8Bit(this WideColor c)
        {
            return new Color
                {
                    A = (byte)MathHelper.Clamp(c.A, byte.MinValue, byte.MaxValue),
                    R = (byte)MathHelper.Clamp(c.R, byte.MinValue, byte.MaxValue),
                    G = (byte)MathHelper.Clamp(c.G, byte.MinValue, byte.MaxValue),
                    B = (byte)MathHelper.Clamp(c.B, byte.MinValue, byte.MaxValue)
                };
        }

        public static WideColor MixLightLight(this WideColor l1, WideColor l2)
        {
            return l1 + l2;
        }

        /**
         * mix two surface colors, eg. for transparent texture overlays
         * @param topColor
         * @param bottomColor
         * @return
         */
        public static WideColor MixSurfaceSurface(this WideColor topColor, WideColor bottomColor) {
            
            // the two most common cases: top color is fully opaque or fully transparent
            if (topColor.A >= 0xFF)
                return topColor;

            if (topColor.A == 0x00)
                return bottomColor;
            
            // we know that a is smaller than 255 already so we don't have to check
            var alpha = topColor.A / 256.0;
            var invAlpha = 1 - alpha;

            return new WideColor
                {
                    A = (ushort) (topColor.A        + bottomColor.A*invAlpha),
                    R = (ushort) (topColor.R*alpha  + bottomColor.R*invAlpha),
                    G = (ushort) (topColor.G*alpha  + bottomColor.G*invAlpha),
                    B = (ushort) (topColor.B*alpha  + bottomColor.B*invAlpha)
                };
        }

        /**
         * mix a surface color with a light color
         * @param surfaceColor
         * @param lightColor
         * @return
         */
        public static WideColor MixSurfaceLight(this WideColor surfaceColor, WideColor lightColor) {
            
            // weight the surface colors with the light colors (multiply, divide by normalized maximum)
            return new WideColor
                {
                    A = surfaceColor.A,
                    R = (ushort) (surfaceColor.R * lightColor.R / 256),
                    G = (ushort) (surfaceColor.G * lightColor.G / 256),
                    B = (ushort) (surfaceColor.B * lightColor.B / 256)
                };
        }

        [Obsolete("use * operator")]
        public static WideColor Scale(this WideColor c1, double d)
        {
            return c1*d;
        }
    }
}
