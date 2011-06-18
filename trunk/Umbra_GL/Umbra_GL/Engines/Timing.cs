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
    public class Timing
    {
        // Variables
        LinkedList<IEngine> Engines;
        int fps_frames;
        double fps_time;
        int tps_frames;
        double tps_time;

        int net_graph;

        public double TotalMilliseconds;

        public Timing()
        {
            Constants.GameWindow.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            Constants.GameWindow.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            Constants.GameWindow.Resize += new EventHandler<EventArgs>(Resize);

            Engines = new LinkedList<IEngine>();
        }

        void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Constants.GameWindow.Size.Width, Constants.GameWindow.Size.Height);
        }

        public void AddEngine(IEngine engine)
        {
            Engines.AddLast(engine);
        }

        public void Update(object sender, FrameEventArgs e)
        {
            foreach (IEngine engine in Engines)
            {
                engine.Update(new GameTime(TotalMilliseconds, e.Time * 1000.0D));
            }

            TotalMilliseconds += e.Time * 1000.0D;
        }

        public void Net_Graph(int param)
        {
            net_graph = param;
        }

        public void Render(object sender, FrameEventArgs e)
        {
            GL.Clear(OpenGL.ClearBufferMask.ColorBufferBit);

            foreach (IEngine engine in Engines)
            {
                engine.Render(new GameTime(TotalMilliseconds, e.Time * 1000.0D));
            }

            Constants.Main.Context.SwapBuffers();
        }
    }
}
