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

        public void AddFace(BlockIndex position, Direction face, byte blockType, byte shade)
        {

            SmallBlockVertex[] tempVertices = new SmallBlockVertex[6];

            switch (face)
            {
                case Direction.Right:
                    {
                        tempVertices[0].FaceDirection_VertexCorner = 0 + 0 * 4;
                        tempVertices[1].FaceDirection_VertexCorner = 1 + 0 * 4;
                        tempVertices[2].FaceDirection_VertexCorner = 2 + 0 * 4;
                        tempVertices[3].FaceDirection_VertexCorner = 3 + 0 * 4;
                        break;
                    }
                case Direction.Left:
                    {
                        tempVertices[0].FaceDirection_VertexCorner = 0 + 1 * 4;
                        tempVertices[1].FaceDirection_VertexCorner = 1 + 1 * 4;
                        tempVertices[2].FaceDirection_VertexCorner = 2 + 1 * 4;
                        tempVertices[3].FaceDirection_VertexCorner = 3 + 1 * 4;
                        break;
                    }
                case Direction.Up:
                    {
                        tempVertices[0].FaceDirection_VertexCorner = 0 + 2 * 4;
                        tempVertices[1].FaceDirection_VertexCorner = 1 + 2 * 4;
                        tempVertices[2].FaceDirection_VertexCorner = 2 + 2 * 4;
                        tempVertices[3].FaceDirection_VertexCorner = 3 + 2 * 4;
                        break;
                    }
                case Direction.Down:
                    {
                        tempVertices[0].FaceDirection_VertexCorner = 0 + 3 * 4;
                        tempVertices[1].FaceDirection_VertexCorner = 1 + 3 * 4;
                        tempVertices[2].FaceDirection_VertexCorner = 2 + 3 * 4;
                        tempVertices[3].FaceDirection_VertexCorner = 3 + 3 * 4;
                        break;
                    }
                case Direction.Forward:
                    {
                        tempVertices[0].FaceDirection_VertexCorner = 0 + 4 * 4;
                        tempVertices[1].FaceDirection_VertexCorner = 1 + 4 * 4;
                        tempVertices[2].FaceDirection_VertexCorner = 2 + 4 * 4;
                        tempVertices[3].FaceDirection_VertexCorner = 3 + 4 * 4;
                        break;
                    }
                case Direction.Backward:
                    {
                        tempVertices[0].FaceDirection_VertexCorner = 0 + 5 * 4;
                        tempVertices[1].FaceDirection_VertexCorner = 1 + 5 * 4;
                        tempVertices[2].FaceDirection_VertexCorner = 2 + 5 * 4;
                        tempVertices[3].FaceDirection_VertexCorner = 3 + 5 * 4;
                        break;
                    }
            }

            tempVertices[0].Index = (short)(position.X + position.Y * 32 + position.Z * 1024);
            tempVertices[1].Index = (short)(position.X + position.Y * 32 + position.Z * 1024);
            tempVertices[2].Index = (short)(position.X + position.Y * 32 + position.Z * 1024);
            tempVertices[3].Index = (short)(position.X + position.Y * 32 + position.Z * 1024);

            tempVertices[0].BlockType = blockType;
            tempVertices[1].BlockType = blockType;
            tempVertices[2].BlockType = blockType;
            tempVertices[3].BlockType = blockType;

            tempVertices[4] = tempVertices[2];
            tempVertices[5] = tempVertices[1];

            Vertices.AddRange(tempVertices);
        }

        public VertexBuffer GetVertexBuffer()
        {
            VertexBuffer returnValue = new VertexBuffer(Constants.Engine_Graphics.GraphicsDevice, SmallBlockVertex.VertexDeclaration, Vertices.Count, BufferUsage.WriteOnly);
            returnValue.SetData(Vertices.ToArray());
            return returnValue;
        }

        static public FaceValidation GetFaceValidation(Block thisBlock, Block nextBlock)
        {
            BlockVisibility thisBlockVisibility = thisBlock.Visibility;
            BlockVisibility nextBlockVisibility = nextBlock.Visibility;

            // Special cases:

            if (thisBlock.Type == Block.Leaves.Type && nextBlock.Type == Block.Leaves.Type)
            {
                return FaceValidation.BothFaces;
            }


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
