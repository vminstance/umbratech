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
    public class GridLayer
    {
        GridElement[,] Layer;
        public WorldIndex FixedOffset;
        public WorldIndex CurrentOffset;
        public GridElementType Type;

        int Size;

        float Range;

        public GridLayer(GridElementType type, int size, float range)
        {
            Size = size;
            Range = range;
            Type = type;
            Layer = new GridElement[Size, Size];

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Layer[x, y] = new GridElement(Type, Range);
                }
            }

            FixedOffset = WorldIndex.Zero;
            CurrentOffset = WorldIndex.Zero;
        }

        public float GetLandscapePoint(ChunkIndex index, BlockIndex block)
        {
            WorldIndex rounded = new WorldIndex(index + WorldIndex.One * 1.5F);
            ChunkIndex newIndex = (index + WorldIndex.One) % Constants.World.WorldSize;

            return Layer[(int)rounded.X % Size, (int)rounded.Y % Size].GetChunkInterpolant(newIndex, block);
        }

        public void OffsetGridElements(WorldIndex offset)
        {
            if (offset.X == 0 && offset.Y == 0)
            {
                return;
            }

            CurrentOffset += offset;

            GridElement[,] newArray = new GridElement[Size, Size];

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int newX = x + (int)offset.X;
                    int newY = y + (int)offset.Y;

                    if (newX < 0 || newX >= Size || newY < 0 || newY >= Size)
                    {
                        newArray[x, y] = new GridElement(Type, Range);
                    }
                    else
                    {
                        newArray[x, y] = Layer[newX, newY];
                    }
                }
            }
            Layer = newArray;
        }
    }
}
