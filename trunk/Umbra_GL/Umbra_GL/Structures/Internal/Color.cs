using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Internal;
using Console = Umbra.Implementations.Console;

namespace Umbra.Structures.Internal
{
    public class Color
    {
        byte R { get; private set; }
        byte G { get; private set; }
        byte B { get; private set; }
        byte A { get; private set; }

        public Color(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
            A = 255;
        }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public Color(int red, int green, int blue)
        {
            R = (byte)red;
            G = (byte)green;
            B = (byte)blue;
            A = 255;
        }

        public Color(int red, int green, int blue, int alpha)
        {
            R = (byte)red;
            G = (byte)green;
            B = (byte)blue;
            A = (byte)alpha;
        }

        public Color(float red, float green, float blue)
        {
            R = (byte)(255 * red);
            G = (byte)(255 * green);
            B = (byte)(255 * blue);
            A = 255;
        }

        public Color(float red, float green, float blue, float alpha)
        {
            R = (byte)(255 * red);
            G = (byte)(255 * green);
            B = (byte)(255 * blue);
            A = (byte)(255 * alpha);
        }

        static public Color CornflowerBlue
        {
            get
            {
                return new Color(100, 149, 237);
            }
        }

        static public Color Black
        {
            get
            {
                return new Color(0, 0, 0);
            }
        }
    }
}
