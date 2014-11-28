using System;
using JRayXLib.Shapes;
using NUnit.Framework;

namespace JRayXLibTests
{
    [TestFixture]
    public class StructTests
    {
        [Test]
        public void StructsTest1()
        {
            var v1 = new Vect3{X = 10, Y = 20, Z = 30};
            var v2 = v1;
            v1.X = 3;
            Assert.That(v2.X, Is.EqualTo(10));
            Assert.That(v2.Y, Is.EqualTo(20));
            Assert.That(v2.Z, Is.EqualTo(30));
        }
    }
}
