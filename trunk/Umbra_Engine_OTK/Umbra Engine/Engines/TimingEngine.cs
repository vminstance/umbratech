using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;
using Umbra_Engine;

namespace Umbra_Engine.Engines
{
    class TimingEngine
    {
        // Variables
        Game Game;
        LinkedList<IComponent> Components;
        int fps_frames;
        double fps_time;
        int tps_frames;
        double tps_time;

        int net_graph;

        public double Seconds;

        public TimingEngine(Game game)
        {
            Game = game;
            Game.Main.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            Game.Main.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            Game.Main.Resize += new EventHandler<EventArgs>(Resize);

            Components = new LinkedList<IComponent>();
        }

        void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Game.Main.Size.Width, Game.Main.Size.Height);
        }

        public void AddComponent(IComponent cmp)
        {
            Components.AddLast(cmp);
        }

        public void Update(object sender, FrameEventArgs e)
        {
            foreach (IComponent c in Components)
            {
                c.Update();
            }

            Seconds += e.Time;

            tps_time += e.Time;
            tps_frames++;
            if (tps_time >= 1)
            {
                switch (net_graph)
                {
                    case 2:
                        Console.WriteLine("TPS: {0}", tps_frames);
                        break;

                    default:
                        break;
                }
                tps_frames = 0;
                tps_time = 0;
            }
        }

        public void Net_Graph(int param)
        {
            net_graph = param;
        }

        public void Render(object sender, FrameEventArgs e)
        {
            GL.Clear(OpenGL.ClearBufferMask.ColorBufferBit);

            foreach (IComponent c in Components)
            {
                c.Render();
            }

            fps_time += e.Time;
            fps_frames++;
            if (fps_time >= 1)
            {
                switch (net_graph)
                {
                    case 1:
                        Console.WriteLine("FPS: {0}", fps_frames);
                        break;

                    case 2:
                        Console.WriteLine("FPS: {0}", fps_frames);
                        break;

                    default:
                        break;
                }
                fps_frames = 0;
                fps_time = 0;
            }

            Game.Context.SwapBuffers();
        }
    }
}
