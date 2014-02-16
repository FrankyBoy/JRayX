using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class CubeSphere {

        public static bool IsSphereEnclosedByCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius){
            return System.Math.Abs(cCenter[0] - sCenter[0]) < cWidthHalf - sRadius &&
                   System.Math.Abs(cCenter[1] - sCenter[1]) < cWidthHalf - sRadius &&
                   System.Math.Abs(cCenter[2] - sCenter[2]) < cWidthHalf - sRadius;
        }

        public static bool IsSphereIntersectingCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius)
        {
            return System.Math.Abs(cCenter[0] - sCenter[0]) < cWidthHalf + sRadius &&
                   System.Math.Abs(cCenter[1] - sCenter[1]) < cWidthHalf + sRadius &&
                   System.Math.Abs(cCenter[2] - sCenter[2]) < cWidthHalf + sRadius;
        }
    }
}
