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
            var planePoint = new Vect3(cubeCenter.X - cubeWidthHalf, cubeCenter.Y, cubeCenter.Z);
            var planeNormal = new Vect3(cubeWidthHalf, 0, 0);
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;
            planePoint.X += 2*cubeWidthHalf;
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;

            // --- Y ---
            planePoint = new Vect3(cubeCenter.X, cubeCenter.Y - cubeWidthHalf, cubeCenter.Z);
            planeNormal = new Vect3(0, cubeWidthHalf, 0);
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;
            planePoint.Y += 2*cubeWidthHalf;
            if (AreaCone.IsAreaIntersectingCone(planeNormal, planePoint, cubeWidthHalf, conePosition, coneAxis,
                                                coneAxisLength, coneCosPhi))
                return true;

            // --- Z ---
            planePoint = new Vect3(cubeCenter.X, cubeCenter.Y, cubeCenter.Z - cubeWidthHalf);
            planeNormal = new Vect3(0, 0, cubeWidthHalf);
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