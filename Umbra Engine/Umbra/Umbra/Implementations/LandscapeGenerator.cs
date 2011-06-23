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
        static public Random Random;
        static public int Size = Constants.World.WorldSize * Constants.World.ChunkSize;

        static GridLayer[] Layers;

        static public void Initialize(string seed)
        {
            if (seed != "time")
            {
                Random = new Random(seed.GetHashCode());
            }
            else
            {
                Random = new Random((int)System.Diagnostics.Stopwatch.GetTimestamp());
            }
            int layerCount = 4;
            Layers = new GridLayer[layerCount];
            for (int x = 0; x < layerCount; x++)
            {
                Layers[x] = new GridLayer(GridElementType.Bicubic, 3, (float)Random.NextDouble() * 5.0F);
            }
        }

        static public void GenerateTerrain()
        {

            Layers[0].FixedOffset = WorldIndex.Zero;
            Layers[1].FixedOffset = WorldIndex.UnitX / 2.0F;
            Layers[2].FixedOffset = WorldIndex.UnitY / 2.0F;
            Layers[3].FixedOffset = WorldIndex.One / 2.0F;

            Layers[0].CurrentOffset = WorldIndex.Zero;
            Layers[1].CurrentOffset = WorldIndex.Zero;
            Layers[2].CurrentOffset = WorldIndex.Zero;
            Layers[3].CurrentOffset = WorldIndex.Zero;
        }

        static public void OffsetLayerGrids(WorldIndex currentOffset)
        {
            foreach (GridLayer layer in Layers)
            {
                layer.OffsetGridElements(currentOffset);
            }
        }

        static private float GetLandscapeHeight(int x, int y, ChunkIndex index)
        {
            ChunkIndex relativeIndex;
            float weight;
            float totalWeight;

            float returnVal = 0;
            totalWeight = 0;

            foreach (GridLayer layer in Layers)
            {
                relativeIndex = index + layer.FixedOffset;
                weight = Interpolation.Weight((float)((((relativeIndex.X % Constants.World.WorldSize) + Constants.World.WorldSize) % Constants.World.WorldSize) * Constants.World.ChunkSize + x) / (float)(Constants.World.WorldSize * Constants.World.ChunkSize), (float)(((((relativeIndex.Z % Constants.World.WorldSize) + Constants.World.WorldSize) % Constants.World.WorldSize) * Constants.World.ChunkSize + y) / (float)(Constants.World.WorldSize * Constants.World.ChunkSize)));

                relativeIndex = index - layer.CurrentOffset + layer.FixedOffset;
                returnVal += layer.GetLandscapePoint(relativeIndex, new BlockIndex(x, 0, y)) * weight;
                totalWeight += weight;
            }
            returnVal /= totalWeight;

            return returnVal * 10;
        }

        static public void SetChunkTerrain(Chunk chunk)
        {
            int height;
            int absoluteHeight;

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int z = 0; z < Constants.World.ChunkSize; z++)
                {
                    height = (int)GetLandscapeHeight(x, z, chunk.Index) + Constants.Landscape.WorldHeightOffset;

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
