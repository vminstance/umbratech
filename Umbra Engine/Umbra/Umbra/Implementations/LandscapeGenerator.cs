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
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra.Implementations
{
    static public class LandscapeGenerator
    {
        static private int Seed;

        static public void Initialize(string seed)
        {
            Seed = seed.GetHashCode();
        }

        static private float[,] GetLandscapeHeight(ChunkIndex index)
        {
            int realX = (int)index.Position.X;
            int realY = (int)index.Position.Z;

            float[,] dataPoints = new float[4, 4];

            dataPoints[0, 0] = Perlin.GetPerlin(realX - Constants.World.ChunkSize, realY - Constants.World.ChunkSize, 5, Seed);
            dataPoints[0, 1] = Perlin.GetPerlin(realX - Constants.World.ChunkSize, realY, 5, Seed);
            dataPoints[0, 2] = Perlin.GetPerlin(realX - Constants.World.ChunkSize, realY + Constants.World.ChunkSize, 5, Seed);
            dataPoints[0, 3] = Perlin.GetPerlin(realX - Constants.World.ChunkSize, realY + (Constants.World.ChunkSize * 2), 5, Seed);

            dataPoints[1, 0] = Perlin.GetPerlin(realX, realY - Constants.World.ChunkSize, 5, Seed);
            dataPoints[1, 1] = Perlin.GetPerlin(realX, realY, 5, Seed);
            dataPoints[1, 2] = Perlin.GetPerlin(realX, realY + Constants.World.ChunkSize, 5, Seed);
            dataPoints[1, 3] = Perlin.GetPerlin(realX, realY + (Constants.World.ChunkSize * 2), 5, Seed);

            dataPoints[2, 0] = Perlin.GetPerlin(realX + Constants.World.ChunkSize, realY - Constants.World.ChunkSize, 5, Seed);
            dataPoints[2, 1] = Perlin.GetPerlin(realX + Constants.World.ChunkSize, realY, 5, Seed);
            dataPoints[2, 2] = Perlin.GetPerlin(realX + Constants.World.ChunkSize, realY + Constants.World.ChunkSize, 5, Seed);
            dataPoints[2, 3] = Perlin.GetPerlin(realX + Constants.World.ChunkSize, realY + (Constants.World.ChunkSize * 2), 5, Seed);

            dataPoints[3, 0] = Perlin.GetPerlin(realX + (Constants.World.ChunkSize * 2), realY - Constants.World.ChunkSize, 5, Seed);
            dataPoints[3, 1] = Perlin.GetPerlin(realX + (Constants.World.ChunkSize * 2), realY, 5, Seed);
            dataPoints[3, 2] = Perlin.GetPerlin(realX + (Constants.World.ChunkSize * 2), realY + Constants.World.ChunkSize, 5, Seed);
            dataPoints[3, 3] = Perlin.GetPerlin(realX + (Constants.World.ChunkSize * 2), realY + (Constants.World.ChunkSize * 2), 5, Seed);

            Interpolation.UpdateBicubicCoefficients(dataPoints);


            float[,] data = new float[Constants.World.ChunkSize, Constants.World.ChunkSize];

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.World.ChunkSize; y++)
                {
                    data[x, y] = Perlin.GetPerlin(x + realX, y + realY, 5, Seed) * Interpolation.BicubicInterpolation((float)x / (float)Constants.World.ChunkSize, (float)y / (float)Constants.World.ChunkSize);
                }
            }

            return data;
        }

        static public void SetChunkTerrain(Chunk chunk)
        {
            float[,] heightData = GetLandscapeHeight(chunk.Index);
            int absoluteHeight;

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int z = 0; z < Constants.World.ChunkSize; z++)
                {
                    int height = (int)(heightData[x, z] * 50.0F) + Constants.Landscape.WorldHeightOffset;
                    for (int y = 0; y < Constants.World.ChunkSize; y++)
                    {
                        absoluteHeight = y + (int)(chunk.Index).Position.Y;

                        if (absoluteHeight > height)
                        {
                            if (absoluteHeight < 10)
                            {
                                chunk[x, y, z] = Block.Water;
                            }
                            else
                            {
                                chunk[x, y, z] = Block.Air;
                            }
                        }
                        else if (absoluteHeight > height - 1)
                        {
                            if (absoluteHeight < 12)
                            {
                                chunk[x, y, z] = Block.Sand;
                            }
                            else
                            {
                                chunk[x, y, z] = Block.Grass;
                            }
                        }
                        else if (absoluteHeight > height - 3)
                        {
                            if (absoluteHeight < 2)
                            {
                                chunk[x, y, z] = Block.Sand;
                            }
                            else
                            {
                                chunk[x, y, z] = Block.Dirt;
                            }
                        }
                        else
                        {
                            chunk[x, y, z] = Block.Stone;
                        }
                    }
                }
            }
        }
    }
}
