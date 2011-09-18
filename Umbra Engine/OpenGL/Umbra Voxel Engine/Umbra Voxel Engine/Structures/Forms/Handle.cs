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
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures.Forms
{
    class Handle
    {
        Rectangle Location;
        Point? Grip;
        bool DynamicGrip;

        public Handle(Rectangle location, bool dynamicGrip)
        {
            Location = location;
            Grip = null;
            DynamicGrip = dynamicGrip;
        }

        public void Click(MouseButtonEventArgs e)
        {
            if (Location.Contains(e.Position))
            {
                if (DynamicGrip)
                {
                    Grip = new Point(e.Position.X - Location.X, e.Position.Y - Location.Y);
                }
                else
                {
                    Grip = Point.Empty;
                }
            }
        }

        public void Release(MouseButtonEventArgs e)
        {
            Grip = null;
        }

        public void Update()
        {
			Location.Location = new Point(Grip.Value.X + Constants.Engines.Input.MousePosition.X, Grip.Value.Y + Constants.Engines.Input.MousePosition.Y);
        }
    }
}
