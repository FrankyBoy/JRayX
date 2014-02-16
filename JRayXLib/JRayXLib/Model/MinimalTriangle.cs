using System;
using JRayXLib.Math;
using JRayXLib.Colors;
using JRayXLib.Math.intersections;
using JRayXLib.Shapes;

namespace JRayXLib.Model
{
    public class MinimalTriangle : Basic3DObject{

        protected Vect3 V2;
        protected Vect3 V3;
        protected Sphere Bounds;
	
        public MinimalTriangle(Vect3 normal, Vect3 v1, Vect3 v2, Vect3 v3) : base(v1, normal) {
            V2 = v2;
            V3 = v3;
		
            Vect3 avg = GetBoundingSphereCenter();
            Vect3 tmp = new Vect3(0);
            Vect.Subtract(avg, v3, ref tmp);
    	
            Bounds = new Sphere(avg,tmp.Length(), Color.Black);
        }

        public override bool Contains(Vect3 hitPoint) {
            throw new Exception("not implemented");
        }

        public override double GetHitPointDistance(Shapes.Ray r)
        {
            var v1v2 = new Vect3(0);
            var v1v3 = new Vect3(0);
		
            Vect.Subtract(V2, Position, ref v1v2);
            Vect.Subtract(V3, Position, ref v1v3);
		
            double ret = RayTriangle.GetHitPointRayTriangleDistance(r.GetOrigin(), r.GetDirection(), Position, v1v2, v1v3);
            if (ret <= 0) {
                return double.PositiveInfinity;
            }
            return ret;
        }

        public override Vect3 GetNormalAt(Vect3 hitPoint) {
            return new Vect3(LookAt);
        }

        public override void Rotate(Matrix4 rotationMatrix) {
            throw new Exception("not implemented");
        }
	
        public new Sphere GetBoundingSphere(){
            return Bounds;
        }
	
        public new Vect3 GetBoundingSphereCenter(){
            return Vect.Avg(new []{Position, V2, V3});
        }
    
        public override double GetBoundingSphereRadius(){
            return Bounds.GetRadius();
        }
	
        public void purge(){
            Bounds = null;
        }
    }
}
