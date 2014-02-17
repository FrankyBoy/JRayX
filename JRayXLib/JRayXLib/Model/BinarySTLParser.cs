using System.Collections.Generic;
using System.IO;
using JRayXLib.Shapes;

namespace JRayXLib.Model
{
    public class BinarySTLParser
    {
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

        public static TriangleMeshModel Parse(string f)
        {
            using (var reader = new BinaryReader(new FileStream(f, FileMode.Open, FileAccess.Read)))
            {
                reader.ReadBytes(80); // skipping header

                int triangleCount = reader.ReadInt32();

                var triangleEdgeData = new List<I3DObject>(triangleCount);

                for (int i = 0; i < triangleCount; i++)
                {
                    triangleEdgeData[i] = new MinimalTriangle(
                        new Vect3
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Z = reader.ReadSingle()
                        },
                        new Vect3
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Z = reader.ReadSingle()
                        },
                        new Vect3
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Z = reader.ReadSingle()
                        },
                        new Vect3
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Z = reader.ReadSingle()
                        }
                        );
                    reader.ReadUInt16(); // skip the 2 attribute bytes
                }

                return new TriangleMeshModel(triangleEdgeData);
            }
        }
    }
}