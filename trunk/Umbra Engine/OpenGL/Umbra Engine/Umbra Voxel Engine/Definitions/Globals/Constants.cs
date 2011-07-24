﻿using System;
using System.IO;
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

namespace Umbra.Definitions.Globals
{
    static public class Constants
    {
        static public Engines.Graphics Engine_Graphics;
        static public Engines.Input Engine_Input;
        static public Engines.Main Engine_Main;
        static public Engines.Overlay Engine_Overlay;
        static public Engines.Physics Engine_Physics;
        static public Engines.Audio Engine_Sound;

        static public void SetupEngines(Main main)
        {
            Engine_Graphics = new Engines.Graphics();
            Engine_Input = new Engines.Input();
            Engine_Main = main;
            Engine_Overlay = new Engines.Overlay();
            Engine_Physics = new Engines.Physics();
            Engine_Sound = new Engines.Audio();

            Engine_Main.AddEngine(Engine_Input);
            Engine_Main.AddEngine(Engine_Physics);
            Engine_Main.AddEngine(Engine_Graphics);
            Engine_Main.AddEngine(Engine_Overlay);
            Engine_Main.AddEngine(Engine_Sound);


            LandscapeGenerator.Initialize(Landscape.WorldSeed);
            Engine_Physics.Player.Initialize();

            World.Current = new Structures.World(World.Name);
            ConsoleFunctions.Initialize();
            ChunkManager.InitializeThreads();
            World.Current.Initialize();
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
            static public float MouseSensitivityInv = 400.0F;
            static public float AlterDelay = 30.0F;            // Delay between editing blocks (mS)
            static public bool SmoothCameraEnabled = false;
            static public bool CanPlaceBlocks = false;
            static public bool NoclipAllowed = true;
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
            static public Vector2 ScreenResolution = new Vector2(1920, 1080);
            static public float AspectRatio = ScreenResolution.X / ScreenResolution.Y;
            static public bool AntiAliasingEnabled = true;
            static public float CameraNearPlane = 0.01f;
            static public float CameraFarPlane = 64000f;
            static public float FieldOfView = MathHelper.DegreesToRadians(60);
            static public bool EnableFullScreen = false;
            static public float[] TranslucentBlocks = { Block.Glass.GetFace(Direction.Up), Block.Water.GetFace(Direction.Up), Block.Leaves.GetFace(Direction.Up), Block.Ice.GetFace(Direction.Up) };

            static public class Lighting
            {
                static public byte UpShade = 250;
                static public byte DownShade = 100;
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

            static public class Forms
            {
                static public int MinimumHeight = 52;
                static public int MinimumWidth = 60;
            }
        }

        static public class Launcher
        {
            static public bool Enabled = true;
        }

        static public class World
        {
            static public Structures.World Current;
            static public string Name = "default";
            static public int ChunkSize = 32;
            static public int WorldSize = 13;
            static public bool DynamicWorld = true;
            static public bool SaveDynamicWorld = false;
            static public int UpdateLengthFromCenter = ChunkSize;
        }

        static public class Physics
        {
            static public float Gravity = 9.81F;
            static public float FrictionSignificance = 1F;
        }

        static public class Landscape
        {
            static public string WorldSeed = "";
            static public int PerlinOctaves = 8;            // Area taken into account = 2^octaves, currently 256 blocks;
            static public float PerlinBicubicWeight = 0.7F; // 0.0F = Total perlin, 1.0F = Total Bicubic
            static public float WorldHeightAmplitude = 128.0F;//256.0F;
            static public int WorldHeightOffset = (int)(-WorldHeightAmplitude / 2.0F) - 4;


            static public class Vegetation
            {
                static public float TreeMinHeight = 7.0F;   // Tree height = (random 0-1) * TreeVaryHeight + TreeMinHeight
                static public float TreeVaryHeight = 8.0F;  // Tree height will vary from TreeMinHeight to TreeVaryHeight + TreeMinHeight
                static public float TreeChance = 0.05F;     // If a tree can be placed at a location, this is the chance that it will grow there.
            }
        }

        static public class Player
        {
            static public Vector3 Spawn = new Vector3(16, 0, 16);
            static public float MinDistanceToGround = 0.02F;

            static public class Physics
            {
                static public float Mass = 350.0F;

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

                static public float WalkForce = 36.0F * Physics.Mass;
                static public float MaxSpeed = 4.0F;
                static public float JumpForce = Physics.Mass * 4.9F * 60.0F;
                static public float SwimForce = 40.0F * Physics.Mass;

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
                static public string WorldPath = Directory.GetCurrentDirectory() + @"/worlds/";
                static public string ChunkFilePath = @"chunks/";
                static public string ChunkFileExtension = @".cnk";
            }
        }
    }
}
