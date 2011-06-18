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
    class Face
    {
        public Vertex[] Vertices;
        public uint[] Indices;

        public uint TextureID;

        public Face(Vector3 Normal, uint Offset, Vector3 Position, uint ID)
        {

            TextureID = ID;
            Vertices = new Vertex[4];

            if (Normal == Vector3.UnitX)
            {
                Vertices[0].Position = new Vector3(1, 1, 0);
                Vertices[1].Position = new Vector3(1, 1, 1);
                Vertices[2].Position = new Vector3(1, 0, 0);
                Vertices[3].Position = new Vector3(1, 0, 1);
            }
            else if (Normal == -Vector3.UnitX)
            {
                Vertices[0].Position = new Vector3(0, 1, 1);
                Vertices[1].Position = new Vector3(0, 1, 0);
                Vertices[2].Position = new Vector3(0, 0, 1);
                Vertices[3].Position = new Vector3(0, 0, 0);
            }
            else if (Normal == Vector3.UnitY)
            {
                Vertices[0].Position = new Vector3(1, 1, 0);
                Vertices[1].Position = new Vector3(0, 1, 0);
                Vertices[2].Position = new Vector3(1, 1, 1);
                Vertices[3].Position = new Vector3(0, 1, 1);
            }
            else if (Normal == -Vector3.UnitY)
            {
                Vertices[0].Position = new Vector3(0, 0, 0);
                Vertices[1].Position = new Vector3(1, 0, 0);
                Vertices[2].Position = new Vector3(0, 0, 1);
                Vertices[3].Position = new Vector3(1, 0, 1);
            }
            else if (Normal == Vector3.UnitZ)
            {
                Vertices[0].Position = new Vector3(1, 1, 1);
                Vertices[1].Position = new Vector3(0, 1, 1);
                Vertices[2].Position = new Vector3(1, 0, 1);
                Vertices[3].Position = new Vector3(0, 0, 1);
            }
            else if (Normal == -Vector3.UnitZ)
            {
                Vertices[0].Position = new Vector3(0, 1, 0);
                Vertices[1].Position = new Vector3(1, 1, 0);
                Vertices[2].Position = new Vector3(0, 0, 0);
                Vertices[3].Position = new Vector3(1, 0, 0);
            }

            Vertices[0].Position += Position;
            Vertices[1].Position += Position;
            Vertices[2].Position += Position;
            Vertices[3].Position += Position;

            Vertices[0].Color = Color.White;
            Vertices[1].Color = Color.White;
            Vertices[2].Color = Color.White;
            Vertices[3].Color = Color.White;

            Vertices[0].Texture = new Vector2(0, 0);
            Vertices[1].Texture = new Vector2(1, 0);
            Vertices[2].Texture = new Vector2(0, 1);
            Vertices[3].Texture = new Vector2(1, 1);

            Vertices[0].Normal = Normal;
            Vertices[1].Normal = Normal;
            Vertices[2].Normal = Normal;
            Vertices[3].Normal = Normal;

            Indices = new uint[6];
            // Face one
            Indices[0] = 0 + Offset * 4;
            Indices[1] = 1 + Offset * 4;
            Indices[2] = 2 + Offset * 4;
            Indices[3] = 2 + Offset * 4;
            Indices[4] = 1 + Offset * 4;
            Indices[5] = 3 + Offset * 4;
        }
    }
}
