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

namespace Umbra.Structures
{
    public enum GridElementType
    {
        Perlin,
        Bicubic,
        PerlinBicubic
    }
    public class GridElement
    {
        GridElementType Type;

        float[,] Values;
        public GridElement(GridElementType type)
        {
            Type = type;
            Values = new float[Constants.WorldSize * Constants.ChunkSize, Constants.WorldSize * Constants.ChunkSize];

            if (type == GridElementType.Bicubic || type == GridElementType.PerlinBicubic)
            {
                int size = Constants.WorldSize + 3;
                float[,] InterpolantSeeds = new float[size, size];

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        InterpolantSeeds[x, y] = (float)LandscapeGenerator.Random.NextDouble();
                    }
                }



                for (int ChunkX = 0; ChunkX < Constants.WorldSize; ChunkX++)
                {
                    for (int ChunkY = 0; ChunkY < Constants.WorldSize; ChunkY++)
                    {

                        float[,] points = new float[4, 4];

                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                points[i, j] = InterpolantSeeds[i + ChunkX, j + ChunkY];
                            }
                        }

                        Interpolation.UpdateBicubicCoefficients(points);

                        for (int x = 0; x < Constants.ChunkSize; x++)
                        {
                            for (int y = 0; y < Constants.ChunkSize; y++)
                            {
                                Values[x + ChunkX * Constants.ChunkSize, y + ChunkY * Constants.ChunkSize] = Interpolation.BicubicInterpolation((float)x / (float)Constants.ChunkSize, (float)y / (float)Constants.ChunkSize);
                            }
                        }
                    }
                }
            }

            if (type == GridElementType.Perlin || type == GridElementType.PerlinBicubic)
            {
                Perlin.Generate(ref Values, Constants.WorldSize * Constants.ChunkSize);
            }

            for (int x = 0; x < Constants.WorldSize * Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.WorldSize * Constants.ChunkSize; y++)
                {
                    Values[x, y] *= Interpolation.Weight((float)x / (float)(Constants.WorldSize * Constants.ChunkSize), (float)y / (float)(Constants.WorldSize * Constants.ChunkSize));
                }
            }
        }

        public float GetChunkInterpolant(ChunkIndex index, BlockIndex block)
        {
            return Values[block.X + (int)index.Position.X, block.Z + (int)index.Position.Z];
        }
    }
}
