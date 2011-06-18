using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
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
    public class Main : IEngine
    {
        //Variables
        public GameWindow GameWindow;

        public Main()
        {
            GameWindow = new GameWindow((int)Constants.ScreenResolution.X, (int)Constants.ScreenResolution.Y, GraphicsMode.Default, "Umbra Engine", GameWindowFlags.Default, DisplayDevice.Default);
            GameWindow.MakeCurrent();
            Constants.SetupEngines(this);
        }

        public void Run()
        {
            GameWindow.Run(100, 60);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Render(GameTime gameTime)
        {
        }
    }
}
