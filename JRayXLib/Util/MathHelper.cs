namespace JRayXLib.Util
{
    internal static class MathHelper
    {
        public static double ToRadians(double angle)
        {
            return (System.Math.PI/180)*angle;
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }
    }
}