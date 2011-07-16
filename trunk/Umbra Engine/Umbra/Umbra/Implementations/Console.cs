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

namespace Umbra.Implementations
{
    static public class Console
    {
        static List<ConsoleMessage> Buffer = new List<ConsoleMessage>();
        static public SpriteFont Font { get; set; }
        static public bool IsActive { get; private set; }

        static ConsoleState State = ConsoleState.Closed;
        static int StateCounter = Constants.Overlay.Console.FadeSpeed;

        static double LastTimeStamp = 0;

        static public string InputString { get; set; }
        static public int CursorPosition = 0;

        static int MessageQuantity = 19;

        static public bool IsClosed()
        {
            return State == ConsoleState.Closed;
        }

        static public void Clear()
        {
            Buffer = new List<ConsoleMessage>();
        }

        static public void Write(string message)
        {
            Buffer.Add(new ConsoleMessage(message, LastTimeStamp, Color.White));
        }

        static public void Execute(string inputString)
        {
            string[] args;
            string command = FormatInput(inputString.Substring(1), out args); // Use substring to remove the slash

            bool showCommand = true;

            if (ConsoleFunctions.ConsoleCommands.ContainsKey(command))
            {
                showCommand = ((ConsoleFunction)ConsoleFunctions.ConsoleCommands[command]).Invoke(command, args, inputString);
            }

            if (showCommand)
            {
                Buffer.Add(new ConsoleMessage(command, LastTimeStamp, Color.White));
            }
        }

        static public void ExecuteCurrentInput()
        {
            string[] args;
            string command = FormatInput(InputString.Substring(1), out args); // Use substring to remove the slash

            bool showCommand = true;

            if (ConsoleFunctions.ConsoleCommands.ContainsKey(command))
            {
                showCommand = ((ConsoleFunction)ConsoleFunctions.ConsoleCommands[command]).Invoke(command, args, InputString);
            }

            if (showCommand)
            {
                Buffer.Add(new ConsoleMessage(InputString, LastTimeStamp, Color.White));
            }
        }

        static public void WriteCurrentInput()
        {
            Write("[Player]: "+InputString);
        }

        static public void Input(CharacterEventArgs e)
        {
            if (e.Character == '\b')
            {
                InputString = InputString.Substring(0, Math.Max(InputString.Length - 1, 0));
                CursorPosition--;
            }
            else if (e.Character == '\r')
            {
                if (InputString != null && InputString != "")
                {
                    if (InputString[0] == '/')
                    {
                        ExecuteCurrentInput();
                    }
                    else
                    {
                        WriteCurrentInput();
                    }
                }
                InputString = "";
                CursorPosition = 0;
                if (!(Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.LeftShift) || Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.RightShift)))
                {
                    Close();
                }
            }
            else if (e.Character == (char)27)
            {
                Toggle();
                CursorPosition = 0;
            }
            else if (e.Character == '\t')
            {
                InputString = "    ";
                CursorPosition += 4;
            }
            else
            {
                InputString = InputString + e.Character;
                CursorPosition++;
            }

            CursorPosition = Math.Max(CursorPosition, 0);
            CursorPosition = Math.Min(CursorPosition, InputString.Length);
        }

        static private string FormatInput(string input, out string[] args)
        {
            string[] inputs = input.ToLower().Split(' ');
            string command = inputs[0];
            args = new string[] { };
            if (inputs.Length > 1)
            {
                args = new string[inputs.Length - 1];
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = inputs[i + 1];
                }
            }

            return command;
        }

        static public void Update(GameTime gameTime)
        {

            if (InputString == null)
            {
                InputString = "";
            }

            if (State == ConsoleState.FadeOut)
            {
                StateCounter += (int)Math.Min(gameTime.ElapsedGameTime.TotalMilliseconds, Constants.Overlay.Console.FadeSpeed - StateCounter);
                if (StateCounter >= Constants.Overlay.Console.FadeSpeed)
                {
                    State = ConsoleState.Closed;
                }
            }
            else if (State == ConsoleState.FadeIn)
            {
                StateCounter -= (int)Math.Min(gameTime.ElapsedGameTime.TotalMilliseconds, StateCounter);
                if (StateCounter <= 0)
                {
                    State = ConsoleState.Open;
                }
            }

            if (State == ConsoleState.Open || State == ConsoleState.FadeIn)
            {
                Variables.Game.IsActive = false;
                IsActive = true;
            }
            else
            {
                if (State == ConsoleState.FadeOut)
                {
                    Constants.Engine_Input.ResetMouse();
                }
                Variables.Game.IsActive = true;
                IsActive = false;
            }

            LastTimeStamp = gameTime.TotalGameTime.TotalMilliseconds;
        }

        static public void Toggle()
        {
            InputString = "";
            if (State == ConsoleState.Closed || State == ConsoleState.FadeOut)
            {
                State = ConsoleState.FadeIn;
            }
            else
            {
                State = ConsoleState.FadeOut;
            }
        }

        static public void Open()
        {
            InputString = "";

            State = ConsoleState.FadeIn;
        }

        static public void Close()
        {
            InputString = "";

            State = ConsoleState.FadeOut;
        }

        static public void Draw(GameTime gameTime)
        {
            MessageQuantity = (int)(((float)Variables.Overlay.Console.Area.Height - 30.0F) / 18.0F);

            // gives a floating point number between 0 and 1 indicating how much the console is open
            float normalizedState = (((float)Constants.Overlay.Console.FadeSpeed - (float)StateCounter) / (float)Constants.Overlay.Console.FadeSpeed);

            Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Variables.Overlay.Console.Area.X, Variables.Overlay.Console.Area.Y, Variables.Overlay.Console.Area.Width, Variables.Overlay.Console.Area.Height), new Color(20, 20, 20, (int)(normalizedState * 100.0F)));
            Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Variables.Overlay.Console.Area.X, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height - 25, Variables.Overlay.Console.Area.Width, 25), new Color(20, 20, 20, (int)(normalizedState * 100.0F)));
            Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Variables.Overlay.Console.Area.X, Variables.Overlay.Console.Area.Y, 15, Variables.Overlay.Console.Area.Height - 25), new Color(20, 20, 20, (int)(normalizedState * 100.0F)));
            Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Variables.Overlay.Console.Area.X + 15, Variables.Overlay.Console.Area.Y, Variables.Overlay.Console.Area.Width - 15, 5), new Color(20, 20, 20, (int)(normalizedState * 100.0F)));
            Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Variables.Overlay.Console.Area.Width - 5, Variables.Overlay.Console.Area.Y, 5, Variables.Overlay.Console.Area.Height - 25), new Color(20, 20, 20, (int)(normalizedState * 100.0F)));

            int characterLimit = (Variables.Overlay.Console.Area.Width - 30) / 8;
            string inputString = InputString;

            if (inputString.Length > characterLimit)
            {
                inputString = inputString.Substring(inputString.Length - characterLimit, characterLimit);
            }
            Constants.Engine_Overlay.SpriteBatch.DrawString(Font, "> " + inputString, new Vector2(Variables.Overlay.Console.Area.X + 5, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height - 20), new Color(255, 255, 255, (int)(normalizedState * 255.0F)));
            Constants.Engine_Overlay.SpriteBatch.DrawString(Font, "|", new Vector2(Variables.Overlay.Console.Area.X + 3 + Font.MeasureString("> " + inputString.Substring(0, CursorPosition)).X, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height - 20), new Color(255, 255, 255, (int)((Math.Sin((float)(gameTime.TotalGameTime.TotalMilliseconds) / 100.0F) * 255.0F) * normalizedState)));


            if (Buffer.Count > 0)
            {
                int count = 0;
                int heightOffset = 0;
                string message = "";
                Color color = Color.White;
                for (int i = Buffer.Count - 1; i > Buffer.Count - MessageQuantity - 1; i--)
                {
                    if (i < 0 || i >= Buffer.Count)
                    {
                        break;
                    }

                    message = Buffer.ElementAt(i).Message;
                    heightOffset = (i - (Buffer.Count - 1)) * 18 - 40;

                    if (message.Length > 5)
                    {
                        message = message.Substring(0, (int)Math.Min(Math.Max(Console.Font.MeasureString("12345").X, Variables.Overlay.Console.Area.Width - 30) / 8, message.Length));
                    }

                    int closedAlpha = (int)(255 - (int)Math.Min(Math.Max(LastTimeStamp - Constants.Overlay.Console.Timeout - Buffer.ElementAt(i).Timestamp, 0) / 5.0F, 255));

                    if (Console.State == ConsoleState.Open)
                    {
                        color = new Color(255, 255, 255, 255);
                    }
                    else if (Console.State == ConsoleState.FadeIn || Console.State == ConsoleState.FadeOut)
                    {
                        color = new Color(255, 255, 255, (int)MathHelper.Lerp(closedAlpha, 255, normalizedState));
                    }
                    else if (Console.State == ConsoleState.Closed)
                    {
                        color = new Color(255, 255, 255, closedAlpha);
                    }

                    Constants.Engine_Overlay.SpriteBatch.DrawString(Font, message, new Vector2(Variables.Overlay.Console.Area.X + 20, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height + heightOffset), color);
                    count++;
                }

            }
        }
    }
}
