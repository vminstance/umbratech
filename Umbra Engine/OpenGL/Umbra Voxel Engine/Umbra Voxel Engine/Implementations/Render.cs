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
        static public Dictionary<char, int> Characters = new Dictionary<char, int>();

        static public void Initialize()
        {
            for (int i = 32; i <= 256; i++)
            {
                string character = char.ConvertFromUtf32(i);

                Point size = Measure(character);

                Bitmap texture = new Bitmap(size.X, size.Y);

                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(texture);
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                graphics.DrawString(character, Constants.Overlay.DefaultFont, Brushes.White, Point.Empty);

                int textureID;

                RenderHelp.CreateTexture(out textureID, texture);

                Characters.Add(character[0], textureID);
            }
        }

        static public void Render(string str, Point position, Color color)
        {
            if (str == "")
            {
                return;
            }

            for (int i = 0; i < str.Length; i++)
            {
                int textureID = Characters[str[i]];

                Point positionOffset = new Point(position.X + Constants.Overlay.DefaultFontWidth * i, position.Y);
                Point size = Measure(str[i] + "");


                GL.BindTexture(TextureTarget.Texture2D, textureID);
                GL.Color4(color);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0.0F, 1.0F); GL.Vertex2(positionOffset.X, positionOffset.Y + size.Y);
                GL.TexCoord2(1.0F, 1.0F); GL.Vertex2(positionOffset.X + size.X, positionOffset.Y + size.Y);
                GL.TexCoord2(1.0F, 0.0F); GL.Vertex2(positionOffset.X + size.X, positionOffset.Y);
                GL.TexCoord2(0.0F, 0.0F); GL.Vertex2(positionOffset.X, positionOffset.Y);
                GL.End();
            }
        }

        static public Point Measure(string str)
        {
            return new Point(Constants.Overlay.DefaultFontWidth * str.Length, Constants.Overlay.DefaultFont.Height);
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

        static public void RenderTexture(int textureID, Rectangle destination)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Color4(Color.White);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0F, 1.0F); GL.Vertex2(destination.X, destination.Y + destination.Height);
            GL.TexCoord2(1.0F, 1.0F); GL.Vertex2(destination.X + destination.Width, destination.Y + destination.Height);
            GL.TexCoord2(1.0F, 0.0F); GL.Vertex2(destination.X + destination.Width, destination.Y);
            GL.TexCoord2(0.0F, 0.0F); GL.Vertex2(destination.X, destination.Y);
            GL.End();
        }

        static public void RenderTexture(int textureID, Size BitmapSize, Point destination, Rectangle source)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Color4(Color.White);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2((float)source.X / (float)BitmapSize.Width, (float)(source.Height + source.Y) / (float)BitmapSize.Height); GL.Vertex2(destination.X, destination.Y + source.Height);
            GL.TexCoord2((float)(source.Width + source.X) / (float)BitmapSize.Width, (float)(source.Height + source.Y) / (float)BitmapSize.Height); GL.Vertex2(destination.X + source.Width, destination.Y + source.Height);
            GL.TexCoord2((float)(source.Width + source.X) / (float)BitmapSize.Width, (float)source.Y / (float)BitmapSize.Height); GL.Vertex2(destination.X + source.Width, destination.Y);
            GL.TexCoord2((float)source.X / (float)BitmapSize.Width, (float)source.Y / (float)BitmapSize.Height); GL.Vertex2(destination.X, destination.Y);
            GL.End();
        }
    }
}
