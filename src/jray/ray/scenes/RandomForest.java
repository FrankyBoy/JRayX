package jray.ray.scenes;

import java.io.IOException;
import java.util.List;
import java.util.Vector;

import jray.common.Cone;
import jray.common.Object3D;
import jray.common.Sky;
import jray.common.Sphere;
import jray.common.Vect3;
import jray.math.intersections.IntColors;
import jray.ray.Camera;
import jray.ray.Scene;
import jray.struct.Octree;


public class RandomForest extends Scene {

    private List<Object3D> objects = new Vector<Object3D>();
    private Camera c;
    private Octree tree;

    public RandomForest() throws IOException{
    	double dist=0.2;
    	double h = 0.3;
    	double camheight = 10;
    	
    	for(double x = 1;x<camheight*3;x+=h)
    		for(double z = -camheight;z<camheight;z+=h)
    			addTree(x+Math.random()*dist-dist/2,-2,z+Math.random()*dist-dist/2);
        
        c = new Camera(new Vect3(-camheight, camheight*2, 0), new Vect3(-camheight+1, camheight*2-1, 0), new Vect3(0.707106781186547, 0.707106781186547, 0), 1, 1, 1);
        objects.add(c);
        
        tree = Octree.buildTree(new Vect3(camheight*1.5,-3.1,0),objects);
    }

    private void addTree(double x, double y, double z){
    	double dist= 0.1;
    	double t0 = Math.random()*dist-dist/2;
    	double t1 = Math.random()*dist-dist/2;
    	double ld = 0.2;
    	
    	int leafColor = IntColors.A+(((int)(IntColors.G*(0.5 + 0.5*Math.random())))&IntColors.G);
    	
    	if(Math.random()<0.5){
    		objects.add(new Cone(new Vect3(x,y+2,z),new Vect3(t0,-1.6,t1),10,leafColor));
    		objects.add(new Cone(new Vect3(x,y+2,z),new Vect3(t0*2,-2,t1*2),1,0xFF8B4513));
    	}else{
    		objects.add(new Sphere(new Vect3(x,y+1.5-ld/2,z),ld,leafColor));
    		objects.add(new Cone(new Vect3(x,y+1.5,z),new Vect3(t0*2,-1.5,t1*2),1,0xFF8B4513));
    	}
    }
    
    @Override
    public List<Object3D> getObjects() {
        return objects;
    }

    @Override
    public Camera getCamera() {
        return c;
    }

    @Override
    public Sky getSky() {
        return null;
    }
    
    @Override
    public Octree getSceneTree(){
    	return tree;
    }
}
