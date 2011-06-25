using System;
using System.IO;
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
using Console = Umbra.Implementations.Console;


namespace Umbra.Definitions.Globals
{
    static public class Constants
    {
        static public Engines.Content Engine_Content;
        static public Engines.Graphics Engine_Graphics;
        static public Engines.Input Engine_Input;
        static public Engines.Main Engine_Main;
        static public Engines.Overlay Engine_Overlay;
        static public Engines.Physics Engine_Physics;
        static public Engines.Sound Engine_Sound;

        static public void SetupEngines(Main main)
        {
            Engine_Content = new Engines.Content(main);
            Engine_Graphics = new Engines.Graphics(main);
            Engine_Input = new Engines.Input(main);
            Engine_Main = main;
            Engine_Overlay = new Engines.Overlay(main);
            Engine_Physics = new Engines.Physics(main);
            Engine_Sound = new Engines.Sound(main);

            Engine_Main.Components.Add(Engine_Content);
            Engine_Main.Components.Add(Engine_Input);
            Engine_Main.Components.Add(Engine_Physics);
            Engine_Main.Components.Add(Engine_Graphics);
            Engine_Main.Components.Add(Engine_Overlay);
            Engine_Main.Components.Add(Engine_Sound);

            World.Current = new Structures.World("default");

            ConsoleFunctions.Initialize();
            EventInput.Initialize(Engine_Main.Window);
            ChunkManager.InitializeThreads();
            LandscapeGenerator.Initialize(Landscape.WorldSeed);
            World.Current.Initialize();
            LandscapeGenerator.GenerateTerrain();
            ClockTime.SetTimeOfDay(TimeOfDay.Day);
        }

        static public class Overlay
        {

            static public class Console
            {
                static public int CharacterLimit = 32;
                static public Rectangle DefaultArea = new Rectangle(0, (int)Graphics.ScreenResolution.Y / 2, 290, (int)Graphics.ScreenResolution.Y / 2);
                static public int FadeSpeed = 500; // mS
                static public int Timeout = 10000; // mS
            }

            static public class Popup
            {
                static public int Timein = 1000; // mS
                static public int Timeout = 3000; // mS
            }

            static public class Compass
            {
                static public Vector2 FrameSize = new Vector2(144, 85);
                static public Vector2 ScreenPosition = new Vector2(Graphics.ScreenResolution.X - FrameSize.X - 10, 10);
                static public Vector2 StripOffset = new Vector2(10, 10);
                static public Vector2 StripWindowSize = FrameSize - 2 * StripOffset;
            }
        }

        static public class Controls
        {
            static public float MouseSensitivityInv = 200.0F;
            static public float AlterDelay = 250.0F;            // Delay between editing blocks (mS)
            static public float SmoothCameraResponse = 0.4F;
            static public string[] PlacableBlocks = {
                                                    "grass",
                                                    "stone",
                                                    "dirt",
                                                    "water",
                                                    "glass",
                                                    "bookshelf",
                                                    "log",
                                                    "wood",
                                                    "snow",
                                                    "slab",
                                                    "craftingtable",
                                                    "furnace",
                                                    "leaves",
                                                    "lava",
                                                    "sand",
                                                    "brick",
                                                    "cobblestone",
                                                    "ice",
                                               };
        }

        static public class Graphics
        {
            static public Vector2 ScreenResolution = new Vector2(1024, 600);
            static public float AspectRatio = ScreenResolution.X / ScreenResolution.Y;
            static public bool AntiAliasingEnabled = true;
            static public float CameraNearPlane = 0.01f;
            static public float CameraFarPlane = 64000f;
            static public float FieldOfView = MathHelper.ToRadians(60);
            static public bool EnableFullScreen = false;
            static public float[] TranslucentBlocks = { Block.Glass.GetFace(Direction.Up), Block.Water.GetFace(Direction.Up), Block.Leaves.GetFace(Direction.Up), Block.Ice.GetFace(Direction.Up) };

            static public class Lighting
            {
                static public byte UpShade = 250;
                static public byte DownShade = 250;
                static public byte LeftShade = 200;
                static public byte RightShade = 200;
                static public byte ForwardShade = 150;
                static public byte BackwardShade = 150;

                static readonly public float DayFaceLightCoef = 1F;
                static readonly public float NightFaceLightCoef = 1F / 16F;
            }

            static public class Fog
            {
                static readonly public float DayFogStart = World.WorldSize * World.ChunkSize / 2 - 80;
                static readonly public float NightFogStart = 0;
                static readonly public float DayFogEnd = World.WorldSize * World.ChunkSize / 2 - 20;
                static readonly public float NightFogEnd = World.WorldSize * World.ChunkSize / 5;
            }

            static public class DayNight
            {
                static readonly public Color DayColor = Color.CornflowerBlue;
                static readonly public Color NightColor = Color.Black;

                static public float DayDuration = 16;           //seconds
                static public float NightDuration = 6;          //seconds
                static public float TransitionDuration = 1;     //Happens twice every cycle
                static public float TotalDuration = DayDuration + NightDuration + 2 * TransitionDuration;
            }
        }

        static public class World
        {
            static public Structures.World Current;
            static public int ChunkSize = 32;
            static public int WorldSize = 8;
            static public float UpdateChunksMoveLength = ChunkSize * ChunkSize / 4;
            static public float UpdateGridsMoveLength = ChunkSize * WorldSize * ChunkSize * WorldSize / 6;
            static public bool DynamicWorld = true;
            static public bool SaveDynamicWorld = false;
        }

        static public class Physics
        {
            static public float Gravity = 9.81F;
            static public float FrictionSignificance = 1F;
        }

        static public class Landscape
        {
            static public int PerlinNoiseAmplitude = World.WorldSize * World.ChunkSize / 8;
            static public int BicubicAmplitude = World.WorldSize * World.ChunkSize / 1;
            static public string WorldSeed = "time";
            static public int WorldHeightOffset = 0;
        }

        static public class Player
        {
            static public Vector3 Spawn = new Vector3(0, 0, 0);
            static public float MinDistanceToGround = 0.02F;

            static public class Physics
            {
                static public float Mass = 75.0F;

                static public class Box
                {
                    static public float Width = 0.6F;
                    static public float Height = 1.8F;
                }

                static public float EyeHeight = 1.5F;
            }

            static public class Camera
            {
                static public class Bobbing
                {
                    static public float Speed = 0.4F;
                    static public float Magnitude = 0.7F;
                }
            }

            static public class Movement
            {
                static public float NoclipSpeed = 0.3F;

                static public float WalkForce = 24.0F * Physics.Mass;
                static public float MaxSpeed = 4.0F;
                static public float JumpForce = Physics.Mass * 5.42F * 60.0F;

                static public float GripSignificance = 3F;
            }

            static public class BlockEditing
            {
                static public Block StartBlock = Block.Stone;
                static public float Reach = 10.0F;
            }
        }

        static public class Content
        {
            static public class Textures
            {
                static public string Path = @"textures/";
                static public string TerrainFilename = Path + @"textures";
                static public string CrosshairFilename = Path + @"crosshair";
                static public string CompassFilename = Path + @"compass";
                static public int TerrainTextureIndexWidthHeigh = 16;
            }

            static public class Fonts
            {
                static public string Path = @"fonts/";
                static public string ConsoleFilename = Path + @"console";
                static public string DebugFilename = Path + @"debug";
                static public string PopupFilename = Path + @"popup";
            }

            static public class Sounds
            {
                static public string Path = @"sounds/";
                static public string MusicPath = Path + @"music/";
                static public string SongFilename = MusicPath + @"song";
                static public string WalkFilename = Path + @"walk";
                static public string BlockEditFilename = Path + @"snap";
            }

            static public class Data
            {
                static public string WorldPath = Directory.GetCurrentDirectory() + @"worlds/";
                static public string ChunkFilePath = @"chunks/";
                static public string ChunkFileExtension = @".cnk";
            }
        }
    }
}
