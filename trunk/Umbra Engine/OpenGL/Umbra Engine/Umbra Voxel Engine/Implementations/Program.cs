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
using Console = Umbra.Implementations.Graphics.Console;

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
                Main UmbraEngine;

                if (Constants.Graphics.EnableFullScreen)
                {
                    Constants.Graphics.ScreenResolution = new Vector2(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                    UmbraEngine = new Main(new GraphicsMode(), "Umbra Voxel Engine", GameWindowFlags.Fullscreen);
                }
                else
                {
                    UmbraEngine = new Main(new GraphicsMode(), "Umbra Voxel Engine", GameWindowFlags.Default);
                }

                UmbraEngine.Run(100.0, 60.0);
            }
        }
    }
}