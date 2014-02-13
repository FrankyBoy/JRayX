package jray.ray;

import jray.common.Matrix4;
import jray.common.Object3D;
import jray.common.Ray;
import jray.common.Vect3;
import jray.math.Vect;
import jray.math.intersections.RaySphere;


public class Camera extends Object3D{

    private Vect3 position;
    private Vect3 viewPaneEdge; //Linke obere Ecke der ViewPane
    private Vect3 viewPaneWidthVector; //RichtungsVektor von der linken oberen Ecke zur rechten oberen Ecke
    private Vect3 viewPaneHeightVector; //RichtungsVektor von der linken oberen Ecke zur linken unteren Ecke

    private Camera(Vect3 position, Vect3 viewPaneEdge, Vect3 viewPaneWidthVector, Vect3 viewPaneHeightVector) {
    	super(position, null);
        this.position = position;
        this.viewPaneEdge = viewPaneEdge;
        this.viewPaneWidthVector = viewPaneWidthVector;
        this.viewPaneHeightVector = viewPaneHeightVector;
    }

    public Vect3 getPosition() {
        return position;
    }

    public Vect3 getViewPaneEdge() {
        return viewPaneEdge;
    }

    public Vect3 getViewPaneWidthVector() {
        return viewPaneWidthVector;
    }

    public Vect3 getViewPaneHeightVector() {
        return viewPaneHeightVector;
    }

    public static Camera createCamera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double viewPaneWidth, double viewPaneHeight) {
        Vect3 temp1 = new Vect3(camUp);
        temp1.normalize();
        Vect3 viewPaneHeightVector = new Vect3(camUp);
        Vect.scale(viewPaneHeightVector, -viewPaneHeight, viewPaneHeightVector);

        Vect3 viewPaneWidthVector = new Vect3();
        Vect.subtract(position, viewPaneCenter, temp1);
        Vect.crossProduct(temp1, viewPaneHeightVector, viewPaneWidthVector);
        viewPaneWidthVector.normalize();
        Vect.scale(viewPaneWidthVector, viewPaneWidth, viewPaneWidthVector);

        Vect3 viewPaneEdge = new Vect3();
        viewPaneWidthVector.copyDataTo(temp1);
        Vect.scale(temp1, 0.5, temp1);
        Vect.subtract(viewPaneCenter, temp1, viewPaneEdge);
        viewPaneHeightVector.copyDataTo(temp1);
        Vect.scale(temp1, 0.5, temp1);
        Vect.subtract(viewPaneEdge, temp1, viewPaneEdge);

        return new Camera(position, viewPaneEdge, viewPaneWidthVector, viewPaneHeightVector);
    }

    public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp, double width, double height, double absoluteHeight) {
    	super(position, new Vect3(viewPaneCenter));
        Camera temp = createCamera(position, viewPaneCenter, camUp, width / height * absoluteHeight, absoluteHeight);

        this.position = temp.position;
        this.viewPaneEdge = temp.viewPaneEdge;
        this.viewPaneHeightVector = temp.viewPaneHeightVector;
        this.viewPaneWidthVector = temp.viewPaneWidthVector;
    }

    public Camera(Vect3 position, Vect3 viewPaneCenter, Vect3 camUp) {
    	super(position, new Vect3(viewPaneCenter));
        Camera temp = createCamera(position, viewPaneCenter, camUp, 640.0 / 480, 1);

        this.position = temp.position;
        this.viewPaneEdge = temp.viewPaneEdge;
        this.viewPaneHeightVector = temp.viewPaneHeightVector;
        this.viewPaneWidthVector = temp.viewPaneWidthVector;
    }

    @Override
    public String toString() {
        return "Camera@" + position + " VPE=" + viewPaneEdge + " VPHV=" + viewPaneHeightVector + " VPWV=" + viewPaneWidthVector;
    }

    // cheap ;)
    void setScreenDimensions(int width, int height) {
        double factor = width / (2.0 * height);
        Vect.scale(viewPaneWidthVector, 0.5, viewPaneWidthVector);
        Vect.add(viewPaneEdge, viewPaneWidthVector, viewPaneEdge);
        viewPaneWidthVector.normalize();
        Vect.scale(viewPaneWidthVector, factor, viewPaneWidthVector);
        Vect.subtract(viewPaneEdge, viewPaneWidthVector, viewPaneEdge);
        Vect.scale(viewPaneWidthVector, 2, viewPaneWidthVector);
    }

	@Override
	public void rotate(Matrix4 rotationMatrix) {
        throw new RuntimeException("not yet implemented");
	}

	@Override
	public double getHitPointDistance(Ray r) {
		return RaySphere.getHitPointRaySphereDistance(r.getOrigin(), r.getDirection(), position, 0);
	}

	@Override
	public void getNormalAt(Vect3 hitPoint, Vect3 normal) {
		Vect.subtract(hitPoint, position, normal);
        normal.normalize();
	}

	@Override
	public boolean contains(Vect3 hitPoint) {
		return false;
	}
	
	@Override
	public double getBoundingSphereRadius() {
		return 0;
	}
}
