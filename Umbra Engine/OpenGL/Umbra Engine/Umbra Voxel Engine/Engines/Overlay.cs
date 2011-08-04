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
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Umbra.Implementations.Graphics;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Engines
{
    public class Overlay : Engine
    {
        //public SpriteBatch SpriteBatch { get; private set; }
        //public SpriteFont DebugFont { get; set; }
        //List<Window> Windows;

        public Overlay()
        {
        }

        public void DebugWindow(string title)
        {
            //switch (title)
            //{
            //    case "scroolWheel": Windows.Add(new GraphWindow(title, new Rectangle(100, 50, 400, 100), 10.0F, Color.Green, (GraphFunction)(() => new float[] { 
            //        Constants.Engine_Input.MouseCurrentState.ScrollWheelValue
            //    }))); break;
            //    case "position": Windows.Add(new GraphWindow(title, new Rectangle(100, 50, 400, 100), 10.0F, Color.Green, (GraphFunction)(() => new float[] { 
            //        Constants.Engine_Physics.Player.Position.X,
            //        Constants.Engine_Physics.Player.Position.Y,
            //        Constants.Engine_Physics.Player.Position.Z
            //    }))); break;
            //    case "velocity": Windows.Add(new GraphWindow(title, new Rectangle(100, 50, 400, 100), 10.0F, Color.Green, (GraphFunction)(() => new float[] { 
            //        Constants.Engine_Physics.Player.Velocity.X,
            //        Constants.Engine_Physics.Player.Velocity.Y,
            //        Constants.Engine_Physics.Player.Velocity.Z
            //    }))); break;
            //}
        }

        //public void CloseAllWindows()
        //{
        //    Windows.Clear();
        //}

        //public void GiveFocus(Window window)
        //{
        //    Windows.Remove(window);
        //    Windows.Add(window);
        //}

        public override void Initialize(EventArgs e)
        {
            //SpriteBatch = new SpriteBatch(Constants.Engine_Graphics.GraphicsDevice);

            //Windows = new List<Window>();

            //Windows.Add(new GraphWindow("Scroll wheel value", new Rectangle(100, 50, 400, 100), 10.0F, Color.Green,
            //    (GraphFunction)(() => new float[] { 
            //        Constants.Engine_Input.MouseCurrentState.ScrollWheelValue
            //    })));
            //Windows.Add(new GraphWindow("Negative scroll wheel value", new Rectangle(100, 50, 400, 100), 10.0F, Color.Green,
            //    (GraphFunction)(() => new float[] { 
            //        -Constants.Engine_Input.MouseCurrentState.ScrollWheelValue
            //    })));

            base.Initialize(e);
        }

        public override void Update(FrameEventArgs e)
        {
            Console.Update(e);
            Popup.Update(e);

            //for (int i = 0; i < Windows.Count; i++)
            //{
            //    Windows.ElementAt(i).Event_OnUpdate.Invoke(gameTime, new object[0]);
            //}
            base.Update(e);
        }

        public override void Render(FrameEventArgs e)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Constants.Graphics.ScreenResolution.X, Constants.Graphics.ScreenResolution.Y, 0, -1.0, 1.0);


            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            Crosshair.Render(e);
            Console.Render(e);
            Popup.Render(e);
            Compass.Render(e);

            if (Variables.Overlay.DisplayFPS)
            {
                SpriteString.Render(Math.Round(1.0 / e.Time, 2) + "", Console.Font, Point.Empty, Color.Yellow);
            }
            if (Variables.Game.DeveloperMode)
            {
                // Memory
                string memoryUsage = (int)(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024) + " kB";
                SpriteString.Render(memoryUsage, Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(memoryUsage, Console.Font).X - 10, 100), Color.Yellow);

                // Position
                string[] position = { 
                                        "Px: " + Math.Round(Constants.Engine_Physics.Player.Position.X, 1), 
                                        "Py: " + Math.Round(Constants.Engine_Physics.Player.Position.Y, 1), 
                                        "Pz: " + Math.Round(Constants.Engine_Physics.Player.Position.Z, 1) 
                                    };

                SpriteString.Render(position[0], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(position[0], Console.Font).X - 10, 130), Color.Yellow);
                SpriteString.Render(position[1], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(position[1], Console.Font).X - 10, 150), Color.Yellow);
                SpriteString.Render(position[2], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(position[2], Console.Font).X - 10, 170), Color.Yellow);

               
                // Velocity
                string[] velocity = { 
                                        "Vx: " + Math.Round(Constants.Engine_Physics.Player.Velocity.X, 2), 
                                        "Vy: " + Math.Round(Constants.Engine_Physics.Player.Velocity.Y, 2), 
                                        "Vz: " + Math.Round(Constants.Engine_Physics.Player.Velocity.Z, 2) 
                                    };

                SpriteString.Render(velocity[0], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(velocity[0], Console.Font).X - 10, 200), Color.Yellow);
                SpriteString.Render(velocity[1], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(velocity[1], Console.Font).X - 10, 220), Color.Yellow);
                SpriteString.Render(velocity[2], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(velocity[2], Console.Font).X - 10, 240), Color.Yellow);

                
                // Acceleration
                string[] acceleration = { 
                                            "Ax: " + Math.Round(Constants.Engine_Physics.Player.ForceAccumulator.X / Constants.Engine_Physics.Player.Mass, 2), 
                                            "Ay: " + Math.Round(Constants.Engine_Physics.Player.ForceAccumulator.Y / Constants.Engine_Physics.Player.Mass, 2), 
                                            "Az: " + Math.Round(Constants.Engine_Physics.Player.ForceAccumulator.Z / Constants.Engine_Physics.Player.Mass, 2) 
                                        };

                SpriteString.Render(acceleration[0], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(acceleration[0], Console.Font).X - 10, 270), Color.Yellow);
                SpriteString.Render(acceleration[1], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(acceleration[1], Console.Font).X - 10, 290), Color.Yellow);
                SpriteString.Render(acceleration[2], Console.Font, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(acceleration[2], Console.Font).X - 10, 310), Color.Yellow);

            }

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            base.Render(e);
        }
    }
}
