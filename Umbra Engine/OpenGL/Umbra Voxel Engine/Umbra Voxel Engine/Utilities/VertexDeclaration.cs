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
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Utilities
{
    public struct VoxelVertex
    {
        public uint Data;

        public VoxelVertex(uint data)
        {
            Data = data;
        }
    }

    public class VertexBuffer
    {
        int ID;
        int Count;
        int ArrayID;
        ChunkIndex Offset;

        public VertexBuffer()
        {
        }

        public VertexBuffer(ChunkIndex offset)
        {
            Offset = offset;
        }

        public void SetData(VoxelVertex[] vertices)
        {
            GL.GenVertexArrays(1, out ArrayID);
            GL.BindVertexArray(ArrayID);

            GL.GenBuffers(1, out ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);

            GL.BufferData<VoxelVertex>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(int)), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribIPointer(Shaders.DataID, 1, VertexAttribIPointerType.UnsignedInt, sizeof(int), IntPtr.Zero);

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

        public void Render()
        {
            if (Count > 0)
            {
                GL.PointSize(20.0F);

                Matrix4 world = Matrix4.CreateTranslation((Vector3)Offset.Position);
                GL.UniformMatrix4(Shaders.WorldMatrixID, false, ref world);

                GL.BindVertexArray(ArrayID);
                GL.EnableVertexAttribArray(Shaders.DataID);

                GL.DrawArrays(OpenTK.Graphics.OpenGL.BeginMode.Quads, 0, Count);

                GL.DisableVertexAttribArray(Shaders.DataID);
                GL.BindVertexArray(0);
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffers(1, ref ID);
        }
    }
}
