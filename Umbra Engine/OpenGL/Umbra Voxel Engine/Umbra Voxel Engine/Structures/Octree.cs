using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures
{
    class Octree
    {
        Octree[, ,] Children;
        Chunk ParentChunk;
        BlockIndex AbsoluteBlockIndex;
        public BlockIndex LocalPosition;
        public byte Size;
        BlockVisibility Visibility;

        public Octree(Block[, ,] data, Chunk parentChunk)
        {
            SetupOctree((byte)Constants.World.ChunkSize, data, BlockIndex.Zero, BlockIndex.Zero, parentChunk);
        }

        public Octree(byte size, Block[, ,] data, BlockIndex absoluteBlockIndex, BlockIndex localPosition, Chunk parentChunk)
        {
            SetupOctree(size, data, absoluteBlockIndex, localPosition, parentChunk);
        }

        void SetupOctree(byte size, Block[, ,] data, BlockIndex absoluteBlockIndex, BlockIndex localPosition, Chunk parentChunk)
        {
            AbsoluteBlockIndex = absoluteBlockIndex;
            Size = size;
            LocalPosition = localPosition;
            ParentChunk = parentChunk;

            if (size == 1)
            {
                Visibility = data[absoluteBlockIndex.X, absoluteBlockIndex.Y, absoluteBlockIndex.Z].Visibility;
                Children = null;
            }
            else
            {
                Children = new Octree[2, 2, 2];

                bool areChildrenEqual = true;

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            Children[x, y, z] = new Octree((byte)(size / 2), data, absoluteBlockIndex + new BlockIndex(x, y, z) * size / 2, new BlockIndex(x, y, z) * size / 2, ParentChunk);
                            if (!Children[x, y, z].Compare(Children[0, 0, 0]))
                            {
                                areChildrenEqual = false;
                            }
                        }
                    }
                }

                if (areChildrenEqual)
                {
                    Visibility = Children[0, 0, 0].Visibility;
                    Children = null;
                }
            }
        }

        public Octree(byte size, BlockIndex localPosition, Octree parent, BlockVisibility visibility, Chunk parentChunk)
        {
            Size = size;
            LocalPosition = localPosition;
            AbsoluteBlockIndex = localPosition + parent.AbsoluteBlockIndex;
            Visibility = visibility;
            ParentChunk = parentChunk;
            Children = null;
        }


        bool Compare(Octree oct)
        {
            if (oct.Visibility != Visibility || !(oct.Children == null && Children == null))
            {
                return false;
            }
            return true;
        }

        public BlockVisibility GetVisibility(BlockIndex position)
        {
            if (Children == null)
            {
                return Visibility;
            }
            else
            {
                position -= LocalPosition;
                Octree target = Children[position.X < Size / 2 ? 0 : 1, position.Y < Size / 2 ? 0 : 1, position.Z < Size / 2 ? 0 : 1];
                return target.GetVisibility(position);
            }
        }

        public void SetVisibility(BlockIndex position, BlockVisibility visibility)
        {
            if (Size == 1)
            {
                Visibility = visibility;
                return;
            }

            if (visibility != Visibility)
            {
                if (Children == null)
                {
                    Children = new Octree[2, 2, 2];
                }

                bool areChildrenLeaves = true;

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            if (Children == null || Children[x, y, z] == null)
                            {
                                Children[x, y, z] = new Octree((byte)(Size / 2), new BlockIndex(x, y, z) * Size / 2, this, Visibility, ParentChunk);
                            }
                        }
                    }
                }

                position -= LocalPosition;
                Octree target = Children[position.X < Size / 2 ? 0 : 1, position.Y < Size / 2 ? 0 : 1, position.Z < Size / 2 ? 0 : 1];
                target.SetVisibility(position, visibility);

                if (areChildrenLeaves)
                {
                    Simplify();
                }
            }
        }

        public void Simplify()
        {
            if (Children == null)
            {
                return;
            }

            if (Children == null)
            {
                Children = new Octree[2, 2, 2];
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
                Visibility = Children[0, 0, 0].Visibility;
                Children = null;
            }
        }

        public FaceList GetFaces()
        {
            FaceList returnValue = new FaceList();
            GetFaces(returnValue);
            return returnValue;
        }

        void GetFaces(FaceList faceList)
        {
            if (Children != null)
            {
                foreach (Octree child in Children)
                {
                    child.GetFaces(faceList);
                }
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        SetupFace(ref faceList, AbsoluteBlockIndex + new BlockIndex(Size - 1, i, j), Direction.Right);
                        SetupFace(ref faceList, AbsoluteBlockIndex + new BlockIndex(i, Size - 1, j), Direction.Up);
                        SetupFace(ref faceList, AbsoluteBlockIndex + new BlockIndex(i, j, Size - 1), Direction.Backward);
                    }
                }
            }
        }

        BlockIndex nextIndex;
        FaceValidation validation;
        private void SetupFace(ref FaceList faceList, BlockIndex currentIndex, Direction faceDirection)
        {
            nextIndex = new BlockIndex(currentIndex.Position + faceDirection.GetVector3());

            validation = FaceList.GetFaceValidation(Constants.World.Current.GetBlock(currentIndex + ParentChunk.Index), Constants.World.Current.GetBlock(nextIndex + ParentChunk.Index));

            if (validation == FaceValidation.ThisFace || validation == FaceValidation.BothFaces)
            {
                faceList.AddFace(currentIndex, faceDirection, ParentChunk[currentIndex], Block.GetFaceShade(currentIndex + ParentChunk.Index.ToBlockIndex(), faceDirection));
            }
            if (validation == FaceValidation.OtherFace || validation == FaceValidation.BothFaces)
            {
                faceList.AddFace(nextIndex, faceDirection.Opposite(), Constants.World.Current.GetBlock(nextIndex + ParentChunk.Index.ToBlockIndex()), Block.GetFaceShade(nextIndex + ParentChunk.Index.ToBlockIndex(), faceDirection.Opposite()));
            }
        }
    }
}
