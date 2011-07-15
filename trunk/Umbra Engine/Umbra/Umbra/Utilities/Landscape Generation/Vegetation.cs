using System;
using System.IO;
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

namespace Umbra.Utilities
{
    static public class Vegetation
    {
        static private int Seed;

        static public void Initialize(int seed)
        {
            Seed = seed;
        }


        static public void Vegetate(Chunk chunk)
        {
            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int z = 0; z < Constants.World.ChunkSize; z++)
                {
                    for (int y = 0; y < Constants.World.ChunkSize; y++)
                    {
                        if (chunk[x, y, z].Type == Block.Grass.Type)
                        {
                            GrowTree(chunk, new BlockIndex(x, y, z), (int)(Perlin.GetByValues(x * x * z + y * z, chunk.Index.X * chunk.Index.X * chunk.Index.Z + chunk.Index.Y * chunk.Index.Z, Seed) * Constants.Landscape.Vegetation.TreeVaryHeight + Constants.Landscape.Vegetation.TreeMinHeight));
                        }
                    }
                }
            }
        }

        static private void GrowTree(Chunk chunk, BlockIndex originPoint, int treeSize)
        {
            BlockIndex point = originPoint + BlockIndex.UnitY;

            int trunkHeight = (int)Math.Floor((float)treeSize * 4.0F / 5.0F);
            float leafRadius = (float)trunkHeight / 2.0F;
            int leafCenter = (int)Math.Floor((float)treeSize * 2.0F / 3.0F);


            // Check whether or not tree can be placed at all

            BlockIndex blockWorldPos;

            List<BlockIndex> leafPositions = new List<BlockIndex>();
            List<BlockIndex> trunkPositions = new List<BlockIndex>();

            for (int x = -(int)leafRadius; x <= leafRadius; x++)
            {
                for (int z = -(int)leafRadius; z <= leafRadius; z++)
                {
                    for (int y = 0; y <= treeSize; y++)
                    {
                        blockWorldPos = new BlockIndex(point.X + x, point.Y + y, point.Z + z) + chunk.Index;
                        if (x == 0 && z == 0 && y <= trunkHeight)
                        {
                            if (Constants.World.Current.GetBlock(blockWorldPos).Type != Block.Air.Type)
                            {
                                return;
                            }
                            trunkPositions.Add(blockWorldPos);
                        }
                        else
                        {
                            if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2) + Math.Pow(y - leafCenter, 2)) < leafRadius)
                            {
                                if (Constants.World.Current.GetBlock(blockWorldPos).Type != Block.Air.Type)
                                {
                                    return;
                                }
                                leafPositions.Add(blockWorldPos);
                            }
                        }
                    }
                }
            }

            if (Perlin.GetByValues(point.X * point.X * point.Z + point.Y * point.Z, chunk.Index.X * chunk.Index.X * chunk.Index.Z + chunk.Index.Y * chunk.Index.Z, Seed) > Constants.Landscape.Vegetation.TreeChance)
            {
                return;
            }

            foreach (BlockIndex index in leafPositions)
            {
                if (index.ToChunkIndex() == chunk.Index)
                {
                    chunk[index - chunk.Index] = Block.Leaves;
                }
                else
                {
                    Constants.World.Current.SetBlock(index, Block.Leaves, false);
                }
            }

            foreach (BlockIndex index in trunkPositions)
            {
                if (index.ToChunkIndex() == chunk.Index)
                {
                    chunk[index - chunk.Index] = Block.Log;
                }
                else
                {
                    Constants.World.Current.SetBlock(index, Block.Log, false);
                }
            }
        }
    }
}
