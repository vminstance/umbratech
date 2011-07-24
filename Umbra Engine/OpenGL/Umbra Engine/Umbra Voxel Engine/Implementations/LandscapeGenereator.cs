using System;
using System.Linq;
using System.Text;
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
using Umbra.Utilities.Landscape;
using Umbra.Structures.Geometry;
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

            if (seed == "")
            {
                Seed = (int)System.Diagnostics.Stopwatch.GetTimestamp();
            }

            Vegetation.Initialize(Seed);
        }

        static private float[,] GetLandscapeHeight(ChunkIndex index)
        {
            int squareSize = Constants.World.ChunkSize;

            int x = (int)index.Position.X;
            int y = (int)index.Position.Z;

            int xInSquare = Mathematics.AbsModulo(x, squareSize);
            int yInSquare = Mathematics.AbsModulo(y, squareSize);

            float[,] dataPoints = new float[4, 4];

            dataPoints[0, 0] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare - squareSize, Seed);
            dataPoints[0, 1] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare, Seed);
            dataPoints[0, 2] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare + squareSize, Seed);
            dataPoints[0, 3] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare + squareSize * 2, Seed);

            dataPoints[1, 0] = Noise.GetPerlin(x - xInSquare, y - yInSquare - squareSize, Seed);
            dataPoints[1, 1] = Noise.GetPerlin(x - xInSquare, y - yInSquare, Seed);
            dataPoints[1, 2] = Noise.GetPerlin(x - xInSquare, y - yInSquare + squareSize, Seed);
            dataPoints[1, 3] = Noise.GetPerlin(x - xInSquare, y + y - yInSquare + squareSize * 2, Seed);

            dataPoints[2, 0] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare - squareSize, Seed);
            dataPoints[2, 1] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare, Seed);
            dataPoints[2, 2] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare + squareSize, Seed);
            dataPoints[2, 3] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare + squareSize * 2, Seed);

            dataPoints[3, 0] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare - squareSize, Seed);
            dataPoints[3, 1] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare, Seed);
            dataPoints[3, 2] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare + squareSize, Seed);
            dataPoints[3, 3] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare + squareSize * 2, Seed);

            Interpolation.UpdateBicubicCoefficients(dataPoints);


            float[,] data = new float[Constants.World.ChunkSize, Constants.World.ChunkSize];

            int xBlockInSquare = 0;
            int yBlockInSquare = 0;

            for (int blockX = 0; blockX < Constants.World.ChunkSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.World.ChunkSize; blockY++)
                {
                    xBlockInSquare = blockX + xInSquare;
                    yBlockInSquare = blockY + yInSquare;

                    data[blockX, blockY] = Interpolation.Linear(
                        Noise.GetPerlin(x + blockX, y + blockY, Seed),
                        Interpolation.Bicubic((float)xBlockInSquare / (float)squareSize, (float)yBlockInSquare / (float)squareSize),
                        Constants.Landscape.PerlinBicubicWeight
                        );

                    data[blockX, blockY] = (data[blockX, blockY] * Constants.Landscape.WorldHeightAmplitude) + (float)Constants.Landscape.WorldHeightOffset;
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
                    float height = heightData[x, z];

                    for (int y = 0; y < Constants.World.ChunkSize; y++)
                    {
                        absoluteHeight = y + (int)(chunk.Index).Position.Y;

                        if (absoluteHeight > height)
                        {
                            if (absoluteHeight < 0)
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
                            if (absoluteHeight < 2)
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

        static public float GetLandscapeHeight(Vector2 position)
        {
            int squareSize = Constants.World.ChunkSize;

            int x = (int)position.X;
            int y = (int)position.Y;

            int xInSquare = Mathematics.AbsModulo(x, squareSize);
            int yInSquare = Mathematics.AbsModulo(y, squareSize);

            float[,] dataPoints = new float[4, 4];

            dataPoints[0, 0] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare - squareSize, Seed);
            dataPoints[0, 1] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare, Seed);
            dataPoints[0, 2] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare + squareSize, Seed);
            dataPoints[0, 3] = Noise.GetPerlin(x - xInSquare - squareSize, y - yInSquare + squareSize * 2, Seed);

            dataPoints[1, 0] = Noise.GetPerlin(x - xInSquare, y - yInSquare - squareSize, Seed);
            dataPoints[1, 1] = Noise.GetPerlin(x - xInSquare, y - yInSquare, Seed);
            dataPoints[1, 2] = Noise.GetPerlin(x - xInSquare, y - yInSquare + squareSize, Seed);
            dataPoints[1, 3] = Noise.GetPerlin(x - xInSquare, y + y - yInSquare + squareSize * 2, Seed);

            dataPoints[2, 0] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare - squareSize, Seed);
            dataPoints[2, 1] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare, Seed);
            dataPoints[2, 2] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare + squareSize, Seed);
            dataPoints[2, 3] = Noise.GetPerlin(x - xInSquare + squareSize, y - yInSquare + squareSize * 2, Seed);

            dataPoints[3, 0] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare - squareSize, Seed);
            dataPoints[3, 1] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare, Seed);
            dataPoints[3, 2] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare + squareSize, Seed);
            dataPoints[3, 3] = Noise.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare + squareSize * 2, Seed);

            Interpolation.UpdateBicubicCoefficients(dataPoints);


            float data = Interpolation.Linear(
                        Noise.GetPerlin(x, y, Seed),
                        Interpolation.Bicubic((float)xInSquare / (float)squareSize, (float)yInSquare / (float)squareSize),
                        Constants.Landscape.PerlinBicubicWeight
                        );

            data = (data * Constants.Landscape.WorldHeightAmplitude) + (float)Constants.Landscape.WorldHeightOffset;

            return data;
        }

        static public float[] GetAdjacentTerrain(ChunkIndex index, Direction dir)
        {
            float[,] data;
            float[] returnValue = new float[Constants.World.ChunkSize];

            if (dir.GetVector3() == Vector3.UnitX)
            {
                data = GetLandscapeHeight(index + ChunkIndex.UnitX);

                for (int i = 0; i < Constants.World.ChunkSize; i++)
                {
                    returnValue[i] = data[0, i];
                }
            }
            else if (dir.GetVector3() == Vector3.UnitZ)
            {
                data = GetLandscapeHeight(index + ChunkIndex.UnitZ);

                for (int i = 0; i < Constants.World.ChunkSize; i++)
                {
                    returnValue[i] = data[i, 0];
                }
            }

            return returnValue;
        }
    }
}
