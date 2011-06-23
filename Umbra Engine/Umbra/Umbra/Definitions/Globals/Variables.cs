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
    static public class Variables
    {
        static public class Game
        {
            static public bool IsActive = true;
            static public bool DeveloperMode = true;

            static public bool IsInitialized;
        }

        static public class Controls
        {
            static public bool SmoothCameraEnabled = false;
        }

        static public class Overlay
        {
            static public bool DisplayFPS = true;

            static public class Console
            {
                static public Rectangle Area = Constants.Overlay.Console.DefaultArea;
            }
        }

        static public class Graphics
        {
            static public Color ScreenClearColor;

            static public class Lighting
            {
                static public bool FlashLightEnabled = false;
            }

            static public class Fog
            {
                static public bool Enabled = false;

                static public float CurrentStart;
                static public float CurrentEnd;
                static public float[] CurrentColor;
            }

            static public class DayNight
            {
                static public bool CycleEnabled = false;
                static public float CurrentFaceLightCoef;
            }
        }

        static public class World
        {
            static public Structures.World Current = new Structures.World("default");
        }

        static public class Player
        {
            static public bool NoclipEnabled = true;

            static public class BlockEditing
            {
                static public Block CurrentCursorBlock = Constants.Player.BlockEditing.StartBlock;
            }

            static public class Camera
            {
                static public class Bobbing
                {
                    static public bool Enabled = true;
                }
            }
        }

        static public class Sounds
        {
            static public bool MusicEnabled = false;
            static public bool InteractSoundEnabled = true;
            static public bool WalkSoundEnabled = true;
            static public byte CurrentTrack = 0;
        }
    }
}
