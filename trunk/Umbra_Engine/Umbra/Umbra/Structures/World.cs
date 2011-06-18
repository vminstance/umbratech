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
    public class World
    {
        Chunk[, ,] LoadedChunks;
        static float[,] ChunkValues;

        public ChunkIndex Offset { get; private set; }

        public string Path 
        {
            get
            {
                return Constants.WorldPath + @"\" + Name;
            }
        }
        public string Name { get; private set; }

        public World(string name)
        {
            Name = name;

            LoadedChunks = new Chunk[Constants.WorldSize, Constants.WorldSize, Constants.WorldSize];
            Offset = ChunkIndex.One * (-Constants.WorldSize / 2);

            ChunkValues = new float[Constants.WorldSize + 3, Constants.WorldSize + 3];
        }

        public void Initialize()
        {
            for (int x = 0; x < Constants.WorldSize + 3; x++)
            {
                for (int y = 0; y < Constants.WorldSize + 3; y++)
                {
                    if (x < Constants.WorldSize && y < Constants.WorldSize)
                    {
                        for (int z = 0; z < Constants.WorldSize; z++)
                        {
                            LoadedChunks[x, y, z] = ChunkManager.ObtainChunk(Offset + new ChunkIndex(x, y, z));
                        }
                    }
                    ChunkValues[x, y] = ((float)LandscapeGenerator.Random.NextDouble() * Constants.BicubicAmplitude) - Constants.BicubicAmplitude / 2;
                }
            }
        }

        public void GetLandscape(ChunkIndex chunkIndex, ref float[,] returnValue)
        {
            returnValue = new float[Constants.ChunkSize, Constants.ChunkSize];
            ChunkIndex localIndex = chunkIndex - Offset;


            if (localIndex.X < 0 || localIndex.X >= Constants.WorldSize + 3 || localIndex.Z < 0 || localIndex.Z >= Constants.WorldSize + 3)
            {
                return;
            }

            float[,] points = new float[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                      points[i, j] = ChunkValues[i + localIndex.X, j + localIndex.Z];
                }
            }

            Interpolation.UpdateBicubicCoefficients(points);

            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.ChunkSize; y++)
                {
                    returnValue[x, y] = Interpolation.BicubicInterpolation((float)x / (float)Constants.ChunkSize, (float)y / (float)Constants.ChunkSize);
                }
            }
        }

        public void OffsetChunks(ChunkIndex offset)
        {
            if (offset == ChunkIndex.Zero)
            {
                return;
            }

            Offset += offset;

            Chunk[, ,] newArray = new Chunk[Constants.WorldSize, Constants.WorldSize, Constants.WorldSize];
            float[,] newValueArray = new float[Constants.WorldSize + 3, Constants.WorldSize + 3];

            for (int x = 0; x < Constants.WorldSize + 3; x++)
            {
                for (int y = 0; y < Constants.WorldSize + 3; y++)
                {
                    if (x < Constants.WorldSize && y < Constants.WorldSize)
                    {
                        for (int z = 0; z < Constants.WorldSize; z++)
                        {
                            int newX = x + offset.X;
                            int newY = y + offset.Y;
                            int newZ = z + offset.Z;

                            if (newX < 0 || newX >= Constants.WorldSize || newY < 0 || newY >= Constants.WorldSize || newZ < 0 || newZ >= Constants.WorldSize)
                            {
                                ChunkManager.UnloadChunk(LoadedChunks[Constants.WorldSize - x - 1, Constants.WorldSize - y - 1, Constants.WorldSize - z - 1]);
                                newArray[x, y, z] = ChunkManager.ObtainChunk(Offset + new ChunkIndex(x, y, z));
                            }
                            else
                            {
                                newArray[x, y, z] = LoadedChunks[newX, newY, newZ];

                                if ((newX == 0 && offset.X < 0) || (newX == Constants.WorldSize - 1 && offset.X > 0) ||
                                    (newY == 0 && offset.Y < 0) || (newY == Constants.WorldSize - 1 && offset.Y > 0) ||
                                    (newZ == 0 && offset.Z < 0) || (newZ == Constants.WorldSize - 1 && offset.Z > 0) ||
                                    !newArray[x, y, z].IsSetup)
                                {
                                    newArray[x, y, z].IsSetup = false;
                                    ChunkManager.Setup.AddToSetup(newArray[x, y, z]);
                                }
                            }
                        }
                    }

                    if (new ChunkIndex(offset.X, 0, offset.Z) != ChunkIndex.Zero)
                    {
                        int newValueX = x + offset.X;
                        int newValueY = y + offset.Z;

                        if (newValueX < 0 || newValueX >= Constants.WorldSize + 3 || newValueY < 0 || newValueY >= Constants.WorldSize + 3)
                        {
                            newValueArray[x, y] = ((float)LandscapeGenerator.Random.NextDouble() * Constants.BicubicAmplitude) - Constants.BicubicAmplitude / 2;
                        }
                        else
                        {
                            newValueArray[x, y] = ChunkValues[newValueX, newValueY];
                        }
                    }
                }
            }

            LoadedChunks = newArray;

            if (new ChunkIndex(offset.X, 0, offset.Z) != ChunkIndex.Zero)
            {
                ChunkValues = newValueArray;
            }
        }

        public Chunk GetChunk(ChunkIndex index)
        {
            ChunkIndex indexRelative = index - Offset;

            if (indexRelative.X < 0 || indexRelative.X >= Constants.WorldSize || indexRelative.Y < 0 || indexRelative.Y >= Constants.WorldSize || indexRelative.Z < 0 || indexRelative.Z >= Constants.WorldSize)
            {
                return null;
            }

            Chunk returnChunk = LoadedChunks[indexRelative.X, indexRelative.Y, indexRelative.Z];

            if (returnChunk.Index != index)
            {
                return null;
            }

            return returnChunk;
        }

        public ushort GetBlock(BlockIndex index)
        {
            Chunk chunk = GetChunk(new ChunkIndex(index.Position));

            if (chunk != null)
            {
                return chunk[index - chunk.Index];
            }
            else
            {
                return Block.Vacuum;
            }
        }

        public void SetBlock(BlockIndex index, ushort type)
        {
            Chunk chunk = GetChunk(new ChunkIndex(index.Position));

            if (chunk != null)
            {
                chunk.SetBlock(index - chunk.Index, type, true);
            }
        }

        public Chunk[, ,] GetChunks()
        {
            return LoadedChunks;
        }
    }
}
