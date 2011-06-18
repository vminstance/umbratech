using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Console = Umbra.Implementations.Console;

namespace Umbra.Utilities
{
    public struct SmallBlockVertex
    {
        public byte PositionX;
        public byte PositionY;
        public byte PositionZ;
        public byte TextureX;

        public byte ColorR;
        public byte ColorG;
        public byte ColorB;
        public byte TextureY;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Byte4, VertexElementUsage.Position, 0),
            new VertexElement(4, VertexElementFormat.Byte4, VertexElementUsage.Color, 0)
        );
    }

    public struct CursorVertex
    {
        public Vector3 Position;
        public Color Color;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );

        public CursorVertex(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }
    }
}
