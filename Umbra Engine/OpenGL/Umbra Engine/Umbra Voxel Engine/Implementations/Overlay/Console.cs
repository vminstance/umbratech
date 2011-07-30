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
        static List<ConsoleMessage> Buffer = new List<ConsoleMessage>();
        static public Font Font = new Font("Lucida Console", 11, FontStyle.Regular);

        static bool IsOpen = false;
        static int StateCounter = Constants.Overlay.Console.FadeSpeed;

        static double LastTimeStamp = 0;

        static public string InputString { get; set; }
        static public int CursorPosition = 0;

        static int MessageQuantity = 19;

        static public bool IsClosed()
        {
            return !IsOpen;
        }

        static public void Clear()
        {
            Buffer = new List<ConsoleMessage>();
        }

        static public void Write(string message)
        {
            if (message != "")
            {
                Buffer.Add(new ConsoleMessage(message, LastTimeStamp, Color.White));
            }
        }

        static public void Execute(string inputString)
        {
            string[] args;
            string command = FormatInput(inputString, out args);

            if (ConsoleFunctions.ConsoleCommands.ContainsKey(command))
            {
                ((ConsoleFunction)ConsoleFunctions.ConsoleCommands[command]).Invoke(command, args, inputString);
                Buffer.Add(new ConsoleMessage(inputString, LastTimeStamp, Color.White));
            }
        }

        static public void ExecuteCurrentInput()
        {
            string[] args;
            string command = FormatInput(InputString, out args);


            if (ConsoleFunctions.ConsoleCommands.ContainsKey(command))
            {
                ((ConsoleFunction)ConsoleFunctions.ConsoleCommands[command]).Invoke(command, args, InputString);
                Buffer.Add(new ConsoleMessage(InputString, LastTimeStamp, Color.White));
            }
        }

        static public void Input(KeyboardKeyEventArgs e)
        {
            string character = e.Key.ToString();

            if (e.Key == Key.BackSpace)
            {
                InputString = InputString.Substring(0, Math.Max(InputString.Length - 1, 0));
                CursorPosition--;
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
            else
            {
                InputString = InputString + character;
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

        static public void Toggle()
        {
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

            if (InputString == null)
            {
                InputString = "";
            }

            LastTimeStamp += e.Time;
        }

        static public void Render(FrameEventArgs e)
        {
            if (Buffer.Count > 0)
            {
                int count = 0;
                int heightOffset = 0;
                ConsoleMessage message;
                Color color = Color.White;
                for (int i = Buffer.Count - 1; i > Buffer.Count - MessageQuantity - 1; i--)
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

                    //message.SpriteString.Render(new Point(Variables.Overlay.Console.Area.X + 20, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height + heightOffset), color);
                    SpriteString.Render(message.Message, Font, new Point(Variables.Overlay.Console.Area.X + 20, Variables.Overlay.Console.Area.Y + Variables.Overlay.Console.Area.Height + heightOffset), color);


                    count++;
                }
            }
        }
    }
}
