using System;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Scene
{
    public class Camera : Basic3DObject
    {
        //Linke obere Ecke der ViewPane
        public Vect3 ViewPaneEdge { get; private set; }

        //RichtungsVektor von der linken oberen Ecke zur linken unteren Ecke
        public Vect3 ViewPaneHeightVector { get; private set; }

        //RichtungsVektor von der linken oberen Ecke zur rechten oberen Ecke
        public Vect3 ViewPaneWidthVector { get; private set; }

        private Camera(Vect3 position, Vect3 viewPaneEdge, Vect3 viewPaneWidthVector, Vect3 viewPaneHeightVector)
            : base(position, new Vect3())
        {
            ViewPaneEdge = viewPaneEdge;
            ViewPaneWidthVector = viewPaneWidthVector;
            ViewPaneHeightVector = viewPaneHeightVector;
        }

        public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double width, double height,
                      double absoluteHeight)
            : base(position, viewPaneCenter)
        {
            Camera temp = CreateCamera(position, viewPaneCenter, camUp, width/height*absoluteHeight, absoluteHeight);

            Position = temp.Position;
            ViewPaneEdge = temp.ViewPaneEdge;
            ViewPaneHeightVector = temp.ViewPaneHeightVector;
            ViewPaneWidthVector = temp.ViewPaneWidthVector;
        }

        public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp) : base(position, viewPaneCenter)
        {
            Camera temp = CreateCamera(position, viewPaneCenter, camUp, 640.0/480, 1);

            Position = temp.Position;
            ViewPaneEdge = temp.ViewPaneEdge;
            ViewPaneHeightVector = temp.ViewPaneHeightVector;
            ViewPaneWidthVector = temp.ViewPaneWidthVector;
        }

        public static Camera CreateCamera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double viewPaneWidth,
                                          double viewPaneHeight)
        {
            var viewPaneHeightVector = camUp;
            viewPaneHeightVector *= -viewPaneHeight;

            Vect3 temp1 = position - viewPaneCenter;
            Vect3 viewPaneWidthVector = Vect3Extensions.CrossProduct(temp1, viewPaneHeightVector);
            viewPaneWidthVector = viewPaneWidthVector.Normalize();
            viewPaneWidthVector *= viewPaneWidth;

            viewPaneWidthVector.CopyDataTo(ref temp1);
            temp1 = temp1/2;
            Vect3 viewPaneEdge = viewPaneCenter - temp1;
            viewPaneHeightVector.CopyDataTo(ref temp1);
            temp1 = temp1/2;
            viewPaneEdge -= temp1;

            return new Camera(position, viewPaneEdge, viewPaneWidthVector, viewPaneHeightVector);
        }

        // cheap ;)
        public void SetScreenDimensions(int width, int height)
        {
            double factor = width/(2.0*height);
            ViewPaneWidthVector /= 2;
            ViewPaneEdge += ViewPaneWidthVector;
            ViewPaneWidthVector = ViewPaneWidthVector.Normalize();
            ViewPaneWidthVector *= factor;
            ViewPaneEdge -= ViewPaneWidthVector;
            ViewPaneWidthVector *= 2;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            throw new Exception("not yet implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r)
        {
            return RaySphere.GetHitPointRaySphereDistance(r.Origin, r.Direction, Position, 0);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            Vect3 target = hitPoint - Position;
            return target.Normalize();
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return false;
        }

        public override double GetBoundingSphereRadius()
        {
            return 0;
        }
    }
}