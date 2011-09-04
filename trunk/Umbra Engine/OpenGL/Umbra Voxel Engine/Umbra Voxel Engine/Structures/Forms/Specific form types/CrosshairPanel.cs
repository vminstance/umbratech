using System;
using System.Linq;
using System.Text;
using System.Drawing;
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
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures.Forms
{
    class CrosshairPanel : Panel
    {
        private int TextureID;
        private int Width;
        private int Height;

        public CrosshairPanel()
        {
            Bitmap Texture = (Bitmap)Content.Load(Constants.Content.Textures.CrosshairFilename);

            RenderHelp.CreateTexture2D(out TextureID, Texture);
            
            Width = Texture.Width;
            Height = Texture.Height;
        }

        public void Render(Rectangle clientFrame)
        {
            RenderHelp.RenderTexture(TextureID, new Rectangle(clientFrame.X - Width / 2, clientFrame.Y - Height / 2, Width, Height));
        }
    }
}
