package jray.model;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.IntBuffer;
import java.nio.channels.FileChannel;

public class BinarySTLParser {
	
	/**
	 *  Format (everything little endian): 
	 *  
	 * 	UINT8[80] – Header
	 *	UINT32 – Number of triangles
	 *	
	 *	foreach triangle
	 *	REAL32[3] – Normal vector
	 *	REAL32[3] – Vertex 1
	 *	REAL32[3] – Vertex 2
	 *	REAL32[3] – Vertex 3
	 *	UINT16 – Attribute byte count
	 *	end
	 *  
	 * @param f
	 * @return
	 * @throws IOException
	 */
	public static TriangleMeshModel parse(File f) throws IOException{
		FileInputStream fis = new FileInputStream(f);
		FileChannel inChannel = fis.getChannel();
	    
		ByteBuffer buf = ByteBuffer.allocate(4);
		buf.order(ByteOrder.LITTLE_ENDIAN);
	    inChannel.position(80);
		
		if(inChannel.read(buf)!=4)
			throw new RuntimeException("could not read triangle count!");
		
		buf.rewind();
		IntBuffer ibuf = buf.asIntBuffer();
		int triangleCount = ibuf.get();
		float[] triangleEdgeData = new float[12*triangleCount];
		buf = ByteBuffer.allocateDirect(50*triangleCount);
		buf.order(ByteOrder.LITTLE_ENDIAN);
		if(inChannel.read(buf)!=50*triangleCount)
			throw new RuntimeException("could not read "+(50*triangleCount)+"bytes!");
		
		for(int i=0;i<triangleCount;i++){
			buf.position(50*i);
			buf.asFloatBuffer().get(triangleEdgeData, i*12, 12);
		}
		
		fis.close();
		
		return new TriangleMeshModel(triangleEdgeData);
	}
}
