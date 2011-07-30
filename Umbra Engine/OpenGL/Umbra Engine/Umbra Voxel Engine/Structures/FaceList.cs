using System;
using System.Linq;
using System.Text;
using System.Drawing;
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
    public class FaceList
    {
        List<SmallBlockVertex> Vertices;

        public bool IsEmpty
        {
            get { return Vertices.Count == 0; }
        }

        public FaceList()
        {
            Clear();
        }

        public void Clear()
        {
            Vertices = new List<SmallBlockVertex>();
        }

        public void AddFace(BlockIndex position, Direction face, byte texture, byte shade)
        {
            SmallBlockVertex[] tempVertices = new SmallBlockVertex[4];

            if (face == Direction.Right)
            {
                tempVertices[0].PositionX = 1;
                tempVertices[0].PositionY = 0;
                tempVertices[0].PositionZ = 0;

                tempVertices[1].PositionX = 1;
                tempVertices[1].PositionY = 1;
                tempVertices[1].PositionZ = 0;

                tempVertices[2].PositionX = 1;
                tempVertices[2].PositionY = 1;
                tempVertices[2].PositionZ = 1;

                tempVertices[3].PositionX = 1;
                tempVertices[3].PositionY = 0;
                tempVertices[3].PositionZ = 1;
            }
            else if (face == Direction.Left)
            {
                tempVertices[0].PositionX = 0;
                tempVertices[0].PositionY = 0;
                tempVertices[0].PositionZ = 1;

                tempVertices[1].PositionX = 0;
                tempVertices[1].PositionY = 1;
                tempVertices[1].PositionZ = 1;

                tempVertices[2].PositionX = 0;
                tempVertices[2].PositionY = 1;
                tempVertices[2].PositionZ = 0;

                tempVertices[3].PositionX = 0;
                tempVertices[3].PositionY = 0;
                tempVertices[3].PositionZ = 0;
            }
            else if (face == Direction.Up)
            {
                tempVertices[0].PositionX = 1;
                tempVertices[0].PositionY = 1;
                tempVertices[0].PositionZ = 1;

                tempVertices[1].PositionX = 1;
                tempVertices[1].PositionY = 1;
                tempVertices[1].PositionZ = 0;

                tempVertices[2].PositionX = 0;
                tempVertices[2].PositionY = 1;
                tempVertices[2].PositionZ = 0;

                tempVertices[3].PositionX = 0;
                tempVertices[3].PositionY = 1;
                tempVertices[3].PositionZ = 1;
            }
            else if (face == Direction.Down)
            {
                tempVertices[0].PositionX = 0;
                tempVertices[0].PositionY = 0;
                tempVertices[0].PositionZ = 1;

                tempVertices[1].PositionX = 0;
                tempVertices[1].PositionY = 0;
                tempVertices[1].PositionZ = 0;

                tempVertices[2].PositionX = 1;
                tempVertices[2].PositionY = 0;
                tempVertices[2].PositionZ = 0;

                tempVertices[3].PositionX = 1;
                tempVertices[3].PositionY = 0;
                tempVertices[3].PositionZ = 1;
            }
            else if (face == Direction.Forward)
            {
                tempVertices[0].PositionX = 0;
                tempVertices[0].PositionY = 0;
                tempVertices[0].PositionZ = 0;

                tempVertices[1].PositionX = 0;
                tempVertices[1].PositionY = 1;
                tempVertices[1].PositionZ = 0;

                tempVertices[2].PositionX = 1;
                tempVertices[2].PositionY = 1;
                tempVertices[2].PositionZ = 0;

                tempVertices[3].PositionX = 1;
                tempVertices[3].PositionY = 0;
                tempVertices[3].PositionZ = 0;
            }
            else if (face == Direction.Backward)
            {
                tempVertices[0].PositionX = 1;
                tempVertices[0].PositionY = 0;
                tempVertices[0].PositionZ = 1;

                tempVertices[1].PositionX = 1;
                tempVertices[1].PositionY = 1;
                tempVertices[1].PositionZ = 1;

                tempVertices[2].PositionX = 0;
                tempVertices[2].PositionY = 1;
                tempVertices[2].PositionZ = 1;

                tempVertices[3].PositionX = 0;
                tempVertices[3].PositionY = 0;
                tempVertices[3].PositionZ = 1;
            }

            tempVertices[0].PositionX += (byte)position.X;
            tempVertices[1].PositionX += (byte)position.X;
            tempVertices[2].PositionX += (byte)position.X;
            tempVertices[3].PositionX += (byte)position.X;

            tempVertices[0].PositionY += (byte)position.Y;
            tempVertices[1].PositionY += (byte)position.Y;
            tempVertices[2].PositionY += (byte)position.Y;
            tempVertices[3].PositionY += (byte)position.Y;

            tempVertices[0].PositionZ += (byte)position.Z;
            tempVertices[1].PositionZ += (byte)position.Z;
            tempVertices[2].PositionZ += (byte)position.Z;
            tempVertices[3].PositionZ += (byte)position.Z;

            tempVertices[0].TextureX = (byte)(GetTextureOffset(texture).X);
            tempVertices[0].TextureY = (byte)(1 + GetTextureOffset(texture).Y);

            tempVertices[1].TextureX = (byte)(GetTextureOffset(texture).X);
            tempVertices[1].TextureY = (byte)(GetTextureOffset(texture).Y);

            tempVertices[2].TextureX = (byte)(1 + GetTextureOffset(texture).X);
            tempVertices[2].TextureY = (byte)(GetTextureOffset(texture).Y);

            tempVertices[3].TextureX = (byte)(1 + GetTextureOffset(texture).X);
            tempVertices[3].TextureY = (byte)(1 + GetTextureOffset(texture).Y);

            tempVertices[0].ColorR = shade;
            tempVertices[0].ColorG = shade;
            tempVertices[0].ColorB = shade;
            tempVertices[1].ColorR = shade;
            tempVertices[1].ColorG = shade;
            tempVertices[1].ColorB = shade;
            tempVertices[2].ColorR = shade;
            tempVertices[2].ColorG = shade;
            tempVertices[2].ColorB = shade;
            tempVertices[3].ColorR = shade;
            tempVertices[3].ColorG = shade;
            tempVertices[3].ColorB = shade;

            //tempVertices[0].BlockData = Constants.World.Current.GetBlock(position).Type;
            //tempVertices[1].BlockData = tempVertices[0].BlockData;
            //tempVertices[2].BlockData = tempVertices[0].BlockData;
            //tempVertices[3].BlockData = tempVertices[0].BlockData;

            Vertices.AddRange(tempVertices);
        }

        Point GetTextureOffset(byte ID)
        {
            return new Point(ID % 16, ID >> 4);
        }

        public void FillVertexBuffer(ref VertexBuffer value)
        {
            value.SetData<SmallBlockVertex>(Vertices.ToArray());
        }

        static public FaceList GetCursorFacelist(BlockIndex cursorIndex)
        {
            FaceList faceList = new FaceList();

            faceList.AddFace(cursorIndex, Direction.Right, Block.Stone.GetFace(Direction.Right), 0);
            faceList.AddFace(cursorIndex, Direction.Left, Block.Stone.GetFace(Direction.Left), 0);
            faceList.AddFace(cursorIndex, Direction.Up, Block.Stone.GetFace(Direction.Up), 0);
            faceList.AddFace(cursorIndex, Direction.Down, Block.Stone.GetFace(Direction.Down), 0);
            faceList.AddFace(cursorIndex, Direction.Backward, Block.Stone.GetFace(Direction.Backward), 0);
            faceList.AddFace(cursorIndex, Direction.Forward, Block.Stone.GetFace(Direction.Forward), 0);

            return faceList;
        }

        static public FaceValidation GetFaceValidation(Block thisBlock, Block nextBlock)
        {
            BlockVisibility thisBlockVisibility = thisBlock.Visibility;
            BlockVisibility nextBlockVisibility = nextBlock.Visibility;

            // Special cases:

            if (nextBlock.Type == Block.Vacuum.Type)
            {
                return FaceValidation.NoFaces;
            }

            //if (thisBlock.Type == Block.Leaves.Type)
            //{
            //    if (nextBlock.Visibility == BlockVisibility.Invisible)
            //    {
            //        return FaceValidation.ThisFace;
            //    }
            //    else
            //    {
            //        return FaceValidation.BothFaces;
            //    }
            //}

            //if (nextBlock.Type == Block.Leaves.Type)
            //{
            //    if (thisBlock.Visibility == BlockVisibility.Invisible)
            //    {
            //        return FaceValidation.OtherFace;
            //    }
            //    else
            //    {
            //        return FaceValidation.BothFaces;
            //    }
            //}


            // Normal

            if (thisBlockVisibility == BlockVisibility.Invisible && nextBlockVisibility == BlockVisibility.Invisible)
            {
                return FaceValidation.NoFaces;
            }
            if (thisBlockVisibility == BlockVisibility.Opaque && nextBlockVisibility == BlockVisibility.Invisible)
            {
                return FaceValidation.ThisFace;
            }
            if (thisBlockVisibility == BlockVisibility.Invisible && nextBlockVisibility == BlockVisibility.Opaque)
            {
                return FaceValidation.OtherFace;
            }
            if (thisBlockVisibility == BlockVisibility.Translucent)
            {
                if (nextBlockVisibility == BlockVisibility.Invisible)
                {
                    return FaceValidation.ThisFace;
                }
                else if (nextBlockVisibility == BlockVisibility.Translucent)
                {
                    if (thisBlock.Type == nextBlock.Type)
                    {
                        return FaceValidation.NoFaces;
                    }
                    else
                    {
                        return FaceValidation.BothFaces;
                    }
                }
                else
                {
                    return FaceValidation.OtherFace;
                }
            }
            if (nextBlockVisibility == BlockVisibility.Translucent)
            {
                if (thisBlockVisibility == BlockVisibility.Invisible)
                {
                    return FaceValidation.OtherFace;
                }
                else if (thisBlockVisibility == BlockVisibility.Translucent)
                {
                    if (thisBlock.Type == nextBlock.Type)
                    {
                        return FaceValidation.NoFaces;
                    }
                    else
                    {
                        return FaceValidation.BothFaces;
                    }
                }
                else
                {
                    return FaceValidation.ThisFace;
                }
            }

            return FaceValidation.NoFaces;
        }
    }
}
