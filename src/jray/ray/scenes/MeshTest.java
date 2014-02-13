package jray.ray.scenes;

import java.io.File;
import java.io.IOException;
import java.util.List;
import java.util.Vector;

import jray.common.Object3D;
import jray.common.Sky;
import jray.common.Vect3;
import jray.model.BinarySTLParser;
import jray.model.ModelInstance;
import jray.model.TriangleMeshModel;
import jray.ray.Camera;
import jray.ray.Scene;
import jray.struct.Octree;

public class MeshTest extends Scene {

    private List<Object3D> objects = new Vector<Object3D>();
    private Camera c;
    private Octree tree;
    
    public MeshTest() throws IOException{
    	double camheight= 50;
    	c = new Camera(new Vect3(0, camheight*2, camheight), new Vect3(0, camheight*2-1, camheight-1), new Vect3(0, 0.707106781186547, -0.707106781186547), 1, 1, 1);
    	//c = new Camera(new Vect3(-camheight, camheight*2, 0), new Vect3(-camheight+1, camheight*2-1, 0), new Vect3(0.707106781186547, 0.707106781186547, 0), 1, 1, 1);
    	//c = new Camera(new Vect3(0, camheight*2, -camheight), new Vect3(0, camheight*2-1, -camheight+1), new Vect3(0, 0.707106781186547, 0.707106781186547), 1, 1, 1);
    	//c = new Camera(new Vect3(0, camheight, -10), new Vect3(0, camheight-1, -10), new Vect3(0, 0, 1), 1, 1, 1);
        objects.add(c);
        TriangleMeshModel model = BinarySTLParser.parse(new File("models/elka.stl"));
        
        int nx = 2;
        int ny = 4;
        for(int i=-nx;i<=nx;i++)
        	for(int j=-ny;j<=ny;j++)
        		objects.add(new ModelInstance(new Vect3(i*15+(j%2)*7,0,(j+4)*-15),model));
        
        //objects.add(new Plane(new Vect3(0,0,0),new Vect3(0,1,0),0xFFFFFFFF));
        tree = Octree.buildTree(new Vect3(0,0,0),objects);
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
