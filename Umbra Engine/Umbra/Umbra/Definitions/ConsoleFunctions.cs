using System;
using System.Linq;
using System.Text;
using System.Collections;
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

namespace Umbra.Definitions
{
    public delegate bool ConsoleFunction(string command, string[] args, string original);

    static public class ConsoleFunctions
    {
        static public Hashtable ConsoleCommands { get; private set; }

        static public void Initialize()
        {
            ConsoleCommands = new Hashtable();

            ConsoleCommands["exit"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                ChunkManager.AbortThreads();
                Constants.Engine_Main.Exit();
                return false;
            });
            ConsoleCommands.Add("quit", ConsoleCommands["exit"]);



            ConsoleCommands["info"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                Popup.Post("Umbra Engine 1.0.0");
                return false;
            });

            ConsoleCommands["clear"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                Console.Clear();
                Popup.Post("Console cleared!");
                return false;
            });

            ConsoleCommands["popup"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length == 0)
                {
                    return false;
                }
                Popup.Post(original.Substring(command.Length + 1));

                return false;
            });

            ConsoleCommands["help"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length == 0)
                {
                    Console.Write("           Help menu             ");
                    Console.Write("---------------------------------");
                    Console.Write("");
                    Console.Write("To get help with a command, type:");
                    Console.Write("\"help <command>\"");
                    Console.Write("");
                    Console.Write("To quit, type:");
                    Console.Write("\"exit\"");
                    Console.Write("");
                    Console.Write("To exit the console, press:");
                    Console.Write("[ESCAPE]");
                    Console.Write("");
                    Console.Write("");
                    Console.Write("---------------------------------");
                    Console.Write("");
                    Console.Write("");
                }

                return false;
            });

            ConsoleCommands["viewbobbing"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Variables.Player.Camera.Bobbing.Enabled = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Variables.Player.Camera.Bobbing.Enabled = !Variables.Player.Camera.Bobbing.Enabled;
                }

                Popup.Post("Camera Bobbing: " + Variables.Player.Camera.Bobbing.Enabled.ToString());

                return false;
            });

            ConsoleCommands["noclip"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Variables.Player.NoclipEnabled = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Variables.Player.NoclipEnabled = !Variables.Player.NoclipEnabled;
                }

                Popup.Post("Noclip: " + Variables.Player.NoclipEnabled.ToString());

                Constants.Engine_Physics.Player.Velocity = Vector3.Zero;

                return false;
            });

            ConsoleCommands["nightday"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Variables.Graphics.DayNight.CycleEnabled = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Variables.Graphics.DayNight.CycleEnabled = !Variables.Graphics.DayNight.CycleEnabled;
                }

                Popup.Post("Day/Night Cycle: " + Variables.Graphics.DayNight.CycleEnabled.ToString());

                return false;
            });

            ConsoleCommands["fps"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Variables.Overlay.DisplayFPS = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Variables.Overlay.DisplayFPS = !Variables.Overlay.DisplayFPS;
                }

                Popup.Post("FPS: " + Variables.Overlay.DisplayFPS.ToString());

                return false;
            });

            ConsoleCommands["dynamicworld"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Constants.World.DynamicWorld = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Constants.World.DynamicWorld = !Constants.World.DynamicWorld;
                }

                Popup.Post("Dynamic World: " + Constants.World.DynamicWorld.ToString());

                return false;
            });

            ConsoleCommands["fog"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Variables.Graphics.Fog.Enabled = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Variables.Graphics.Fog.Enabled = !Variables.Graphics.Fog.Enabled;
                }

                Popup.Post("Fog: " + Variables.Graphics.Fog.Enabled.ToString());

                return false;
            });

            ConsoleCommands["flashlight"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    if (Boolify(args[0]).HasValue)
                    {
                        Variables.Graphics.Lighting.FlashLightEnabled = Boolify(args[0]).Value;
                    }
                }
                else
                {
                    Variables.Graphics.Lighting.FlashLightEnabled = !Variables.Graphics.Lighting.FlashLightEnabled;
                }

                Popup.Post("Flashlight: " + Variables.Graphics.Lighting.FlashLightEnabled.ToString());

                return false;
            });

            ConsoleCommands["light"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                Variables.Graphics.DayNight.CurrentFaceLightCoef = 5.0F;
                return false;
            });

            ConsoleCommands["facing"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                Popup.Post(Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationY(Constants.Engine_Physics.Player.FirstPersonCamera.Direction)).ToString());
                return false;
            });

            ConsoleCommands["list"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                Console.Write("---------------------------------");
                foreach (string s in ConsoleCommands.Keys)
                {
                    Console.Write(s);
                }

                Console.Toggle();
                return false;
            });

            ConsoleCommands["time"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "sunset":
                            ClockTime.SetTimeOfDay(TimeOfDay.SunSet);
                            break;

                        case "night":
                            ClockTime.SetTimeOfDay(TimeOfDay.Night);
                            break;

                        case "sunrise":
                            ClockTime.SetTimeOfDay(TimeOfDay.SunRise);
                            break;

                        case "day":
                            ClockTime.SetTimeOfDay(TimeOfDay.Day);
                            break;

                        case "cycle":
                            {
                                if (args.Length > 1)
                                {
                                    Console.Execute("/daynight " + args[1]);
                                }
                                else
                                {
                                    Console.Execute("/daynight");
                                }
                                break;
                            }

                        default:
                            int time;
                            if (int.TryParse(args[0], out time))
                            {
                                ClockTime.SetTimeOfDay((float)time);
                            }
                            else
                            {
                                Console.Write("\"time <time>\"");
                                Console.Write("<time> must be either an integer");
                                Console.Write("between 0 - 360 or");
                                Console.Write("[DAY | NIGHT | SUNSET | SUNRISE]");
                            }
                            break;
                    }
                }
                return false;
            });

            ConsoleCommands["block"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                if (args.Length == 0)
                {
                    Console.Write("Usage: \"block <blocktype>\"");
                    Console.Write("Currently selectable blocktypes:");
                    foreach (string s in Constants.Controls.PlacableBlocks)
                    {
                        Console.Write(s);
                    }
                    Console.Toggle();
                    return false;
                }

                bool canUse = false;
                foreach (string s in Constants.Controls.PlacableBlocks)
                {
                    if (args[0] == s)
                    {
                        canUse = true;
                    }
                }

                if (!canUse)
                {
                    Popup.Post("\"" + args[0] + "\" is not a placable block!");
                    Console.Toggle();
                    return false;
                }

                Variables.Player.BlockEditing.CurrentCursorBlock = Block.GetFromName(args[0]);
                Popup.Post("Block cursor set to " + args[0] + ".");
                return false;
            });
        
            ConsoleCommands["window"] = (ConsoleFunction)((string command, string[] args, string original) =>
            {
                Constants.Engine_Overlay.DebugWindow("scrollWheel");
                return false;
            });
        }

        static private bool? Boolify(string input)
        {
            switch (input)
            {
                case "on": return true;
                case "off": return false;
                case "enable": return true;
                case "disable": return false;
                case "true": return true;
                case "false": return false;
                case "yes": return true;
                case "no": return false;
                case "Hija\'": return true;
                case "goble\'": return false;

                default: return null;
            }
        }
    }
}
