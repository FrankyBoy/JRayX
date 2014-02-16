using System;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Ray
{
    public class Camera : Basic3DObject{

        private Vect3 _viewPaneEdge; //Linke obere Ecke der ViewPane
        private Vect3 _viewPaneWidthVector; //RichtungsVektor von der linken oberen Ecke zur rechten oberen Ecke
        private readonly Vect3 _viewPaneHeightVector; //RichtungsVektor von der linken oberen Ecke zur linken unteren Ecke

        private Camera(Vect3 position, Vect3 viewPaneEdge, Vect3 viewPaneWidthVector, Vect3 viewPaneHeightVector)
            : base(position, new Vect3(0)){
            _viewPaneEdge = viewPaneEdge;
            _viewPaneWidthVector = viewPaneWidthVector;
            _viewPaneHeightVector = viewPaneHeightVector;
            }

        public Vect3 GetViewPaneEdge() {
            return _viewPaneEdge;
        }

        public Vect3 GetViewPaneWidthVector() {
            return _viewPaneWidthVector;
        }

        public Vect3 GetViewPaneHeightVector() {
            return _viewPaneHeightVector;
        }

        public static Camera CreateCamera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double viewPaneWidth, double viewPaneHeight) {
            var temp1 = new Vect3(camUp);
            temp1.Normalize();
            var viewPaneHeightVector = new Vect3(camUp);
            Vect.Scale(viewPaneHeightVector, -viewPaneHeight, ref viewPaneHeightVector);

            Vect.Subtract(position, viewPaneCenter, ref temp1);
            var viewPaneWidthVector = Vect.CrossProduct(temp1, viewPaneHeightVector);
            viewPaneWidthVector.Normalize();
            Vect.Scale(viewPaneWidthVector, viewPaneWidth, ref viewPaneWidthVector);

            var viewPaneEdge = new Vect3(0);
            viewPaneWidthVector.CopyDataTo(ref temp1);
            Vect.Scale(temp1, 0.5, ref temp1);
            Vect.Subtract(viewPaneCenter, temp1, ref viewPaneEdge);
            viewPaneHeightVector.CopyDataTo(ref temp1);
            Vect.Scale(temp1, 0.5, ref temp1);
            Vect.Subtract(viewPaneEdge, temp1, ref viewPaneEdge);

            return new Camera(position, viewPaneEdge, viewPaneWidthVector, viewPaneHeightVector);
        }

        public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double width, double height, double absoluteHeight)
            : base(position, new Vect3(viewPaneCenter)){
            Camera temp = CreateCamera(position, viewPaneCenter, camUp, width / height * absoluteHeight, absoluteHeight);

            Position = temp.Position;
            _viewPaneEdge = temp._viewPaneEdge;
            _viewPaneHeightVector = temp._viewPaneHeightVector;
            _viewPaneWidthVector = temp._viewPaneWidthVector;
            }

        public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp) : base(position, new Vect3(viewPaneCenter)) {
    	
            Camera temp = CreateCamera(position, viewPaneCenter, camUp, 640.0 / 480, 1);

            Position = temp.Position;
            _viewPaneEdge = temp._viewPaneEdge;
            _viewPaneHeightVector = temp._viewPaneHeightVector;
            _viewPaneWidthVector = temp._viewPaneWidthVector;
        }

        public new string ToString() {
            return "Camera@" + Position + " VPE=" + _viewPaneEdge + " VPHV=" + _viewPaneHeightVector + " VPWV=" + _viewPaneWidthVector;
        }

        // cheap ;)
        public void SetScreenDimensions(int width, int height) {
            double factor = width / (2.0 * height);
            Vect.Scale(_viewPaneWidthVector, 0.5, ref _viewPaneWidthVector);
            Vect.Add(_viewPaneEdge, _viewPaneWidthVector, ref _viewPaneEdge);
            _viewPaneWidthVector.Normalize();
            Vect.Scale(_viewPaneWidthVector, factor, ref _viewPaneWidthVector);
            Vect.Subtract(_viewPaneEdge, _viewPaneWidthVector, ref _viewPaneEdge);
            Vect.Scale(_viewPaneWidthVector, 2, ref _viewPaneWidthVector);
        }

        public override void Rotate(Matrix4 rotationMatrix) {
            throw new Exception("not yet implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r) {
            return RaySphere.GetHitPointRaySphereDistance(r.GetOrigin(), r.GetDirection(), Position, 0);
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            var target = new Vect3(0);
            Vect.Subtract(hitPoint, Position, ref target);
            return target.Normalize();
        }

        public override bool Contains(Vect3 hitPoint) {
            return false;
        }

        public override double GetBoundingSphereRadius()
        {
            return 0;
        }
    }
}
