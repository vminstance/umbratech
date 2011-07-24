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
using Console = Umbra.Implementations.Console;

namespace Umbra.Definitions.Globals
{
    static public class Variables
    {
        static public class Game
        {
            static private bool _isActive = true;
            static public bool IsActive
            {
                get
                {
                    return _isActive;
                }
                set
                {
                    _isActive = value;
                    Constants.Engine_Input.SetMouseShow(!value);
                }
            }
            static public bool DeveloperMode = true;

            static public bool IsInitialized;
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
            static public Color ScreenClearColor
            {
                get
                {
                    return ClockTime.GetScreenColor();
                }
            }

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

        static public class Player
        {
            static private bool _NoclipEnabled = true;

            static public bool NoclipEnabled
            {
                get
                {
                    return _NoclipEnabled && Constants.Controls.NoclipAllowed;
                }
                set
                {
                    _NoclipEnabled = value;
                }
            }

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
