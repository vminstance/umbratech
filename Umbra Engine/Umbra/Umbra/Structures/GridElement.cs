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
        public GridElement(GridElementType type, float range)
        {
            Type = type;
            Values = new float[Constants.World.WorldSize * Constants.World.ChunkSize, Constants.World.WorldSize * Constants.World.ChunkSize];

            if (type == GridElementType.Bicubic || type == GridElementType.PerlinBicubic)
            {
                int size = Constants.World.WorldSize + 3;
                float[,] InterpolantSeeds = new float[size, size];

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        InterpolantSeeds[x, y] = (float)LandscapeGenerator.Random.NextDouble() * range;
                    }
                }



                for (int ChunkX = 0; ChunkX < Constants.World.WorldSize; ChunkX++)
                {
                    for (int ChunkY = 0; ChunkY < Constants.World.WorldSize; ChunkY++)
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

                        for (int x = 0; x < Constants.World.ChunkSize; x++)
                        {
                            for (int y = 0; y < Constants.World.ChunkSize; y++)
                            {
                                Values[x + ChunkX * Constants.World.ChunkSize, y + ChunkY * Constants.World.ChunkSize] = Interpolation.BicubicInterpolation((float)x / (float)Constants.World.ChunkSize, (float)y / (float)Constants.World.ChunkSize);
                            }
                        }
                    }
                }
            }

            if (type == GridElementType.Perlin || type == GridElementType.PerlinBicubic)
            {
                Perlin.Generate(ref Values, Constants.World.WorldSize * Constants.World.ChunkSize);
            }
        }

        public float GetChunkInterpolant(ChunkIndex index, BlockIndex block)
        {
            return Values[block.X + (int)index.Position.X, block.Z + (int)index.Position.Z];
        }
    }
}
