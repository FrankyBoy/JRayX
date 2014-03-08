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

        /// <summary>
        /// Inserts a node into the tree and returns "true" for success or "false" for failure.
        /// </summary>
        /// <param name="obj">the object to insert</param>
        /// <returns>"true" for success, "false" for failure</returns>
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
        
        public CollisionDetails GetFirstCollision(Shapes.Ray ray)
        {
            // which three sides do we need to perform a collision check with?
            // value > 0 means vect shows in that direction, so nearer side has to be
            // the one with opposite signs
            bool useBiggerX = ray.Direction.X < 0;
            bool useBiggerY = ray.Direction.Y < 0;
            bool useBiggerZ = ray.Direction.Z < 0;

            bool isHit = !double.IsInfinity(RayIntersectionDistance(ray, useBiggerX, useBiggerY, useBiggerZ));
            bool isRayInside = PointCube.Encloses(_center, _halfWidth, ray.Origin);
            
            if(isHit || isRayInside)
                return GetFirstCollisionInternal(ray, useBiggerX, useBiggerY, useBiggerZ);

            return new CollisionDetails(ray);
        }

        private CollisionDetails GetFirstCollisionInternal(Shapes.Ray ray, bool useBiggerX, bool useBiggerY, bool useBiggerZ)
        {
            var result = new CollisionDetails(ray);

            if(_children != null && _children.Length > 0){
                var tmpChildren = _children
                    .OrderBy(x => !PointCube.Encloses(x._center, x._halfWidth, ray.Origin)) // orderby orders first false, then true
                    .ThenBy(x => x.RayIntersectionDistance(ray, useBiggerX, useBiggerY, useBiggerZ))
                    .ToArray();

                foreach (var child in tmpChildren)
                {
                    var collision = child.GetFirstCollisionInternal(ray, useBiggerX, useBiggerY, useBiggerZ);
                    if (!double.IsInfinity(collision.Distance))
                    {
                        result = collision;
                        break;
                    }
                }
            }

            foreach (var o3D in _objects)
            {
                double distance = o3D.GetHitPointDistance(ray);
                if (distance < result.Distance)
                {
                    result.Obj = o3D;
                    result.Distance = distance;
                }
            }

            return result;
        }


        /// <summary>
        /// for a given object return the location where it should be inserted.
        /// where the object is inserted is determined by its bounding sphere.
        ///  * if the sphere is bigger than the object or does not intersect -> dont insert
        ///  * if it is bigger than half the size -> insert into own list
        ///  * if it is smaller than that -> insert into a child node
        /// </summary>
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

        /// <summary>
        /// Get the nearest intersection with the cube
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="useBiggerX"></param>
        /// <param name="useBiggerY"></param>
        /// <param name="useBiggerZ"></param>
        /// <returns></returns>
        private double RayIntersectionDistance(Shapes.Ray ray, bool useBiggerX, bool useBiggerY, bool useBiggerZ)
        {
            var normal = new Vect3{ X = useBiggerX ? _halfWidth : -_halfWidth };
            var planeCenter = _center + normal;
            double distance = SideRayIntersectionDistance(ray, normal, planeCenter);
            // we can return instantly as we only evaluate the hear side -> only one side should be hit at any time
            if (!double.IsInfinity(distance))
                return distance;

            normal = new Vect3 { Y = useBiggerY ? _halfWidth : -_halfWidth };
            planeCenter = _center + normal;
            distance = SideRayIntersectionDistance(ray, normal, planeCenter);
            if (!double.IsInfinity(distance))
                return distance;

            normal = new Vect3 { Z = useBiggerZ ? _halfWidth : -_halfWidth };
            planeCenter = _center + normal;
            distance = SideRayIntersectionDistance(ray, normal, planeCenter);
            if (!double.IsInfinity(distance))
                return distance;

            return double.PositiveInfinity;
        }

        /// <summary>
        /// Evaluate one side of the cube for collision
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="normal"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private double SideRayIntersectionDistance(Shapes.Ray ray, Vect3 normal, Vect3 center){
            var distance = RayPlane.GetHitPointRayPlaneDistance(ray.Origin, ray.Direction, center, normal);
            if (double.IsInfinity(distance))
                return distance;


            var relativeHitPoint = (ray.Origin + (ray.Direction * distance)) - center;

            if (relativeHitPoint.X <= _halfWidth && relativeHitPoint.Y <= _halfWidth && relativeHitPoint.Z <= _halfWidth)
                return distance;

            return double.PositiveInfinity;
        }

    }

    internal enum ObjectLocation
    {
        None,
        Self,
        Child
    }
}
