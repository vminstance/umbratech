using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
using Console = Umbra.Implementations.Console;

namespace Umbra
{
    static class Program
    {
        static public bool CodeClose = false;

        static void Main()
        {
            if (Constants.Launcher.Enabled)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Launcher Launcher = new Launcher();
                Application.Run(Launcher);
            }
            else
            {
                CodeClose = true;
            }

            if (CodeClose)
            {
                GameWindowFlags flag = Constants.Graphics.EnableFullScreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default;

                Main UmbraEngine = new Main((int)Constants.Graphics.ScreenResolution.X, (int)Constants.Graphics.ScreenResolution.Y, new GraphicsMode(), "Umbra Voxel Engine", flag);
                UmbraEngine.Run(100.0F, 60.0F);
            }
        }
    }
}