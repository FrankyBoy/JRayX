using System;

namespace JRayXLib.Util
{
    static class MathHelper
    {
        static public double ToRadians(double angle)
        {
            return (System.Math.PI / 180) * angle;
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
