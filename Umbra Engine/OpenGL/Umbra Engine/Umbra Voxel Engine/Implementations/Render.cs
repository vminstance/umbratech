using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Graphics;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Implementations
{
    static public class SpriteString
    {
        static public void Render(string str, Font font, Point position, Color color)
        {
            if (str == "")
            {
                throw new Exception("String cannot be empty!");
            }

            Point size = Measure(str, font);

            Bitmap texture = new Bitmap(size.X, size.Y);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(texture);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(str, font, Brushes.White, Point.Empty);

            int textureID;

            RenderHelp.CreateTexture(out textureID, texture);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Color4(color);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0F, 1.0F); GL.Vertex2(position.X, position.Y + size.Y);
            GL.TexCoord2(1.0F, 1.0F); GL.Vertex2(position.X + size.X, position.Y + size.Y);
            GL.TexCoord2(1.0F, 0.0F); GL.Vertex2(position.X + size.X, position.Y);
            GL.TexCoord2(0.0F, 0.0F); GL.Vertex2(position.X, position.Y);
            GL.End();

            RenderHelp.DeleteTexture(textureID);
        }

        static public Point Measure(string str, Font font)
        {
            Size size = System.Windows.Forms.TextRenderer.MeasureText(str, font);
            return new Point(size.Width, size.Height);
        }
    }

    static public class RenderHelp
    {
        static public void CreateTexture(out int textureID, string bitmapName)
        {
            CreateTexture(out textureID, (Bitmap)Content.Load(bitmapName));
        }

        static public void CreateTexture(out int textureID, Bitmap texture)
        {
            GL.GenTextures(1, out textureID);

            GL.BindTexture(TextureTarget.Texture2D, textureID);

            BitmapData bmpData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            texture.UnlockBits(bmpData);

            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, Color.Transparent);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
        }

        static public void DeleteTexture(int textureID)
        {
            GL.DeleteTextures(1, ref textureID);
        }

        static public void BindTexture(int textureId, TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Uniform1(Shaders.TextureID, TextureUnit.Texture0 - textureUnit);
        }

        static public void RenderTexture(int textureID, Rectangle rect)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Color4(Color.White);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0F, 1.0F); GL.Vertex2(rect.X, rect.Y + rect.Height);
            GL.TexCoord2(1.0F, 1.0F); GL.Vertex2(rect.X + rect.Width, rect.Y + rect.Height);
            GL.TexCoord2(1.0F, 0.0F); GL.Vertex2(rect.X + rect.Width, rect.Y);
            GL.TexCoord2(0.0F, 0.0F); GL.Vertex2(rect.X, rect.Y);
            GL.End();
        }
    }
}
