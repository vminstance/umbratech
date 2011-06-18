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
using Vector3 = Umbra.Structures.Internal.Vector3;
using Console = Umbra.Implementations.Console;

namespace Umbra.Structures.Internal
{
    public class Vector3
    {
        float X { get; private set; }
        float Y { get; private set; }
        float Z { get; private set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        static public Vector3 Right { get { return new Vector3(1, 0, 0); } }
        static public Vector3 Left { get { return new Vector3(-1, 0, 0); } }
        static public Vector3 Up { get { return new Vector3(0, 1, 0); } }
        static public Vector3 Down { get { return new Vector3(0, -1, 0); } }
        static public Vector3 Backward { get { return new Vector3(0, 0, 1); } }
        static public Vector3 Forward { get { return new Vector3(0, 0, -1); } }

        static public Vector3 UnitX { get { return new Vector3(1, 0, 0); } }
        static public Vector3 UnitY { get { return new Vector3(0, 1, 0); } }
        static public Vector3 UnitZ { get { return new Vector3(0, 0, 1); } }

        static public Vector3 operator +(Vector3 part1, Vector3 part2)
        {
            return new Vector3(part1.X + part2.X, part1.Y + part2.Y, part1.Y + part2.Y);
        }

        static public Vector3 operator -(Vector3 part1, Vector3 part2)
        {
            return new Vector3(part1.X - part2.X, part1.Y - part2.Y, part1.Y - part2.Y);
        }
    }
}
