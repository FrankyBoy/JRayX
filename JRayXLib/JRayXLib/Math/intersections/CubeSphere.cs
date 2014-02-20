using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class CubeSphere
    {
        public static bool IsSphereEnclosedByCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius)
        {
            return System.Math.Abs(cCenter.X - sCenter.X) < cWidthHalf - sRadius &&
                   System.Math.Abs(cCenter.Y - sCenter.Y) < cWidthHalf - sRadius &&
                   System.Math.Abs(cCenter.Z - sCenter.Z) < cWidthHalf - sRadius;
        }

        public static bool IsSphereIntersectingCube(Vect3 cCenter, double cWidthHalf, Sphere sphere)
        {
            return System.Math.Abs(cCenter.X - sphere.Position.X) < cWidthHalf + sphere.Radius &&
                   System.Math.Abs(cCenter.Y - sphere.Position.Y) < cWidthHalf + sphere.Radius &&
                   System.Math.Abs(cCenter.Z - sphere.Position.Z) < cWidthHalf + sphere.Radius;
        }
    }
}