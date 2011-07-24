﻿using System;
using System.Linq;
using System.Text;
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

namespace Umbra.Engines
{
    public class Main : GameWindow
    {
        List<Engine> Engines;

        public Main(int width, int height, GraphicsMode mode, string title, GameWindowFlags flags) : base(width, height, mode, title, flags)
        {
            Engines = new List<Engine>();
            Constants.SetupEngines(this);
        }

        public void AddEngine(Engine engine)
        {
            Engines.Add(engine);
            engine.SetGame(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            foreach (Engine engine in Engines)
            {
                engine.Initialize(e);
            }

            Variables.Game.IsInitialized = true;
            Variables.Game.IsActive = true;
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            foreach (Engine engine in Engines)
            {
                engine.Update(e);
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Variables.Graphics.ScreenClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (Engine engine in Engines)
            {
                engine.Render(e);
            }

            SwapBuffers();

            base.OnRenderFrame(e);
        }
    }
}
