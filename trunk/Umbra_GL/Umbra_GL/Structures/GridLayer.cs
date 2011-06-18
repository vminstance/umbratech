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
    public class GridLayer
    {
        GridElement[,] Layer;
        public WorldIndex FixedOffset;
        public WorldIndex CurrentOffset;
        public GridElementType Type;

        int Size;

        public GridLayer(GridElementType type, int size)
        {
            Size = size;
            Type = type;
            Layer = new GridElement[Size, Size];

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Layer[x, y] = new GridElement(Type);
                }
            }

            CurrentOffset = WorldIndex.Zero;
        }

        public float GetLandscapePoint(ChunkIndex index, BlockIndex block)
        {
            WorldIndex rounded = new WorldIndex(index + WorldIndex.One * 1.5F);

            ChunkIndex newIndex = (index + WorldIndex.One) % Constants.WorldSize;

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
                        newArray[x, y] = new GridElement(Type);
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
