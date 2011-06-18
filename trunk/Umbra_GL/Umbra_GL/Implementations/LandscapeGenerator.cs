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

namespace Umbra.Implementations
{
    static public class LandscapeGenerator
    {
        static public Random Random;
        static public int Size = Constants.WorldSize * Constants.ChunkSize;

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
                Layers[x] = new GridLayer(GridElementType.Bicubic, 4);
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

        static private void LandscapeHeight(ref float[,] values, ChunkIndex index, GridLayer layer)
        {
            ChunkIndex relativeIndex = index - layer.CurrentOffset + layer.FixedOffset;

            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.ChunkSize; y++)
                {
                    values[x, y] += layer.GetLandscapePoint(relativeIndex, new BlockIndex(x, 0, y));
                }
            }
        }

        static private void GetLandscape(ref float[,] values, Chunk chunk)
        {
            values = new float[Constants.ChunkSize, Constants.ChunkSize];
            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.ChunkSize; y++)
                {
                    values[x, y] = 0.0F;
                }
            }

            foreach (GridLayer layer in Layers)
            {
                LandscapeHeight(ref values, chunk.Index, layer);
            }
        }

        static float[,] Values;
        static public void SetChunkTerrain(Chunk chunk)
        {
            int height;
            int absoluteHeight;
            GetLandscape(ref Values, chunk);

            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int z = 0; z < Constants.ChunkSize; z++)
                {
                    height = (int)Values[x, z] + Constants.WorldHeightOffset;

                    for (int y = 0; y < Constants.ChunkSize; y++)
                    {
                        absoluteHeight = y + (int)(chunk.Index).Position.Y;

                        if (absoluteHeight > height)
                        {
                            if (absoluteHeight < 0)
                            {
                                chunk.SetBlock(new BlockIndex(x, y, z), Block.Water);
                            }
                            else
                            {
                                chunk.SetBlock(new BlockIndex(x, y, z), Block.Air);
                            }
                        }
                        else if (absoluteHeight > height - 1)
                        {
                            if (absoluteHeight < 2)
                            {
                                chunk.SetBlock(new BlockIndex(x, y, z), Block.Sand);
                            }
                            else
                            {
                                chunk.SetBlock(new BlockIndex(x, y, z), Block.Grass);
                            }
                        }
                        else if (absoluteHeight > height - 3)
                        {
                            if (absoluteHeight < 2)
                            {
                                chunk.SetBlock(new BlockIndex(x, y, z), Block.Sand);
                            }
                            else
                            {
                                chunk.SetBlock(new BlockIndex(x, y, z), Block.Dirt);
                            }
                        }
                        else
                        {
                            chunk.SetBlock(new BlockIndex(x, y, z), Block.Stone);
                        }
                    }
                }
            }
        }
    }
}
