using System;
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
            //SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //SpriteBatch.Draw(Constants.Engine_Content.CrosshairTexture, new Vector2((Constants.Graphics.ScreenResolution.X - Constants.Engine_Content.CrosshairTexture.Width) / 2, (Constants.Graphics.ScreenResolution.Y - Constants.Engine_Content.CrosshairTexture.Height) / 2), Color.White);
            Console.Draw(e);
            //Popup.Draw();
            //Compass.Draw();
            //if (Variables.Overlay.DisplayFPS)
            //{
            //    SpriteBatch.DrawString(DebugFont, Convert.ToString(1000f / (float)gameTime.ElapsedGameTime.Milliseconds), new Vector2(10, 5), Color.Yellow);
            //}
            //if (Variables.Game.DeveloperMode)
            //{
            //    // Memory
            //    string memoryUsage = (int)(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024) + " kB";
            //    SpriteBatch.DrawString(DebugFont, memoryUsage, new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(memoryUsage).X - 10, 100), Color.Yellow);

            //    // Position
            //    string[] position = { 
            //                            "Px: " + Math.Round(Constants.Engine_Physics.Player.Position.X, 1), 
            //                            "Py: " + Math.Round(Constants.Engine_Physics.Player.Position.Y, 1), 
            //                            "Pz: " + Math.Round(Constants.Engine_Physics.Player.Position.Z, 1) 
            //                        };

            //    SpriteBatch.DrawString(DebugFont, position[0], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(position[0]).X - 10, 130), Color.Yellow);
            //    SpriteBatch.DrawString(DebugFont, position[1], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(position[1]).X - 10, 150), Color.Yellow);
            //    SpriteBatch.DrawString(DebugFont, position[2], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(position[2]).X - 10, 170), Color.Yellow);

            //    // Acceleration
            //    string[] acceleration = { 
            //                                "Ax: " + Math.Round(Constants.Engine_Physics.Player.ForceAccumulator.X / Constants.Engine_Physics.Player.Mass, 2), 
            //                                "Ay: " + Math.Round(Constants.Engine_Physics.Player.ForceAccumulator.Y / Constants.Engine_Physics.Player.Mass, 2), 
            //                                "Az: " + Math.Round(Constants.Engine_Physics.Player.ForceAccumulator.Z / Constants.Engine_Physics.Player.Mass, 2) 
            //                            };


            //    SpriteBatch.DrawString(DebugFont, acceleration[0], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(acceleration[0]).X - 10, 270), Color.Yellow);
            //    SpriteBatch.DrawString(DebugFont, acceleration[1], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(acceleration[1]).X - 10, 290), Color.Yellow);
            //    SpriteBatch.DrawString(DebugFont, acceleration[2], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(acceleration[2]).X - 10, 310), Color.Yellow);


            //    // Velocity
            //    string[] velocity = { 
            //                            "Vx: " + Math.Round(Constants.Engine_Physics.Player.Velocity.X, 2), 
            //                            "Vy: " + Math.Round(Constants.Engine_Physics.Player.Velocity.Y, 2), 
            //                            "Vz: " + Math.Round(Constants.Engine_Physics.Player.Velocity.Z, 2) 
            //                        };

            //    SpriteBatch.DrawString(DebugFont, velocity[0], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(velocity[0]).X - 10, 200), Color.Yellow);
            //    SpriteBatch.DrawString(DebugFont, velocity[1], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(velocity[1]).X - 10, 220), Color.Yellow);
            //    SpriteBatch.DrawString(DebugFont, velocity[2], new Vector2(Constants.Graphics.ScreenResolution.X - DebugFont.MeasureString(velocity[2]).X - 10, 240), Color.Yellow);
            //}

            //for (int i = 0; i < Windows.Count; i++)
            //{
            //    Windows.ElementAt(i).Event_OnPaint.Invoke(gameTime, new object[] { SpriteBatch });
            //}

            //SpriteBatch.End();
            base.Render(e);
        }
    }
}
