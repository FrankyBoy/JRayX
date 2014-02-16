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
            : base(new Vect3(), lookAt)
        {
            _objects = objects;
            SetPosition(position);
        }

        // TODO: can we optimize this?
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
            var ret = GetObjectAt(hitPoint);
            if (ret != null)
            {
                return ret.GetColorAt(hitPoint);
            }
            throw new Exception("internal error!");
        }

        public override void GetNormalAt(Vect3 hitPoint, Vect3 normal)
        {
            var ret = GetObjectAt(hitPoint);
            if (ret != null)
            {
                ret.GetNormalAt(hitPoint, normal);
            }
            else
            {
                throw new Exception("internal error!");
            }
        }

        public override bool Contains(Vect3 hitPoint)
        {
            return _objects.Any(o3D => o3D.Contains(hitPoint));
        }

        public override double GetBoundingSphereRadius()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Vect3 position)
        {
            foreach (Basic3DObject o3D in _objects)
            {
                Vect3 objPos = o3D.Position;
                Vect.subtract(objPos, Position, objPos);
                Vect.Add(objPos, position, objPos);
            }

            Position = position;
        }

        public override void Rotate(Matrix4 rotationMatrix)
        {
            foreach (Basic3DObject o3D in _objects)
            {
                Vect3 objPos = o3D.Position;

                // TODO: we have a matrix to combine these three, right?
                Vect.subtract(objPos, Position, objPos);
                VectMatrix.multiply(objPos, rotationMatrix, objPos);
                Vect.Add(objPos, Position, objPos);

                o3D.Rotate(rotationMatrix);
            }
        }

        public new double GetReflectivityAt(Vect3 hitPoint)
        {
            var ret = GetObjectAt(hitPoint);
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