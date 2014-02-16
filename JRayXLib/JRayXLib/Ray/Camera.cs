using System;
using JRayXLib.Math;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Ray
{
    public class Camera : Object3D{

        private readonly Vect3 _position;
        private readonly Vect3 _viewPaneEdge; //Linke obere Ecke der ViewPane
        private readonly Vect3 _viewPaneWidthVector; //RichtungsVektor von der linken oberen Ecke zur rechten oberen Ecke
        private readonly Vect3 _viewPaneHeightVector; //RichtungsVektor von der linken oberen Ecke zur linken unteren Ecke

        private Camera(Vect3 position, Vect3 viewPaneEdge, Vect3 viewPaneWidthVector, Vect3 viewPaneHeightVector)
            : base(position, null){
            _position = position;
            _viewPaneEdge = viewPaneEdge;
            _viewPaneWidthVector = viewPaneWidthVector;
            _viewPaneHeightVector = viewPaneHeightVector;
            }

        public Vect3 GetPosition() {
            return _position;
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
            temp1.normalize();
            var viewPaneHeightVector = new Vect3(camUp);
            Vect.Scale(viewPaneHeightVector, -viewPaneHeight, viewPaneHeightVector);

            var viewPaneWidthVector = new Vect3();
            Vect.subtract(position, viewPaneCenter, temp1);
            Vect.crossProduct(temp1, viewPaneHeightVector, viewPaneWidthVector);
            viewPaneWidthVector.normalize();
            Vect.Scale(viewPaneWidthVector, viewPaneWidth, viewPaneWidthVector);

            var viewPaneEdge = new Vect3();
            viewPaneWidthVector.CopyDataTo(temp1);
            Vect.Scale(temp1, 0.5, temp1);
            Vect.subtract(viewPaneCenter, temp1, viewPaneEdge);
            viewPaneHeightVector.CopyDataTo(temp1);
            Vect.Scale(temp1, 0.5, temp1);
            Vect.subtract(viewPaneEdge, temp1, viewPaneEdge);

            return new Camera(position, viewPaneEdge, viewPaneWidthVector, viewPaneHeightVector);
        }

        public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double width, double height, double absoluteHeight)
            : base(position, new Vect3(viewPaneCenter)){
            Camera temp = CreateCamera(position, viewPaneCenter, camUp, width / height * absoluteHeight, absoluteHeight);

            _position = temp._position;
            _viewPaneEdge = temp._viewPaneEdge;
            _viewPaneHeightVector = temp._viewPaneHeightVector;
            _viewPaneWidthVector = temp._viewPaneWidthVector;
            }

        public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp) : base(position, new Vect3(viewPaneCenter)) {
    	
            Camera temp = CreateCamera(position, viewPaneCenter, camUp, 640.0 / 480, 1);

            _position = temp._position;
            _viewPaneEdge = temp._viewPaneEdge;
            _viewPaneHeightVector = temp._viewPaneHeightVector;
            _viewPaneWidthVector = temp._viewPaneWidthVector;
        }

        public new string ToString() {
            return "Camera@" + _position + " VPE=" + _viewPaneEdge + " VPHV=" + _viewPaneHeightVector + " VPWV=" + _viewPaneWidthVector;
        }

        // cheap ;)
        public void SetScreenDimensions(int width, int height) {
            double factor = width / (2.0 * height);
            Vect.Scale(_viewPaneWidthVector, 0.5, _viewPaneWidthVector);
            Vect.Add(_viewPaneEdge, _viewPaneWidthVector, _viewPaneEdge);
            _viewPaneWidthVector.normalize();
            Vect.Scale(_viewPaneWidthVector, factor, _viewPaneWidthVector);
            Vect.subtract(_viewPaneEdge, _viewPaneWidthVector, _viewPaneEdge);
            Vect.Scale(_viewPaneWidthVector, 2, _viewPaneWidthVector);
        }

        public override void Rotate(Matrix4 rotationMatrix) {
            throw new Exception("not yet implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r) {
            return RaySphere.GetHitPointRaySphereDistance(r.GetOrigin(), r.GetDirection(), _position, 0);
        }

        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal) {
            Vect.subtract(hitPoint, _position, normal);
            normal.normalize();
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
