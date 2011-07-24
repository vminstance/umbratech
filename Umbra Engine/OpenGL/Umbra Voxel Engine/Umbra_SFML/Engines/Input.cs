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
using Console = Umbra.Implementations.Console;

namespace Umbra.Engines
{
    public class Input : Engine
    {

        KeyboardDevice Keyboard { get; set; }
        MouseDevice Mouse { get; set; }
        Point MouseDelta { get; set; }

        public override void Initialize(EventArgs e)
        {
            Keyboard = Main.Keyboard;
            Mouse = Main.Mouse;

            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyboardEvent);
            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButton);

            System.Windows.Forms.Cursor.Position = Main.PointToScreen(new Point((Main.ClientSize.Width / 2), (Main.ClientSize.Height / 2)));

            base.Initialize(e);
        }

        public void SetMouseShow(bool show)
        {
            if (show)
            {
                System.Windows.Forms.Cursor.Show();
            }
            else
            {
                System.Windows.Forms.Cursor.Hide();
            }
        }

        void MouseMove()
        {
            if (Variables.Game.IsActive)
            {
                MouseDelta = new Point(Mouse.X - (Main.ClientSize.Width / 2), Mouse.Y - (Main.ClientSize.Height / 2));
                System.Windows.Forms.Cursor.Position = Main.PointToScreen(new Point((Main.ClientSize.Width / 2), (Main.ClientSize.Height / 2)));

                Constants.Engine_Physics.Player.FirstPersonCamera.UpdateMouse(MouseDelta);
            }
        }

        void MouseButton(object sender, MouseButtonEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }

            if (Variables.Game.IsActive)
            {
                Constants.Engine_Physics.Player.UpdateMouse(e);
            }
        }

        void KeyboardEvent(object sender, KeyboardKeyEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }

            if (Variables.Game.IsActive)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        {
                            Console.Execute("exit");
                        }
                        break;

                    case Key.T:
                        {
                            Console.Toggle();
                        }
                        break;

                    case Key.Q:
                        {
                            Console.Execute("noclip");
                        }
                        break;

                    case Key.F:
                        {
                            Console.Execute("flashlight");
                        }
                        break;
                }
            }
            else
            {
                Console.Input(e);
            }
        }


        public override void Update(FrameEventArgs e)
        {
            MouseMove();
            Constants.Engine_Physics.Player.UpdateKeyboard(Keyboard);


            if (!Variables.Game.IsActive)
            {
                //----------------
                // Console writing 
                //----------------

                //if (MouseCurrentState.LeftButton == ButtonState.Pressed)
                //{
                //    if (IsResizingConsole)
                //    {
                //        Variables.Overlay.Console.Area.Width = Constants.Overlay.Console.DefaultArea.Width + (MouseCurrentState.X - ResizePosition.X);
                //        Variables.Overlay.Console.Area.Y = Variables.Overlay.Console.Area.Y + (MouseCurrentState.Y - ResizePosition.Y);
                //        Variables.Overlay.Console.Area.Height = Variables.Overlay.Console.Area.Height - (MouseCurrentState.Y - ResizePosition.Y);

                //        Variables.Overlay.Console.Area.Width = (int)Math.Max(Console.Font.MeasureString("12345").X + 30, Variables.Overlay.Console.Area.Width);
                //        Variables.Overlay.Console.Area.Y = (int)Math.Min(Constants.Graphics.ScreenResolution.Y - 50, Variables.Overlay.Console.Area.Y);
                //    }

                //    if (MouseCurrentState.X < Variables.Overlay.Console.Area.Width && MouseCurrentState.X > Variables.Overlay.Console.Area.Width - 5 && MouseCurrentState.Y < Variables.Overlay.Console.Area.Y + 5 && MouseCurrentState.Y > Variables.Overlay.Console.Area.Y)
                //    {
                //        ResizePosition = new Point(MouseCurrentState.X, MouseCurrentState.Y);
                //        IsResizingConsole = true;
                //    }
                //}

                //if (MouseCurrentState.LeftButton == ButtonState.Released)
                //{
                //    IsResizingConsole = false;
                //    ResizePosition = Point.Zero;
                //    Constants.Overlay.Console.DefaultArea = Variables.Overlay.Console.Area;
                //}
            }
            else if (Variables.Game.IsActive && Main.Focused)
            {
                //----------------
                // Normal 
                //----------------

                //if (KeyboardCurrentState.IsKeyDown(Keys.T) && Console.IsClosed())
                //{
                //    Console.Toggle();
                //}

                //if (KeyboardCurrentState.IsKeyDown(Keys.Escape) && KeyboardLastState.IsKeyUp(Keys.Escape))
                //{
                //    Console.Execute("/exit");
                //}

                //if (KeyboardLastState.IsKeyUp(Keys.Q) && KeyboardCurrentState.IsKeyDown(Keys.Q))
                //{
                //    Console.Execute("/noclip");
                //}

                //if (KeyboardLastState.IsKeyUp(Keys.F) && KeyboardCurrentState.IsKeyDown(Keys.F))
                //{
                //    Console.Execute("/flashlight");
                //}

                //Game.IsMouseVisible = false;
            }

            base.Update(e);
        }
    }
}
