using JRayXLib.Colors;
using JRayXLib.Shapes;
using JRayXLib.Struct;
using NUnit.Framework;

namespace JRayXLibTests
{
    [TestFixture]
    class OctreeTests
    {
        [Test]
        public void OctreeFromOutside()
        {
            var ray = new Ray
                {
                    Origin = new Vect3 {X = -5},
                    Direction = new Vect3 {X = 1}
                };

            var octree = new Octree2(new Vect3(), 2);
            var sphere = new Sphere(new Vect3(), 2, Color.Red);
            octree.Insert(sphere);

            var collisionDetails = octree.GetFirstCollision(ray);
            Assert.That(!double.IsInfinity(collisionDetails.Distance));
            Assert.That(collisionDetails.Obj, Is.EqualTo(sphere));
        }

        [Test]
        public void OctreeFromInside()
        {
            var ray = new Ray
            {
                Origin = new Vect3 { X = -5 },
                Direction = new Vect3 { X = 1 }
            };

            var octree = new Octree2(new Vect3(), 10);
            var sphere = new Sphere(new Vect3(), 2, Color.Red);
            octree.Insert(sphere);

            var collisionDetails = octree.GetFirstCollision(ray);
            Assert.That(!double.IsInfinity(collisionDetails.Distance));
            Assert.That(collisionDetails.Obj, Is.EqualTo(sphere));
        }
    }
}
