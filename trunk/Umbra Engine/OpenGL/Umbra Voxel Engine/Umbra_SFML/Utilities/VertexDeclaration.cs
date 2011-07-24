using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
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
using Umbra.Structures.Graphics;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Umbra.Utilities
{
    public struct SmallBlockVertex
    {
        public byte PositionX;
        public byte PositionY;
        public byte PositionZ;

        public byte ColorR;
        public byte ColorG;
        public byte ColorB;

        public byte TextureX;
        public byte TextureY;
    }

    public struct CursorVertex
    {
        public Vector3 Position;
        public Color Color;

        public CursorVertex(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }
    }

    public class VertexBuffer
    {
        int ID;
        int Count;
        int ArrayID;

        public VertexBuffer()
        {
        }

        public void SetData<VertexType>(VertexType[] vertices) where VertexType : struct
        {
            GL.GenVertexArrays(1, out ArrayID);
            GL.BindVertexArray(ArrayID);

            GL.GenBuffers(1, out ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);

            GL.BufferData<VertexType>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * 8 * sizeof(byte)), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(Shaders.PositionDataID, 3, VertexAttribPointerType.UnsignedByte, false, 8 * sizeof(byte), 0);
            GL.VertexAttribPointer(Shaders.ColorDataID, 3, VertexAttribPointerType.UnsignedByte, true, 8 * sizeof(byte), 3 * sizeof(byte));
            GL.VertexAttribPointer(Shaders.TextureDataID, 2, VertexAttribPointerType.UnsignedByte, false, 8 * sizeof(byte), 6 * sizeof(byte));


            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            Count = vertices.Length;
        }

        public int GetID()
        {
            return ID;
        }

        public int GetCount()
        {
            return Count;
        }

        public void Render(ChunkIndex offset)
        {
            if (Count > 0)
            {
                Matrix4 world = Matrix4.CreateTranslation(offset.Position);
                GL.UniformMatrix4(Shaders.WorldMatrixID, false, ref world);

                GL.BindVertexArray(ArrayID);

                GL.EnableVertexAttribArray(Shaders.PositionDataID);
                GL.EnableVertexAttribArray(Shaders.ColorDataID);
                GL.EnableVertexAttribArray(Shaders.TextureDataID);

                GL.DrawArrays(OpenTK.Graphics.OpenGL.BeginMode.Quads, 0, Count);

                GL.DisableVertexAttribArray(Shaders.PositionDataID);
                GL.DisableVertexAttribArray(Shaders.ColorDataID);
                GL.DisableVertexAttribArray(Shaders.TextureDataID);

                GL.BindVertexArray(0);
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffers(1, ref ID);
        }
    }
}
