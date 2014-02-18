using System.Collections.Generic;
using System.IO;
using JRayXLib.Math;
using JRayXLib.Model;
using JRayXLib.Shapes;

namespace JRayXLib.Scene.Loaders
{
    public class BinarySTLLoader : ISceneLoader
    {
        private readonly string _filePath;
        private readonly Camera _cam;

        //"models/elka.stl"
        public BinarySTLLoader(string filePath, Camera cam = null)
        {

            if (cam != null)
                _cam = cam;
            else
            {
                const double camheight = 50;
                _cam = new Camera(
                    new Vect3 { Y = camheight*2, Z = camheight },
                    new Vect3 { Y = camheight*2 - 1, Z = camheight - 1 },
                    new Vect3 { Y = Constants.InvSqurtTwo, Z = -Constants.InvSqurtTwo }, 1, 1, 1);
            }

            if(!File.Exists(filePath))
                throw new FileNotFoundException();

            _filePath = filePath;
        }

        public Scene LoadScene()
        {
            var scene = new OctreeScene();

            TriangleMeshModel model = Parse(_filePath);
            var objects = new List<I3DObject>();

            const int nx = 2;
            const int ny = 4;
            for (int i = -nx; i <= nx; i++)
                for (int j = -ny; j <= ny; j++)
                    objects.Add(new ModelInstance(new Vect3
                        {
                            X = i * 15 + (j % 2) * 7,
                            Z = (j + 4) * -15
                        }, model));

            objects.Add(_cam);

            scene.UpdateObjects(objects);

            return scene;
        }

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
