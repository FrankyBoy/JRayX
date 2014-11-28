using JRayXLib.Shapes;

namespace JRayXLib.Math.intersections
{
    public class CubeCone
    {
        public static bool IsCubeEnclosingCone(Vect3 cubeCenter, double cubeWidthHalf, Vect3 conePosition,
                                               Vect3 coneAxis, double coneAxisLength, double coneCosPhi)
        {
            return PointCube.Encloses(cubeCenter, cubeWidthHalf, conePosition) &&
                   !IsCubeIntersectingCone(cubeCenter, cubeWidthHalf, conePosition, coneAxis, coneAxisLength, coneCosPhi);
        }


        public static bool IsCubeIntersectingCone(Vect3 cubeCenter, double cubeWidthHalf, Vect3 conePosition,
                                                  Vect3 coneAxis, double coneAxisLength, double coneCosPhi)
        {
            // --- X ---
            var planePoint = new Vect3
                {
                    X = cubeCenter.X - cubeWidthHalf,
                    Y = cubeCenter.Y,
                    Z = cubeCenter.Z
                };
            var planeNormal = new Vect3
                {
                    X = cubeWidthHalf
                };
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;
            planePoint.X += 2*cubeWidthHalf;
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;

            // --- Y ---
            planePoint = new Vect3
            {
                X = cubeCenter.X,
                Y = cubeCenter.Y - cubeWidthHalf,
                Z = cubeCenter.Z
            };
            planeNormal = new Vect3
            {
                Y = cubeWidthHalf
            };
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;
            planePoint.Y += 2*cubeWidthHalf;
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;

            // --- Z ---
            planePoint = new Vect3
            {
                X = cubeCenter.X,
                Y = cubeCenter.Y,
                Z = cubeCenter.Z - cubeWidthHalf
            };
            planeNormal = new Vect3
            {
                Z = cubeWidthHalf
            };
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;
            planePoint.Z += 2*cubeWidthHalf;
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;

            return false;
        }
    }
}