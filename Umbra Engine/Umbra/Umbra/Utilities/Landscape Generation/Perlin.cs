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
    static public class Perlin
    {

        static public void Generate(ref float[,] currentLandscape, int size)
        {

            float[,] tempData;
            for (int i = 2; i <= Math.Log(size, 2); i++)
            {
                tempData = GenerateNoise(size, (int)Math.Pow(2, i));

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        currentLandscape[x, y] += tempData[x, y];
                    }
                }
            }
        }

        static private float[,] GenerateNoise(int size, int level)
        {

            float weight = (float)level / (float)size;

            float[,] data = new float[size + 1, size + 1];

            for (int i = 0; i <= size; i++)
            {
                if (i % level == 0)
                {
                    for (int j = 0; j <= size; j++)
                    {
                        if (j % level == 0)
                        {
                            data[i, j] = (float)LandscapeGenerator.Random.NextDouble() * weight;
                        }
                    }
                }
            }
            float[,] points;
            for (int x = 0; x <= size; x++)
            {
                for (int y = 0; y <= size; y++)
                {
                    if (y % level != 0 || x % level != 0)
                    {

                        points = new float[,] {
                                  { 
                                      data[(int)Math.Max(x - (x % level), 0), (int)Math.Max(y - (y % level), 0)],
                                      data[(int)Math.Max(x - (x % level), 0), (int)Math.Min(y + (level - (y % level)), size)]
                                  },
                                  {
                                      data[(int)Math.Min(x + (level - (x % level)), size), (int)Math.Max(y - (y % level), 0)],
                                      data[(int)Math.Min(x + (level - (x % level)), size), (int)Math.Min(y + (level - (y % level)), size)]
                                  }
                              };

                        data[x, y] = Interpolation.BilinearInterpolation(points, ((float)x / level) % 1, ((float)y / level) % 1);
                    }
                }
            }

            return data;
        }
    }
}