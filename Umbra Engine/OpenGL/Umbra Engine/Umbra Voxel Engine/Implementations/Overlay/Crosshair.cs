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

namespace Umbra.Implementations.Graphics
{
    static public class Crosshair
    {
        static private int TextureID;
        static private Bitmap Texture;

        static public void Initialize()
        {
            Texture = (Bitmap)Content.Load("content/crosshair.png");

            RenderHelp.CreateTexture(out TextureID, Texture);
        }

        static public void Render(FrameEventArgs e)
        {
            RenderHelp.RenderTexture(TextureID, new Rectangle((int)(Constants.Graphics.ScreenResolution.X - Texture.Width) / 2, (int)(Constants.Graphics.ScreenResolution.Y - Texture.Height) / 2, Texture.Width, Texture.Height));
        }
    }
}
