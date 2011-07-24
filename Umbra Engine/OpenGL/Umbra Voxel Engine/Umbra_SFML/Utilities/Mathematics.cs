using System;
using System.Linq;
using System.Text;
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

namespace Umbra.Utilities
{
    static class Mathematics
    {
        static public int AbsModulo(int value, int floor)
        {
            if (value > 0)
            {
                return value % floor;
            }
            else if (value < 0)
            {
                if (value % floor == 0)
                {
                    return 0;
                }
                else
                {
                    return floor + (value % floor);
                }
            }
            else
            {
                return 0;
            }
        }

        static public float WrapAngleRadians(float value)
        {
            return value % ((float)Math.PI * 2);
        }

        static public float WrapAngleDegrees(float value)
        {
            return value % 360;
        }

        static public float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
