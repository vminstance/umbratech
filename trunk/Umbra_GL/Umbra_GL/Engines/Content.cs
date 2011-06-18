using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Internal;
using Vector3 = Umbra.Structures.Internal.Vector3;
using Console = Umbra.Implementations.Console;

namespace Umbra.Engines
{
    public class Content : IEngine
    {
        string RootDirectory;



        public Content()
        {
            RootDirectory = Constants.ContentRootDirectory;
        }

        public uint LoadTexture(string file)
        {
            Bitmap bitmap = new Bitmap(file);
            if (!IsPowerOf2(bitmap))
            {
                // XXX: FormatException isn't really the best here, buuuut...
                throw new FormatException("Texture sizes must be powers of 2!");
            }

            uint texture;
            GL.Hint(OpenGL.HintTarget.PerspectiveCorrectionHint, OpenGL.HintMode.Nicest);

            GL.GenTextures(1, out texture);
            GL.BindTexture(OpenGL.TextureTarget.Texture2D, texture);

            GL.TexEnv(OpenGL.TextureEnvTarget.TextureEnv, OpenGL.TextureEnvParameter.TextureEnvMode, 3);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(OpenGL.TextureTarget.Texture2D, 0, OpenGL.PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, OpenGL.PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(OpenGL.TextureTarget.Texture2D, OpenGL.TextureParameterName.TextureMinFilter, (int)OpenGL.TextureMinFilter.Linear);
            GL.TexParameter(OpenGL.TextureTarget.Texture2D, OpenGL.TextureParameterName.TextureMagFilter, (int)OpenGL.TextureMagFilter.Nearest);

            return texture;
        }

        private bool IsPowerOf2(Bitmap b)
        {
            int w = b.Width;
            int h = b.Height;

            if (w != h)
                return false;

            if ((double)((int)Math.Log(w, 2)) != Math.Log(w, 2))
                return false;

            return true;
        }
    }
}
