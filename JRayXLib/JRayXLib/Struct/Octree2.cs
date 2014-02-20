using System;
using System.Collections.Generic;
using System.Linq;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Struct
{
    
    public class Octree2
    {
        private Octree2[] _children;
        private readonly List<I3DObject> _objects = new List<I3DObject>();
        private readonly double _halfWidth;
        private readonly Vect3 _center;

        public Octree2(Vect3 center, double halfWidth)
        {
            _halfWidth = halfWidth;
            _center = center;
        }


        /* rules for insert:
         * case 1: bounding sphere radius is > halfWidth
         *  -> dont add
         *  
         * case 2: radius is < halfWidth and sphere is inside or intersects cube
         *  case 2a: radius < halfWidth/2 -> pass to children
         *  case 2b: radius > halfWidth/2 -> store yourself
         */
        public bool Insert(I3DObject obj)
        {
            var location = GetLocation(obj);
            switch (location)
            {
                case ObjectLocation.None:
                    return false;

                case ObjectLocation.Child:
                    if (_children == null)
                        _children = CreateChildren();
                    return _children.Select(x => x.Insert(obj)).Aggregate((a, b) => a || b);

                case ObjectLocation.Self:
                    _objects.Add(obj);
                    return true;
            }
            throw new Exception("Unknown ObjectLocation");
        }

        private ObjectLocation GetLocation(I3DObject obj)
        {
            var boundingSphere = obj.GetBoundingSphere();
            if (boundingSphere.Radius > _halfWidth || !CubeSphere.IsSphereIntersectingCube(_center, _halfWidth, boundingSphere))
                return ObjectLocation.None;

            if (boundingSphere.Radius < _halfWidth/2)
            {
                return ObjectLocation.Child;
            }

            return ObjectLocation.Self;
        }

        private Octree2[] CreateChildren()
        {
            var qwidth = _halfWidth/2;
            var result = new Octree2[8];

            for (int counter = 0; counter < 8; counter++)
                _children[counter] = new Octree2(
                    new Vect3
                    {
                        X = counter % 2 < 1 ? _center.X - qwidth : _center.X + qwidth,
                        Y = counter % 4 < 2 ? _center.Y - qwidth : _center.Y + qwidth,
                        Z = counter     < 4 ? _center.Z - qwidth : _center.Z + qwidth,
                    },
                    qwidth);

            return result;
        }



    }

    internal enum ObjectLocation
    {
        None,
        Self,
        Child
    }
}
