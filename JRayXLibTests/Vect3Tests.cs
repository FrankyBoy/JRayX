using System;
using JRayXLib.Shapes;
using NUnit.Framework;

namespace JRayXLibTests
{
    [TestFixture]
    class Vect3Tests
    {
        [Test]
        public void DotProductSameVector()
        {
            var rd = new Random();
            var vect1 = new Vect3 {X = rd.NextDouble(), Y = rd.NextDouble(), Z = rd.NextDouble()};
            
            Assert.That(vect1 * vect1, Is.EqualTo(vect1.QuadLength()));
        }

        [Test]
        public void DotProductInverseVector()
        {
            var rd = new Random();
            var vect1 = new Vect3 { X = rd.NextDouble(), Y = rd.NextDouble(), Z = rd.NextDouble() };

            Assert.That(vect1 * (vect1*-1), Is.EqualTo(- vect1.QuadLength()));
        }

        [Test]
        public void DotProductRightAngle()
        {
            var vect1 = new Vect3 { X = 1 };
            var vect2 = new Vect3 { Y = 1 };

            Assert.That(vect1 * (vect2), Is.EqualTo(0));
        }
    }
}
