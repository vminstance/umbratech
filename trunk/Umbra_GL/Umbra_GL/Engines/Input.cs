using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenGL = OpenTK.Graphics.OpenGL;
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
    public class Input : IEngine
    {
        public KeyboardState KeyboardCurrentState { get; private set; }
        public KeyboardState KeyboardLastState { get; private set; }
        public MouseState MouseCurrentState { get; private set; }
        public MouseState MouseLastState { get; private set; }

        bool IsResizingConsole = false;
        Point ResizePosition;

        public Input()
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
                    Console.ExecuteCurrentInput();
                }
                Console.InputString = "";
                Console.CursorPosition = 0;
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
                        Constants.ConsoleArea.Width = Constants.DefaultConsoleArea.Width + (MouseCurrentState.X - ResizePosition.X);
                        Constants.ConsoleArea.Y = Constants.DefaultConsoleArea.Y + (MouseCurrentState.Y - ResizePosition.Y);
                        Constants.ConsoleArea.Height = Constants.DefaultConsoleArea.Height - (MouseCurrentState.Y - ResizePosition.Y);

                        Constants.ConsoleArea.Width = (int)Math.Max(Console.Font.MeasureString("12345").X + 30, Constants.ConsoleArea.Width);
                        Constants.ConsoleArea.Y = (int)Math.Min(Constants.ScreenResolution.Y - 50, Constants.ConsoleArea.Y);
                    }

                    if (MouseCurrentState.X < Constants.ConsoleArea.Width && MouseCurrentState.X > Constants.ConsoleArea.Width - 5 && MouseCurrentState.Y < Constants.ConsoleArea.Y + 5 && MouseCurrentState.Y > Constants.ConsoleArea.Y)
                    {
                        ResizePosition = new Point(MouseCurrentState.X, MouseCurrentState.Y);
                        IsResizingConsole = true;
                    }
                }

                if (MouseCurrentState.LeftButton == ButtonState.Released)
                {
                    IsResizingConsole = false;
                    ResizePosition = Point.Zero;
                    Constants.DefaultConsoleArea = Constants.ConsoleArea;
                }
            }
            else if(Constants.GameIsActive)
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
                    Console.Execute("exit");
                }

                if (KeyboardLastState.IsKeyUp(Keys.Q) && KeyboardCurrentState.IsKeyDown(Keys.Q))
                {
                    Console.Execute("noclip");
                }

                Game.IsMouseVisible = false;
            }

            KeyboardLastState = KeyboardCurrentState;
            MouseLastState = MouseCurrentState;

            base.Update(gameTime);
        }

        public Vector3 NoclipDirection()
        {
            Vector3 returnVector = new Vector3((Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.D) ? 1 : 0) - (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.A) ? 1 : 0),
                (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.Space) ? 1 : 0) - (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.LeftShift) ? 1 : 0),
                (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.S) ? 1 : 0) - (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.W) ? 1 : 0));

            if (returnVector == Vector3.Zero)
            {
                return Vector3.Zero;
            }

            return Vector3.Normalize(returnVector);
        }

        public Vector3 WalkingDirection()
        {
            Vector3 returnVector = new Vector3((Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.D) ? 1 : 0) - (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.A) ? 1 : 0),
                0,
                (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.S) ? 1 : 0) - (Constants.Input.KeyboardCurrentState.IsKeyDown(Keys.W) ? 1 : 0));

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
            Mouse.SetPosition((int)Constants.ScreenResolution.X / 2, (int)Constants.ScreenResolution.Y / 2);

            KeyboardCurrentState = Keyboard.GetState();
            MouseCurrentState = Mouse.GetState();

            KeyboardLastState = KeyboardCurrentState;
            MouseLastState = MouseCurrentState;
        }
    }
}
