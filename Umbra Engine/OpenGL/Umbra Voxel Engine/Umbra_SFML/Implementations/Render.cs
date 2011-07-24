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
using Console = Umbra.Implementations.Console;

namespace Umbra.Implementations
{
    static public class Render
    {
        static public void RenderString(SpriteString stringTex, Point position, Color color)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            GL.BindTexture(TextureTarget.Texture2D, stringTex.TextureID);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0F, 0.0F); GL.Vertex2(-1.0F, -1.0F);
            GL.TexCoord2(1.0F, 0.0F); GL.Vertex2(-0.5F, -1.0F);
            GL.TexCoord2(1.0F, 1.0F); GL.Vertex2(-0.5F, 0.0F);
            GL.TexCoord2(0.0F, 1.0F); GL.Vertex2(-1.0F, 0.0F);
            GL.End();
        }

        //    static public void RenderString(Font font, string str, Point position, Color color)
        //    {
        //        if (str == "")
        //        {
        //            return;
        //        }

        //        //Bitmap texture = new Bitmap(font.Height * str.Length, font.Height);
        //        //System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(texture);

        //        //graphics.DrawString(str, font, Brushes.Violet, Point.Empty);

        //        Bitmap texture = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/test.png");

        //        int textureID = GL.GenTexture();
        //        GL.BindTexture(TextureTarget.Texture2D, textureID);

        //        System.Drawing.Imaging.BitmapData data = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);



        //        GL.Enable(EnableCap.Texture2D);
        //        GL.Enable(EnableCap.Blend);
        //        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

        //        GL.Begin(BeginMode.Quads);
        //        GL.TexCoord2(0.0F, 0.0F); GL.Vertex2(-1.0F, -1.0F);
        //        GL.TexCoord2(1.0F, 0.0F); GL.Vertex2(-0.5F, -1.0F);
        //        GL.TexCoord2(1.0F, 1.0F); GL.Vertex2(-0.5F, 0.0F);
        //        GL.TexCoord2(0.0F, 1.0F); GL.Vertex2(-1.0F, 0.0F);
        //        GL.End();
        //    }
        //}
    }

    public class SpriteString
    {
        public int TextureID{get; private set;}

        public SpriteString(Font font, string str, Color color)
        {
            if (str == "")
            {
                throw new Exception("String cannot be empty!");
            }

            //Bitmap texture = new Bitmap(font.Height * str.Length, font.Height);
            //System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(texture);

            //graphics.DrawString(str, font, Brushes.Red, Point.Empty);

            //Bitmap texture = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/test.png");

            //BitmapData data = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            //TextureID = GL.GenTexture();
            //GL.BindTexture(TextureTarget.Texture2D, TextureID);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
        }
    }

    static public class RenderHelp
    {
        static public void CreateTexture(out int textureID, string bitmapName)
        {
            GL.GenTextures(1, out textureID);

            GL.BindTexture(TextureTarget.Texture2D, textureID);


            Bitmap texture = (Bitmap)Content.Load(bitmapName);
            BitmapData bmpData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            texture.UnlockBits(bmpData);

            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, Color.White);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinLod, -1000);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLod, 1000);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        static public void BindTexture(int textureId, TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Uniform1(Shaders.TextureID, TextureUnit.Texture0 - textureUnit);
        }
    }
}
