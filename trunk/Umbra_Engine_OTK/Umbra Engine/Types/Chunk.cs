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
    class Chunk
    {

        public Vertex[] Vertices;
        public uint[] Indices;
        public uint[] Textures;

        uint[,,] Data;

        public Chunk(Vector3 Position)
        {
            int size = 32;
            int[,] world = new int[size,size];

            Random rand = new Random();

            for(int i = 0; i < size*size; i++)
            {
                world[i % size, i / size] = rand.Next(3);
            }

            Data = new uint[size, size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        if (size - 1 - y < world[x, z])
                        {
                            Data[x, y, z] = 0;
                        }
                        else
                        {
                            Data[x, y, z] = 1;
                        }
                    }
                }
            }
            Data[size - 1, size - 1, size - 1] = 0;



            uint FaceCount = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {

                    for (int z = 0; z < size; z++)
                    {
                        if (Data[x, y, z] != 0)
                        {
                            if (x == 0 || Data[x - 1, y, z] == 0)
                            {
                                FaceCount++;
                            }
                            if (y == 0 || Data[x, y - 1, z] == 0)
                            {
                                FaceCount++;
                            }
                            if (z == 0 || Data[x, y, z - 1] == 0)
                            {
                                FaceCount++;
                            }

                            if (x == size - 1 || Data[x + 1, y, z] == 0)
                            {
                                FaceCount++;
                            }
                            if (y == size - 1 || Data[x, y + 1, z] == 0)
                            {
                                FaceCount++;
                            }
                            if (z == size - 1 || Data[x, y, z + 1] == 0)
                            {
                                FaceCount++;
                            }
                        }
                    }
                }
            }

            Face tempFace;
            Vertices = new Vertex[FaceCount*4];
            Indices = new uint[FaceCount * 6];
            Textures = new uint[FaceCount];
            FaceCount = 0;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {

                    for (int z = 0; z < size; z++)
                    {
                        if (Data[x, y, z] != 0)
                        {
                            if (x == 0 || Data[x - 1, y, z] == 0)
                            {
                                tempFace = new Face(-Vector3.UnitX, FaceCount, new Vector3(x, y, z) + Position, Data[x, y, z]);
                                tempFace.Vertices.CopyTo(Vertices, FaceCount * 4);

                                tempFace.Indices.CopyTo(Indices, FaceCount * 6);

                                Textures[FaceCount] = tempFace.TextureID;

                                FaceCount++;
                            }
                            if (y == 0 || Data[x, y - 1, z] == 0)
                            {
                                tempFace = new Face(-Vector3.UnitY, FaceCount, new Vector3(x, y, z) + Position, Data[x, y, z]);
                                tempFace.Vertices.CopyTo(Vertices, FaceCount * 4);

                                tempFace.Indices.CopyTo(Indices, FaceCount * 6);

                                Textures[FaceCount] = tempFace.TextureID;

                                FaceCount++;
                            }
                            if (z == 0 || Data[x, y, z - 1] == 0)
                            {
                                tempFace = new Face(-Vector3.UnitZ, FaceCount, new Vector3(x, y, z) + Position, Data[x, y, z]);
                                tempFace.Vertices.CopyTo(Vertices, FaceCount * 4);

                                tempFace.Indices.CopyTo(Indices, FaceCount * 6);

                                Textures[FaceCount] = tempFace.TextureID;

                                FaceCount++;
                            }

                            if (x == size - 1 || Data[x + 1, y, z] == 0)
                            {
                                tempFace = new Face(Vector3.UnitX, FaceCount, new Vector3(x, y, z) + Position, Data[x, y, z]);
                                tempFace.Vertices.CopyTo(Vertices, FaceCount * 4);

                                tempFace.Indices.CopyTo(Indices, FaceCount * 6);

                                Textures[FaceCount] = tempFace.TextureID;

                                FaceCount++;
                            }
                            if (y == size - 1 || Data[x, y + 1, z] == 0)
                            {
                                tempFace = new Face(Vector3.UnitY, FaceCount, new Vector3(x, y, z) + Position, Data[x, y, z]);
                                tempFace.Vertices.CopyTo(Vertices, FaceCount * 4);

                                tempFace.Indices.CopyTo(Indices, FaceCount * 6);

                                Textures[FaceCount] = tempFace.TextureID;

                                FaceCount++;
                            }
                            if (z == size - 1 || Data[x, y, z + 1] == 0)
                            {
                                tempFace = new Face(Vector3.UnitZ, FaceCount, new Vector3(x, y, z) + Position, Data[x, y, z]);
                                tempFace.Vertices.CopyTo(Vertices, FaceCount * 4);

                                tempFace.Indices.CopyTo(Indices, FaceCount * 6);

                                Textures[FaceCount] = tempFace.TextureID;

                                FaceCount++;
                            }
                        }
                    }
                    Console.WriteLine("Pass " + x + "\t" + y);
                }
            }
        }
    }
}
