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
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Implementations.Graphics
{
    static public class Console
    {
        static public bool IsOpen { get; private set; }

        static private double LastTimeStamp = 0;

        static private List<ConsoleMessage> Buffer { get; set; }
        static private int CurrentBufferSelection = 0;
        static public string InputString { get; set; }
        static public int CursorPosition = 0;

        static public void Initialize()
        {
            Buffer = new List<ConsoleMessage>();
            IsOpen = false;
            InputString = "";
        }

        static public bool IsClosed()
        {
            return !IsOpen;
        }

        static public void Clear()
        {
            Buffer = new List<ConsoleMessage>();
        }

        static private void AddMessage(ConsoleMessage message)
        {
            Buffer.Add(message);

            if (CurrentBufferSelection != 0)
            {
                InputString = Buffer[Buffer.Count - CurrentBufferSelection].Message;
                CursorPosition = InputString.Length;
            }
        }

        static public void Write(string message)
        {
            if (message != "")
            {
                AddMessage(new ConsoleMessage(message, LastTimeStamp, Color.White));
            }
        }

        static public void Execute(string inputString)
        {
            string[] args;
            string command = FormatInput(inputString, out args);

            if (ConsoleFunctions.ConsoleCommands.ContainsKey(command))
            {
                ((ConsoleFunction)ConsoleFunctions.ConsoleCommands[command]).Invoke(command, args, inputString);
                AddMessage(new ConsoleMessage(inputString, LastTimeStamp, Color.White));
            }
        }

        static public void ExecuteCurrentInput()
        {
            string[] args;
            string command = FormatInput(InputString, out args);


            if (ConsoleFunctions.ConsoleCommands.ContainsKey(command))
            {
                ((ConsoleFunction)ConsoleFunctions.ConsoleCommands[command]).Invoke(command, args, InputString);
                AddMessage(new ConsoleMessage(InputString, LastTimeStamp, Color.White));
            }
        }

        static public void Input(KeyboardKeyEventArgs e, KeyboardDevice keyboard)
        {
            string character = e.Key.ToString();

            if (e.Key == Key.Up)
            {
                CurrentBufferSelection = (int)Mathematics.Clamp(CurrentBufferSelection + 1, 1, Buffer.Count);
                int selected = Buffer.Count - CurrentBufferSelection;

                InputString = Buffer[selected].Message;
                CursorPosition = InputString.Length;
            }
            else if (e.Key == Key.Down)
            {
                CurrentBufferSelection = (int)Mathematics.Clamp(CurrentBufferSelection - 1, 0, Buffer.Count - 1);

                if (CurrentBufferSelection == 0)
                {
                    InputString = "";
                }
                else
                {
                    int selected = Buffer.Count - CurrentBufferSelection;

                    InputString = Buffer[selected].Message;
                }

                CursorPosition = InputString.Length;
            }
            else if (e.Key == Key.Left)
            {
                CursorPosition--;
            }
            else if (e.Key == Key.Right)
            {
                CursorPosition++;
            }
            else if (e.Key == Key.Enter)
            {
                if (InputString != null && InputString != "")
                {
                    ExecuteCurrentInput();
                }

                InputString = "";
                CursorPosition = 0;
            }
            else if (e.Key == Key.Escape)
            {
                Toggle();
            }
            else if (e.Key == Key.Tab)
            {
                InputString += "    ";
                CursorPosition += 4;
            }
            else if (e.Key == Key.Space)
            {
                InputString += " ";
                CursorPosition++;
            }
            else if (e.Key == Key.BackSpace)
            {
                InputString = InputString.Substring(0, Math.Max(CursorPosition - 1, 0)) + InputString.Substring(CursorPosition);
                CursorPosition--;
            }
            else if (e.Key == Key.Delete)
            {
                InputString = InputString.Substring(0, Math.Max(CursorPosition, 0)) + InputString.Substring(Math.Min(CursorPosition + 1, InputString.Length));
            }
            else if(character.Length == 1)
            {
                if (!keyboard[Key.ShiftLeft] && !keyboard[Key.ShiftRight])
                {
                    character = character.ToLower();
                }
                InputString = InputString.Substring(0, CursorPosition) + character + InputString.Substring(CursorPosition);
                CursorPosition++;
            }

            if (InputString.Length > Constants.Overlay.Console.CharacterLimit)
            {
                InputString = InputString.Substring(0, Constants.Overlay.Console.CharacterLimit);
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

        static public void Toggle()
        {
            Constants.Engine_Input.CenterMouse();
            InputString = "";
            IsOpen = !IsOpen;
            Variables.Game.IsActive = !IsOpen;
        }

        static public void Open()
        {
            InputString = "";

            IsOpen = true;
            Variables.Game.IsActive = false;
        }

        static public void Close()
        {
            InputString = "";

            IsOpen = false;
            Variables.Game.IsActive = true;
        }

        static public void Update(FrameEventArgs e)
        {
            LastTimeStamp += e.Time;
        }

        static public void Render(FrameEventArgs e)
        {
            if (IsOpen)
            {
                SpriteString.Render(">" + InputString, new Point(Variables.Overlay.Console.Area.X + 10, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height - 20), Color.Beige);
                SpriteString.Render("|", new Point(Variables.Overlay.Console.Area.X + 5 + SpriteString.Measure(">" + InputString.Substring(0, CursorPosition)).X, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height - 20), Color.FromArgb((int)((LastTimeStamp * 2) % 2) * 255, Color.White));
            }


            if (Buffer.Count > 0)
            {
                int count = 0;
                int heightOffset = 0;
                ConsoleMessage message;
                Color color = Color.White;
                for (int i = Buffer.Count - 1; i > Buffer.Count - Constants.Overlay.Console.MessageQuantity - 1; i--)
                {
                    if (i < 0 || i >= Buffer.Count)
                    {
                        break;
                    }

                    message = Buffer.ElementAt(i);
                    heightOffset = (i - (Buffer.Count - 1)) * 18 - 40;

                    int closedAlpha = (int)(255 - (int)Math.Min(Math.Max(LastTimeStamp - Constants.Overlay.Console.Timeout - Buffer.ElementAt(i).Timestamp, 0) / 5.0F, 255));

                    if (Console.IsOpen)
                    {
                        color = Color.FromArgb(message.Color.R, message.Color.G, message.Color.B, 255);
                    }
                    else
                    {
                        color = Color.FromArgb(message.Color.R, message.Color.G, message.Color.B, closedAlpha);
                    }

                    SpriteString.Render(message.Message, new Point(Variables.Overlay.Console.Area.X + 20, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height + heightOffset), color);

                    count++;
                }
            }
        }
    }
}
