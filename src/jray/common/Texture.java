package jray.common;

import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import javax.imageio.ImageIO;

/**
 * Stores all Textures Loaded.
 * <b>
 * Maybe some day in future this thing supports texture filtering... 
 */
public class Texture {

    private static Map<File, Texture> storage = new HashMap<File, Texture>();
    private File path;
    private BufferedImage image;
    private int imageWidth;
    private int imageHeight;

    public Texture(File path) {
        this.path = path;
        try {
            image = ImageIO.read(path);
        } catch (IOException e) {
            throw new RuntimeException("Exception while reading image " + path, e);
        }

        if (image == null) {
            throw new RuntimeException("No imagereader capable of loading " + path + " was found!");
        }

        imageWidth = image.getWidth();
        imageHeight = image.getHeight();
    }

    public synchronized static Texture loadTexture(String path) {
        File f = new File(path).getAbsoluteFile();

        Texture ret = storage.get(f);

        if (ret == null) {
            storage.put(f, ret = new Texture(f));
        }

        return ret;
    }

    public int getColorAt(Vect2 texcoord) {
        double[] data = texcoord.getData();
        return getColorAt(data[0], data[1]);
    }

    public int getColorAt(double tx, double ty) {
        int x, y;

        if (tx < -1 || tx > 2 || ty < -1 || ty > 2) {
            throw new RuntimeException("This texcoord is far beyond every numerical tolerance: " + new Vect2(tx, ty));
        }

        x = (int) (tx * imageWidth);
        y = (int) (ty * imageHeight);

        if (x >= imageWidth) {
            x = imageWidth - 1;
        } else if(x < 0) {
            x = 0;
        }

        if (y >= imageHeight) {
            y = imageHeight - 1;
        } else if(y < 0) {
            y = 0;
        }

        return image.getRGB(x, y);
    }

    @Override
    public String toString() {
        return "Texture " + path + "[" + imageWidth + "x" + imageHeight + "]";
    }
}
