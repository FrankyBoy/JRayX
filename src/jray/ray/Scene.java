package jray.ray;

import java.util.List;

import jray.common.Object3D;
import jray.common.Sky;
import jray.struct.Octree;

abstract public class Scene {

    protected String name = this.getClass().getName();

    public abstract Camera getCamera();
    public abstract Sky getSky();

    public abstract List<Object3D> getObjects();

    public String getName() {
        return name;
    }
    
    public Octree getSceneTree(){
    	return null;
    }
}
