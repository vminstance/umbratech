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

namespace Umbra.Definitions
{
    static public class Constants
    {
        //----------------
        // Engines
        //----------------
        static public Content Content;
        static public Graphics Graphics;
        static public Input Input;
        static public Main Main;
        static public Overlay Overlay;
        static public Player Player;
        static public Sound Sound;

        static public bool IsInitialized;



        //----------------
        // Game
        //----------------

        static public bool GameIsActive = true;
        static public bool DeveloperMode = true;


        //----------------
        // World
        //----------------

        static public World CurrentWorld = new World("default");
        public const int ChunkSize = 32;
        public const int WorldSize = 8;
        public const float UpdateChunksMoveLength = ChunkSize * ChunkSize / 4;
        public const float UpdateGridsMoveLength = ChunkSize * WorldSize * ChunkSize * WorldSize / 6;
        static public bool DynamicWorld = true;
        public const bool SaveDynamicWorld = false;


        //----------------
        // Controls
        //----------------

        public const float MouseSensitivityInv = 200.0F;
        static public float AlterDelay = 250.0F; // Delay between editing blocks (mS)
        public const bool SmoothCameraEnabled = false;
        public const float SmoothCameraRespons = 0.4F;
        static public ushort CurrentCursorBlock = Block.Stone;
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
                                                    "sand"
                                               };

        static public float[] TranslucentBlocks = { Block.GetFace(Block.Glass, Direction.Up), Block.GetFace(Block.Water, Direction.Up), Block.GetFace(Block.Leaves, Direction.Up), Block.GetFace(255, Direction.Up) };



        //----------------
        // Landscape Generation
        //----------------
        public const int PerlinNoiseAmplitude = WorldSize * ChunkSize / 8;
        public const int BicubicAmplitude = WorldSize * ChunkSize / 1;
        public const string WorldSeed = "time";
        public const int WorldHeightOffset = 0;



        //----------------
        // Graphics
        //----------------

        // Setup
        static public Color ScreenClearColor;
        static public Vector2 ScreenResolution = new Vector2(1024, 600);
        static public float AspectRatio = ScreenResolution.X / ScreenResolution.Y;
        static public bool AntiAliasingEnabled = true;
        public const float CameraNearPlane = 0.01f;
        public const float CameraFarPlane = 64000f;
        static public float FieldOfView = MathHelper.ToRadians(60);
        public const bool EnableFullScreen = false;

        // Appereance
        static public byte TextureUpShade = 250;
        static public byte TextureDownShade = 250;
        static public byte TextureLeftShade = 200;
        static public byte TextureRightShade = 200;
        static public byte TextureForwardShade = 150;
        static public byte TextureBackwardShade = 150;


        // Fog
        static public bool FogEnabled = false;

        static readonly public Color DayColor = Color.CornflowerBlue;
        static readonly public Color NightColor = Color.Black;

        static readonly public float DayFaceLightCoef = 1F;
        static readonly public float NightFaceLightCoef = 1F / 16F;

        static readonly public float DayFogStart = WorldSize * ChunkSize / 2 - 80;
        static readonly public float NightFogStart = 0;
        static readonly public float DayFogEnd = WorldSize * ChunkSize / 2 - 20;
        static readonly public float NightFogEnd = WorldSize * ChunkSize / 5;

        static public float CurrentFogStart;
        static public float CurrentFogEnd;
        static public float CurrentFaceLightCoef;
        static public float[] CurrentFogColor;


        // Weather & Day/Night
        static public bool DayNightCycleEnabled = false;
        static public float DayDuration = 16;           //seconds
        static public float NightDuration = 6;          //seconds
        static public float TransitionDuration = 1;     //Happens twice every cycle
        static public float TotalDuration = DayDuration + NightDuration + 2 * TransitionDuration;

        //Flashlight
        static public bool FlashLightEnabled = false;




        //----------------
        // Player
        //----------------

        public const float PlayerBoxWidth = 0.6F;
        public const float PlayerBoxHeight = 1.8F;
        public const float PlayerEyeHeight = 1.5F;
        static public Vector3 PlayerSpawn = new Vector3(0, 0, 0);

        // Camera
        static public bool CameraBobbingEnabled = true;
        public const float CameraBobbingSpeed = 0.4F;
        public const float CameraBobbingMagnitude = 0.7F;

        // Movement
        static public bool NoclipEnabed = true;
        public const float NoclipSpeed = 0.3F;

        static public float[] WalkingValues = { 0.015F, 0.015F, 0.1F, 0.01F }; // Acceleration, Friction, MaxSpeed, MinSpeed
        static public float[] RunningValues = { 0.03F, 0.03F, 0.2F, 0.02F };
        static public float[] FlyingValues = { 0.005F, 0.0F, 0.1F, 0.001F };
        static public float[] CurrentValues = WalkingValues;

        public const float JumpForce = 0.17F;
        public const float Gravity = 0.01F;
        public const float AirFriction = 0.015F;
        public const float PlayerMinDistanceToGround = 0.02F;

        // Block editing
        static public ushort StartBlock = Block.Stone;
        public const float PlayerReach = 10.0F;



        //----------------
        // Overlay
        //----------------
        static public bool DisplayFPS = true;

        // Console
        static public int CharacterLimit = 32;
        static public Rectangle DefaultConsoleArea = new Rectangle(0, (int)ScreenResolution.Y / 2, 290, (int)ScreenResolution.Y / 2);
        static public Rectangle ConsoleArea = DefaultConsoleArea;
        public const int ConsoleFadeSpeed = 500; // mS
        public const int ConsoleTimeout = 10000; // mS

        // Popup
        public const int PopupTimein = 1000; // mS
        public const int PopupTimeout = 3000; // mS

        // Compass
        static public Vector2 CompassFrameSize = new Vector2(144, 85);
        static public Vector2 CompassScreenPosition = new Vector2(ScreenResolution.X - CompassFrameSize.X - 10, 10);
        static public Vector2 CompassStripOffset = new Vector2(10, 10);
        static public Vector2 CompassStripWindowSize = CompassFrameSize - 2 * CompassStripOffset;


        //----------------
        // Sounds
        //----------------
        static public bool MusicEnabled = true;
        static public bool InteractSoundEnabled = true;
        static public bool WalkSoundEnabled = true;



        //----------------
        // Content
        //----------------

        // Textures
        public const string TexturePath = @"textures/";
        public const string TerrainTextureFilename = TexturePath + @"textures";
        public const int TerrainTextureIndexWidthHeigh = 16;
        public const string CrosshairTextureFilename = TexturePath + @"crosshair";
        public const string CompassTextureFilename = TexturePath + @"compass";

        // Fonts
        public const string FontPath = @"fonts/";
        public const string ConsoleFontFilename = FontPath + @"console";
        public const string DebugFontFilename = FontPath + @"debug";
        public const string PopupFontFilename = FontPath + @"popup";

        // Paths
        static public string WorldPath = Directory.GetCurrentDirectory() + @"\worlds";
        public const string ChunkFilePath = @"\chunks";
        public const string ChunkFileExtension = @".cnk";

        static public void SetupEngines(Main main)
        {
            Content = new Content(main);
            Graphics = new Graphics(main);
            Input = new Input(main);
            Main = main;
            Overlay = new Overlay(main);
            Player = new Player(main);
            Sound = new Sound(main);

            Main.Components.Add(Content);
            Main.Components.Add(Graphics);
            Main.Components.Add(Input);
            Main.Components.Add(Overlay);
            Main.Components.Add(Player);
            Main.Components.Add(Sound);

            ConsoleFunctions.Initialize();
            EventInput.Initialize(Main.Window);
            ChunkManager.InitializeThreads();
            LandscapeGenerator.Initialize(Constants.WorldSeed);
            CurrentWorld.Initialize();
            LandscapeGenerator.GenerateTerrain();
            ClockTime.SetTimeOfDay(TimeOfDay.Day);
        }
    }
}
