using System;
using System.Linq;
using JRayXLib.Colors;
using JRayXLib.Math;

namespace JRayXLib.Shapes
{
    public class Basic3DObjectStructure : Basic3DObject
    {
        private readonly I3DObject[] _objects;

        public Basic3DObjectStructure(Vect3 position, Vect3 lookAt, I3DObject[] objects)
            : base(position, lookAt)
        {
            _objects = objects;
        }

        public override double GetHitPointDistance(Ray r)
        {
            return _objects.Min(o3D => o3D.GetHitPointDistance(r));
        }

        protected I3DObject GetObjectAt(Vect3 hitPoint)
        {
            return _objects.FirstOrDefault(o3D => o3D.Contains(hitPoint));
        }

        public new Color GetColorAt(Vect3 hitPoint)
        {
            I3DObject ret = GetObjectAt(hitPoint);
            if (ret != null)
            {
                return ret.GetColorAt(hitPoint);
            }
            throw new Exception("internal error!");
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint)
        {
            I3DObject ret = GetObjectAt(hitPoint);
            if (ret != null)
            {
                return ret.GetNormalAt(hitPoint);
            }
            throw new Exception("internal error!");
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return _objects.Any(o3D => o3D.Contains(hitPoint));
        }

        public override double GetBoundingSphereRadius()
        {
            throw new NotImplementedException();
        }

        public override Vect3 Position
        {
            get { return base.Position; }
            set
            {
                foreach (Basic3DObject o3D in _objects)
                {
                    Vect3 objPos = o3D.Position;
                    objPos -= base.Position;
                    o3D.Position = objPos + value;
                }

                base.Position = value;
            }
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            foreach (Basic3DObject o3D in _objects)
            {
                Vect3 objPos = o3D.Position;

                // TODO: we have a matrix to combine these three, right?
                objPos -= Position;
                objPos = VectMatrix.Multiply(objPos, rotationMatrix);
                o3D.Position = objPos + Position;

                o3D.Rotate(rotationMatrix);
            }
        }

        public new double GetReflectivityAt(Vect3 hitPoint)
        {
            I3DObject ret = GetObjectAt(hitPoint);
            if (ret != null)
            {
                return ret.GetReflectivityAt(hitPoint);
            }
            throw new Exception("internal error!");
        }

        public new string ToString()
        {
            return base.ToString() + " - ["
                   + _objects.Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b)
                   + "]";
        }
    }
}