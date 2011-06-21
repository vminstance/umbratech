using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Console = Umbra.Implementations.Console;

namespace Umbra.Structures
{
    public class Chunk
    {
        public VertexBuffer VertexBuffer { get; private set; }

        public ChunkIndex Index { get; private set; }

        ushort[, ,] Data;
        Octree VisibleFaces;

        public byte SetupState { get; set; }
        public bool HasData { get; set; }
        public bool WillBeUnloaded { get; set; }

        public ushort this[BlockIndex index]
        {
            get
            {
                return this[index.X, index.Y, index.Z];
            }
            set
            {
                this[index.X, index.Y, index.Z] = value;
            }
        }

        public ushort this[int x, int y, int z]
        {
            get
            {
                if (x >= 0 && x < Constants.ChunkSize && y >= 0 && y < Constants.ChunkSize && z >= 0 && z < Constants.ChunkSize)
                {
                    return Data[x, y, z];
                }
                else
                {
                    throw new Exception("Index was out of bounds!");
                }
            }
            set
            {
                if (x >= 0 && x < Constants.ChunkSize && y >= 0 && y < Constants.ChunkSize && z >= 0 && z < Constants.ChunkSize)
                {
                    Data[x, y, z] = value;
                    if (VisibleFaces != null)
                    {
                        VisibleFaces.SetVisibility(new BlockIndex(x, y, z), Block.GetVisibility(value));
                    }
                }
                else
                {
                    throw new Exception("Index was out of bounds!");
                }
            }
        }

        public Chunk(ChunkIndex index)
        {
            SetupState = 0;
            HasData = false;
            WillBeUnloaded = false;
            Index = index;
            Data = new ushort[Constants.ChunkSize, Constants.ChunkSize, Constants.ChunkSize];
        }

        public void SetBlock(BlockIndex index, ushort value, bool updateAdjacentChunks)
        {
            if (index.X >= 0 && index.X < Constants.ChunkSize && index.Y >= 0 && index.Y < Constants.ChunkSize && index.Z >= 0 && index.Z < Constants.ChunkSize)
            {
                Data[index.X, index.Y, index.Z] = value;
                BuildGeometry(true);

                if (updateAdjacentChunks)
                {
                    if (index.X == 0)
                    {
                        Chunk chunk = Constants.CurrentWorld.GetChunk(Index - ChunkIndex.UnitX);
                        if (chunk != null)
                        {
                            chunk.BuildGeometry(false);
                        }
                    }
                    else if (index.X == Constants.ChunkSize - 1)
                    {
                        Chunk chunk = Constants.CurrentWorld.GetChunk(Index + ChunkIndex.UnitX);
                        if (chunk != null)
                        {
                            chunk.BuildGeometry(false);
                        }
                    }

                    if (index.Y == 0)
                    {
                        Chunk chunk = Constants.CurrentWorld.GetChunk(Index - ChunkIndex.UnitY);
                        if (chunk != null)
                        {
                            chunk.BuildGeometry(false);
                        }
                    }
                    else if (index.Y == Constants.ChunkSize - 1)
                    {
                        Chunk chunk = Constants.CurrentWorld.GetChunk(Index + ChunkIndex.UnitY);
                        if (chunk != null)
                        {
                            chunk.BuildGeometry(false);
                        }
                    }

                    if (index.Z == 0)
                    {
                        Chunk chunk = Constants.CurrentWorld.GetChunk(Index - ChunkIndex.UnitZ);
                        if (chunk != null)
                        {
                            chunk.BuildGeometry(false);
                        }
                    }
                    else if (index.Z == Constants.ChunkSize - 1)
                    {
                        Chunk chunk = Constants.CurrentWorld.GetChunk(Index + ChunkIndex.UnitZ);
                        if (chunk != null)
                        {
                            chunk.BuildGeometry(false);
                        }
                    }
                }
            }
        }

        public void BuildOctree()
        {
            VisibleFaces = new Octree(Data, this);
        }

        public void BuildGeometry(bool updateOctree)
        {
            DisposeBuffers();

            if (updateOctree)
            {
                BuildOctree();
            }

            FaceList geometry = VisibleFaces.GetFaces();
            if (geometry.IsEmpty)
            {
                return;
            }
            VertexBuffer = geometry.GetVertexBuffer();
        }

        public void DisposeBuffers()
        {
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
        }
    }
}
