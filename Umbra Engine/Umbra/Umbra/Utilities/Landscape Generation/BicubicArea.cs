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

namespace Umbra.Utilities
{
    static public class BicubicArea
    {
        static float[,] ChunkValues;
        static public void Generate(ref float[,] currentLandscape)
        {
            int size = Constants.WorldSize + 3;

            ChunkValues = new float[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {

                    if (x > 0 && y > 0)
                    {
                        ChunkValues[x, y] +=
                            ((((float)LandscapeGenerator.Random.NextDouble() * Constants.BicubicAmplitude / 5) - (Constants.BicubicAmplitude / 10)) +
                            ChunkValues[x, y - 1] +
                            ChunkValues[x - 1, y] +
                            ChunkValues[x - 1, y - 1]) / 3;
                    }
                    else if (x > 0)
                    {
                        ChunkValues[x, y] +=
                            ((((float)LandscapeGenerator.Random.NextDouble() * Constants.BicubicAmplitude / 10) - (Constants.BicubicAmplitude / 20)) +
                            ChunkValues[x - 1, y]) / 1.5F;
                    }
                    else if (y > 0)
                    {
                        ChunkValues[x, y] +=
                            ((((float)LandscapeGenerator.Random.NextDouble() * Constants.BicubicAmplitude / 10) - (Constants.BicubicAmplitude / 20)) +
                            ChunkValues[x, y - 1]) / 1.5F;
                    }
                    else
                    {
                        ChunkValues[x, y] += ((float)LandscapeGenerator.Random.NextDouble() * Constants.BicubicAmplitude) - Constants.BicubicAmplitude / 2;
                    }
                }
            }

            float[,] points = new float[4, 4];


            for (int chunkX = 1; chunkX < size - 2; chunkX++)
            {
                for (int chunkY = 1; chunkY < size - 2; chunkY++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            points[i, j] = ChunkValues[i - 1 + chunkX, j - 1 + chunkY];
                        }
                    }

                    Interpolation.UpdateBicubicCoefficients(points);

                    for (int x = 0; x < Constants.ChunkSize; x++)
                    {
                        for (int y = 0; y < Constants.ChunkSize; y++)
                        {
                            currentLandscape[x + (chunkX - 1) * Constants.ChunkSize, y + (chunkY - 1) * Constants.ChunkSize] += Constants.WorldHeightOffset + Interpolation.BicubicInterpolation((float)x / (float)Constants.ChunkSize, (float)y / (float)Constants.ChunkSize);
                        }
                    }
                }
            }
        }
    }
}
