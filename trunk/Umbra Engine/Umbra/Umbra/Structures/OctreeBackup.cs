/*using System;
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

namespace Umbra.Structures
{
    class Octree
    {
        Octree[, ,] Children;
        bool IsLeaf;
        BlockIndex AbsolutePosition;
        public BlockIndex Position;
        public int Size;
        BlockVisibility Type;

        public Octree()
        {
            Size = Constants.ChunkSize;
            Position = BlockIndex.Zero;
            AbsolutePosition = BlockIndex.Zero;
            Type = BlockVisibility.Invisible;
            IsLeaf = true;
            Children = new Octree[2, 2, 2];
        }

        public Octree(int size, BlockIndex position, BlockIndex absolutePosition, BlockVisibility type)
        {
            Size = size;
            Position = position;
            AbsolutePosition = absolutePosition;
            Type = type;
            IsLeaf = true;
            Children = new Octree[2, 2, 2];
        }

        public Octree(int size, BlockIndex position, Octree parent, BlockVisibility type)
        {
            Size = size;
            Position = position;
            AbsolutePosition = position + parent.AbsolutePosition;
            Type = type;
            IsLeaf = true;
            Children = new Octree[2, 2, 2];
        }

        public void Simplify()
        {
            Constants.SimplifyCount++;
            if (IsLeaf)
            {
                return;
            }

            foreach (Octree o in Children)
            {
                if (o != null)
                {
                    o.Simplify();
                }
            }

            bool isEqual = true;

            foreach (Octree o in Children)
            {
                if (!o.Compare(Children[0, 0, 0]))
                {
                    isEqual = false;
                    break;
                }
            }

            if (isEqual)
            {
                Type = Children[0, 0, 0].Type;
                Children = new Octree[2, 2, 2];
                IsLeaf = true;
            }
        }

        bool Compare(Octree oct)
        {
            if (oct.Type != Type || !(oct.IsLeaf && IsLeaf))
            {
                return false;
            }
            return true;
        }

        public BlockVisibility GetVisibility(BlockIndex position)
        {
            if (IsLeaf)
            {
                return Type;
            }
            else
            {
                position -= Position;
                Octree target = Children[position.X < Size / 2 ? 0 : 1, position.Y < Size / 2 ? 0 : 1, position.Z < Size / 2 ? 0 : 1];
                return target.GetVisibility(position);
            }
        }

        public void SetData(Block[, ,] data)
        {
            Octree[, ,] octrees = new Octree[Constants.ChunkSize, Constants.ChunkSize, Constants.ChunkSize];

            BuildFromBottom(Constants.ChunkSize / 2, octrees);
        }

        void BuildFromBottom(int level, Octree[, ,] children)
        {
            Octree[, ,] octrees = new Octree[level, level, level];

            int size = Constants.ChunkSize / level;

            for (int x = 0; x < level; x++)
            {
                for (int y = 0; y < level; y++)
                {
                    for (int z = 0; z < level; z++)
                    {
                        Octree[, ,] newChildren = new Octree[2, 2, 2];

                        bool areChildrenEqual = true;

                        for (int xInt = 0; xInt < 2; xInt++)
                        {
                            for (int yInt = 0; yInt < 2; yInt++)
                            {
                                for (int zInt = 0; zInt < 2; zInt++)
                                {
                                    if (!children[xInt + x * 2, yInt + y * 2, zInt + z * 2].Compare(children[0, 0, 0]))
                                    {
                                        areChildrenEqual = false;
                                    }
                                    newChildren[xInt, yInt, zInt] = children[xInt + x * 2, yInt + y * 2, zInt + z * 2];
                                }
                            }
                        }

                        if (areChildrenEqual)
                        {
                            octrees[x, y, z] = new Octree(size, new BlockIndex(x, y, z) % size, new BlockIndex(x, y, z) * size, children[0, 0, 0].Type);
                            octrees[x, y, z].IsLeaf = true;
                        }
                        else
                        {
                            octrees[x, y, z] = new Octree(size, new BlockIndex(x, y, z) % size, new BlockIndex(x, y, z) * size, BlockVisibility.Invisible);
                            octrees[x, y, z].Children = newChildren;
                        }

                        if (level == 1)
                        {
                            Children = octrees;
                        }
                    }
                }
            }

            if (level > 1)
            {
                BuildFromBottom(level / 2, octrees);
            }
        }

        public void SetVisibility(BlockIndex position, BlockVisibility type)
        {
            if (Size == 1)
            {
                Type = type;
                return;
            }

            if (type != Type)
            {

                bool areChildrenLeaves = true;

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            if (IsLeaf)
                            {
                                Children[x, y, z] = new Octree(Size / 2, new BlockIndex(x, y, z) * Size / 2, this, Type);
                            }
                        }
                    }
                }
                IsLeaf = false;

                position -= Position;
                Octree target = Children[position.X < Size / 2 ? 0 : 1, position.Y < Size / 2 ? 0 : 1, position.Z < Size / 2 ? 0 : 1];
                target.SetVisibility(position, type);

                if (areChildrenLeaves)
                {
                    Simplify();
                }
            }
        }

        public FaceList GetFaces()
        {
            FaceList returnValue = new FaceList();

            if (!IsLeaf)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            returnValue.AddFaces(Children[x, y, z].GetFaces());
                        }
                    }
                }
            }
            else
            {
                BlockIndex currentBlock;

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        currentBlock = AbsolutePosition + new BlockIndex(0, i, j);

                        if (Constants.CurrentWorld.GetBlock(currentBlock - BlockIndex.UnitX).IsTranslucent())
                        {
                            returnValue.AddFace(currentBlock, Direction.Left, Constants.CurrentWorld.GetBlock(currentBlock).GetFace(Direction.Left), 1.0F);
                        }
                    }
                }

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        currentBlock = AbsolutePosition + new BlockIndex(Size - 1, i, j);

                        if (Constants.CurrentWorld.GetBlock(currentBlock + BlockIndex.UnitX).IsTranslucent())
                        {
                            returnValue.AddFace(currentBlock, Direction.Right, Constants.CurrentWorld.GetBlock(currentBlock).GetFace(Direction.Right), 1.0F);
                        }
                    }
                }

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        currentBlock = AbsolutePosition + new BlockIndex(i, 0, j);

                        if (Constants.CurrentWorld.GetBlock(currentBlock - BlockIndex.UnitY).IsTranslucent())
                        {
                            returnValue.AddFace(currentBlock, Direction.Down, Constants.CurrentWorld.GetBlock(currentBlock).GetFace(Direction.Down), 1.0F);
                        }
                    }
                }

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        currentBlock = AbsolutePosition + new BlockIndex(i, Size - 1, j);

                        if (Constants.CurrentWorld.GetBlock(currentBlock + BlockIndex.UnitY).IsTranslucent())
                        {
                            returnValue.AddFace(currentBlock, Direction.Up, Constants.CurrentWorld.GetBlock(currentBlock).GetFace(Direction.Up), 1.0F);
                        }
                    }
                }

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        currentBlock = AbsolutePosition + new BlockIndex(i, j, 0);

                        if (Constants.CurrentWorld.GetBlock(currentBlock - BlockIndex.UnitZ).IsTranslucent())
                        {
                            returnValue.AddFace(currentBlock, Direction.Backward, Constants.CurrentWorld.GetBlock(currentBlock).GetFace(Direction.Backward), 1.0F);
                        }
                    }
                }

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        currentBlock = AbsolutePosition + new BlockIndex(i, j, Size - 1);

                        if (Constants.CurrentWorld.GetBlock(currentBlock + BlockIndex.UnitZ).IsTranslucent())
                        {
                            returnValue.AddFace(currentBlock, Direction.Forward, Constants.CurrentWorld.GetBlock(currentBlock).GetFace(Direction.Forward), 1.0F);
                        }
                    }
                }
            }

            return returnValue;
        }
    }
}*/
