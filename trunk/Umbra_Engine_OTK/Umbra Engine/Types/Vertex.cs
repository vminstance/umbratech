using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;

namespace Umbra_Engine
{
    struct Vertex
    {
        public Vector2 Texture;
        public Color4 Color;
        public Vector3 Normal;
        public Vector3 Position;
    }
}
