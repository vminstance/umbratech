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
    public class GameTime
    {
        public double TotalMilliseconds { get; private set; }
        public double ElapsedMilliseconds { get; private set; }

        public GameTime(double totalMilliseconds, double elapsedMilliseconds)
        {
            TotalMilliseconds = totalMilliseconds;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}
