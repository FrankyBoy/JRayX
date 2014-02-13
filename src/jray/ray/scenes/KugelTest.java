package jray.ray.scenes;

import java.util.ArrayList;
import java.util.List;

import jray.common.Object3D;
import jray.common.Sky;
import jray.common.Sphere;
import jray.common.Vect3;
import jray.ray.Camera;
import jray.ray.Scene;


public class KugelTest extends Scene {

	private List<Object3D> objects;
	private Camera cam;
	
	public KugelTest(){
		objects = new ArrayList<Object3D>();
		objects.add(new Sphere(new Vect3(0,0,0),1,0xFFFFFF00));
		cam = new Camera(new Vect3(0,0,-5),new Vect3(0,0,-3),new Vect3(0,1,0));
	}
	
	@Override
	public Camera getCamera() {
		return cam;
	}

	@Override
	public Sky getSky() {
		return null;
	}

	@Override
	public List<Object3D> getObjects() {
		return objects;
	}

}
