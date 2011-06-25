using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra.Engines
{
    public class Input : DrawableGameComponent
    {
        public KeyboardState KeyboardCurrentState { get; private set; }
        public KeyboardState KeyboardLastState { get; private set; }
        public MouseState MouseCurrentState { get; private set; }
        public MouseState MouseLastState { get; private set; }

        bool IsResizingConsole = false;
        Point ResizePosition;

        public Input(Main main)
            : base(main)
        {

        }
        public override void Initialize()
        {
            EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
            base.Initialize();
        }

        void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {
            if (!Console.IsActive)
            {
                return;
            }

            if (e.Character == '\b')
            {
                Console.InputString = Console.InputString.Substring(0, Math.Max(Console.InputString.Length - 1, 0));
                Console.CursorPosition--;
            }
            else if (e.Character == '\r')
            {
                if (Console.InputString != null && Console.InputString != "")
                {
                    if (Console.InputString[0] == '/')
                    {
                        Console.ExecuteCurrentInput();
                    }
                    else
                    {
                        Console.WriteCurrentInput();
                    }
                }
                Console.InputString = "";
                Console.CursorPosition = 0;
                if (!(Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.LeftShift) || Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.RightShift)))
                {
                    Console.Close();
                }
            }
            else if (e.Character == (char)27)
            {
                Console.Toggle();
                Console.CursorPosition = 0;
            }
            else if (e.Character == '\t')
            {
                Console.InputString = "    ";
                Console.CursorPosition += 4;
            }
            else
            {
                Console.InputString = Console.InputString + e.Character;
                Console.CursorPosition++;
            }

            Console.CursorPosition = Math.Max(Console.CursorPosition, 0);
            Console.CursorPosition = Math.Min(Console.CursorPosition, Console.InputString.Length);
        }


        public override void Update(GameTime gameTime)
        {

            KeyboardLastState = KeyboardCurrentState;
            MouseLastState = MouseCurrentState;

            KeyboardCurrentState = Keyboard.GetState();
            MouseCurrentState = Mouse.GetState();

            if (Console.IsActive)
            {
                //----------------
                // Console writing 
                //----------------

                if (KeyboardCurrentState.IsKeyDown(Keys.Escape) && KeyboardCurrentState.IsKeyUp(Keys.Escape))
                {
                    Console.Toggle();
                }

                Game.IsMouseVisible = true;

                if (MouseCurrentState.LeftButton == ButtonState.Pressed)
                {
                    if (IsResizingConsole)
                    {
                        Variables.Overlay.Console.Area.Width = Constants.Overlay.Console.DefaultArea.Width + (MouseCurrentState.X - ResizePosition.X);
                        Variables.Overlay.Console.Area.Y = Variables.Overlay.Console.Area.Y + (MouseCurrentState.Y - ResizePosition.Y);
                        Variables.Overlay.Console.Area.Height = Variables.Overlay.Console.Area.Height - (MouseCurrentState.Y - ResizePosition.Y);

                        Variables.Overlay.Console.Area.Width = (int)Math.Max(Console.Font.MeasureString("12345").X + 30, Variables.Overlay.Console.Area.Width);
                        Variables.Overlay.Console.Area.Y = (int)Math.Min(Constants.Graphics.ScreenResolution.Y - 50, Variables.Overlay.Console.Area.Y);
                    }

                    if (MouseCurrentState.X < Variables.Overlay.Console.Area.Width && MouseCurrentState.X > Variables.Overlay.Console.Area.Width - 5 && MouseCurrentState.Y < Variables.Overlay.Console.Area.Y + 5 && MouseCurrentState.Y > Variables.Overlay.Console.Area.Y)
                    {
                        ResizePosition = new Point(MouseCurrentState.X, MouseCurrentState.Y);
                        IsResizingConsole = true;
                    }
                }

                if (MouseCurrentState.LeftButton == ButtonState.Released)
                {
                    IsResizingConsole = false;
                    ResizePosition = Point.Zero;
                    Constants.Overlay.Console.DefaultArea = Variables.Overlay.Console.Area;
                }
            }
            else if (Variables.Game.IsActive)
            {
                //----------------
                // Normal 
                //----------------

                if (KeyboardCurrentState.IsKeyDown(Keys.T) && Console.IsClosed())
                {
                    Console.Toggle();
                }

                if (KeyboardCurrentState.IsKeyDown(Keys.Escape) && KeyboardLastState.IsKeyUp(Keys.Escape))
                {
                    Console.Execute("/exit");
                }

                if (KeyboardLastState.IsKeyUp(Keys.Q) && KeyboardCurrentState.IsKeyDown(Keys.Q))
                {
                    Console.Execute("/noclip");
                }

                if (KeyboardLastState.IsKeyUp(Keys.F) && KeyboardCurrentState.IsKeyDown(Keys.F))
                {
                    Console.Execute("/flashlight");
                }

                Game.IsMouseVisible = false;
            }

            base.Update(gameTime);
        }

        public Vector3 NoclipDirection()
        {
            Vector3 returnVector = new Vector3((Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.D) ? 1 : 0) - (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.A) ? 1 : 0),
                (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.Space) ? 1 : 0) - (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.LeftShift) ? 1 : 0),
                (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.S) ? 1 : 0) - (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.W) ? 1 : 0));

            if (returnVector == Vector3.Zero)
            {
                return Vector3.Zero;
            }

            return Vector3.Normalize(returnVector);
        }

        public Vector3 WalkingDirection()
        {
            Vector3 returnVector = new Vector3((Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.D) ? 1 : 0) - (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.A) ? 1 : 0),
                0,
                (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.S) ? 1 : 0) - (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.W) ? 1 : 0));

            if (returnVector == Vector3.Zero)
            {
                return Vector3.Zero;
            }

            return Vector3.Normalize(returnVector);
        }

        public bool IsRunning()
        {
            return KeyboardCurrentState.IsKeyDown(Keys.LeftShift) && !Console.IsActive;
        }

        public bool ShouldJump()
        {
            if (KeyboardLastState.IsKeyDown(Keys.Space) && KeyboardCurrentState.IsKeyDown(Keys.Space))
            {
                return true;
            }

            return false;
        }

        public void ResetMouse()
        {
            Mouse.SetPosition((int)Constants.Graphics.ScreenResolution.X / 2, (int)Constants.Graphics.ScreenResolution.Y / 2);

            KeyboardLastState = KeyboardCurrentState;
            MouseLastState = MouseCurrentState;

            KeyboardCurrentState = Keyboard.GetState();
            MouseCurrentState = Mouse.GetState();
        }
    }
}
