using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class CubeSphere {

        public static bool IsSphereEnclosedByCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius){
            return System.Math.Abs(cCenter.Data[0] - sCenter.Data[0]) < cWidthHalf - sRadius &&
                   System.Math.Abs(cCenter.Data[1] - sCenter.Data[1]) < cWidthHalf - sRadius &&
                   System.Math.Abs(cCenter.Data[2] - sCenter.Data[2]) < cWidthHalf - sRadius;
        }

        public static bool IsSphereIntersectingCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius)
        {
            return System.Math.Abs(cCenter.Data[0] - sCenter.Data[0]) < cWidthHalf + sRadius &&
                   System.Math.Abs(cCenter.Data[1] - sCenter.Data[1]) < cWidthHalf + sRadius &&
                   System.Math.Abs(cCenter.Data[2] - sCenter.Data[2]) < cWidthHalf + sRadius;
        }
    }
}
