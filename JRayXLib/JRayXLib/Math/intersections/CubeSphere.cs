using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class CubeSphere {

        public static bool IsSphereEnclosedByCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius)
        {
            var cdata = cCenter.Data;
            var sdata = sCenter.Data;
            return System.Math.Abs(cdata[0] - sdata[0]) < cWidthHalf - sRadius &&
                   System.Math.Abs(cdata[1] - sdata[1]) < cWidthHalf - sRadius &&
                   System.Math.Abs(cdata[2] - sdata[2]) < cWidthHalf - sRadius;
        }

        public static bool IsSphereIntersectingCube(Vect3 cCenter, double cWidthHalf, Vect3 sCenter, double sRadius)
        {
            var cdata = cCenter.Data;
            var sdata = sCenter.Data;
            return System.Math.Abs(cdata[0] - sdata[0]) < cWidthHalf + sRadius &&
                   System.Math.Abs(cdata[1] - sdata[1]) < cWidthHalf + sRadius &&
                   System.Math.Abs(cdata[2] - sdata[2]) < cWidthHalf + sRadius;
        }
    }
}
