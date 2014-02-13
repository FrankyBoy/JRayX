using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Runtime.CompilerServices;
using JRayXLib.Util;

namespace JRayXLib.Common
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
                Storage.Add(absolutePath, ret = new Texture(absolutePath));
            }

            return ret;
        }
        #endregion

        private readonly string _path;
        private readonly Bitmap _image;

        private Texture(string path) {
            _path = path;
            _image = new Bitmap(path);
        }
    
        public uint GetColorAt(Vect2 texcoord) {
            double[] data = texcoord.GetData();
            return GetColorAt(data[0], data[1]);
        }

        public uint GetColorAt(double tx, double ty) {
            if (tx < -1 || tx > 2 || ty < -1 || ty > 2) {
                throw new Exception("This texcoord is far beyond every numerical tolerance: " + new Vect2(tx, ty));
            }

            var x = (int) (tx * _image.Width);
            var y = (int) (ty * _image.Height);

            MathHelper.Clamp(x, 0, _image.Width - 1);
            MathHelper.Clamp(y, 0, _image.Height - 1);
            
            return unchecked((uint) _image.GetPixel(x, y).ToArgb());
        }

        public new string ToString() {
            return "Texture " + _path + "[" + _image.Width + "x" + _image.Height + "]";
        }
    }
}
