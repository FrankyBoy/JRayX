using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using JRayXLib.Util;
using Color = JRayXLib.Colors.Color;

namespace JRayXLib.Shapes
{
    public class Texture
    {
        #region loader
        private static readonly Dictionary<string, Texture> Storage = new Dictionary<string, Texture>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Texture Load(string path)
        {
            var absolutePath = Path.GetFullPath(path);
            
            Texture ret;

            if (!Storage.TryGetValue(absolutePath, out ret))
            {
                var bitmap = new Bitmap(path);

                Storage.Add(absolutePath, ret = new Texture(Convert(bitmap)));
            }

            return ret;
        }

        private static Color[,] Convert(Bitmap bmp)
        {
            var result = new Color[bmp.Width,bmp.Height];

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    var px = bmp.GetPixel(j, i);
                    result[i, j].A = px.A;
                    result[i, j].R = px.R;
                    result[i, j].G = px.G;
                    result[i, j].B = px.B;
                }
            }
            return result;
        }

        #endregion

        private readonly Color[,] _data;
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Texture(Color[,] data)
        {
            _data = data;
            Width = _data.GetLength(0);
            Height = data.GetLength(1);
        }

        public Color GetColorAt(Vect2 texcoord)
        {
            double[] data = texcoord.GetData();
            return GetColorAt(data[0], data[1]);
        }

        public Color GetColorAt(double tx, double ty)
        {
            if (tx < -1 || tx > 2 || ty < -1 || ty > 2) {
                throw new Exception("This texcoord is far beyond every numerical tolerance: " + new Vect2(tx, ty));
            }

            var x = (int) (tx * Width);
            var y = (int) (ty * Height);

            MathHelper.Clamp(x, 0, Width - 1);
            MathHelper.Clamp(y, 0, Height - 1);
            return _data[x, y];
        }

        public new string ToString() {
            return "Texture [" + Width + "x" + Height + "]";
        }

        public Color this[int x, int y]
        {
            get { return _data[x, y]; }
            set { _data[x, y] = value; }
        }
    }
}
