using JRayXLib.Colors;

namespace JRayXLib.Shapes
{
    public interface I3DObject
    {
        Vect3 Position { get; set; }
        Vect3 LookAt { get; set; }

        void Rotate(Vect3 axis, double rad);
        void Rotate(Matrix4 rotationMatrix);
        
        double GetHitPointDistance(Ray r);
        
        Color GetColorAt(Vect3 hitPoint);
        void GetNormalAt(Vect3 hitPoint, Vect3 normal);
        double GetReflectivityAt(Vect3 hitPoint);
        bool Contains(Vect3 hitPoint);

        double GetRefractionIndex();
        Sphere GetBoundingSphere();
        bool IsEnclosedByCube(Vect3 cCenter, double cWidthHalf);
    }
}