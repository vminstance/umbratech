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

        public void AddFace(BlockIndex position, Direction face, uint texture, byte shade)
        {
            SmallBlockVertex[] tempVertices = new SmallBlockVertex[6];

            switch (face)
            {
                case Direction.Right:
                    {
                        tempVertices[0].PositionX = 1;
                        tempVertices[0].PositionY = 1;
                        tempVertices[0].PositionZ = 0;

                        tempVertices[1].PositionX = 1;
                        tempVertices[1].PositionY = 1;
                        tempVertices[1].PositionZ = 1;

                        tempVertices[2].PositionX = 1;
                        tempVertices[2].PositionY = 0;
                        tempVertices[2].PositionZ = 0;

                        tempVertices[3].PositionX = 1;
                        tempVertices[3].PositionY = 0;
                        tempVertices[3].PositionZ = 1;
                        break;
                    }
                case Direction.Left:
                    {
                        tempVertices[0].PositionX = 0;
                        tempVertices[0].PositionY = 1;
                        tempVertices[0].PositionZ = 1;

                        tempVertices[1].PositionX = 0;
                        tempVertices[1].PositionY = 1;
                        tempVertices[1].PositionZ = 0;

                        tempVertices[2].PositionX = 0;
                        tempVertices[2].PositionY = 0;
                        tempVertices[2].PositionZ = 1;

                        tempVertices[3].PositionX = 0;
                        tempVertices[3].PositionY = 0;
                        tempVertices[3].PositionZ = 0;
                        break;
                    }
                case Direction.Up:
                    {
                        tempVertices[0].PositionX = 1;
                        tempVertices[0].PositionY = 1;
                        tempVertices[0].PositionZ = 0;

                        tempVertices[1].PositionX = 0;
                        tempVertices[1].PositionY = 1;
                        tempVertices[1].PositionZ = 0;

                        tempVertices[2].PositionX = 1;
                        tempVertices[2].PositionY = 1;
                        tempVertices[2].PositionZ = 1;

                        tempVertices[3].PositionX = 0;
                        tempVertices[3].PositionY = 1;
                        tempVertices[3].PositionZ = 1;
                        break;
                    }
                case Direction.Down:
                    {
                        tempVertices[0].PositionX = 0;
                        tempVertices[0].PositionY = 0;
                        tempVertices[0].PositionZ = 0;

                        tempVertices[1].PositionX = 1;
                        tempVertices[1].PositionY = 0;
                        tempVertices[1].PositionZ = 0;

                        tempVertices[2].PositionX = 0;
                        tempVertices[2].PositionY = 0;
                        tempVertices[2].PositionZ = 1;

                        tempVertices[3].PositionX = 1;
                        tempVertices[3].PositionY = 0;
                        tempVertices[3].PositionZ = 1;
                        break;
                    }
                case Direction.Forward:
                    {
                        tempVertices[0].PositionX = 0;
                        tempVertices[0].PositionY = 1;
                        tempVertices[0].PositionZ = 0;

                        tempVertices[1].PositionX = 1;
                        tempVertices[1].PositionY = 1;
                        tempVertices[1].PositionZ = 0;

                        tempVertices[2].PositionX = 0;
                        tempVertices[2].PositionY = 0;
                        tempVertices[2].PositionZ = 0;

                        tempVertices[3].PositionX = 1;
                        tempVertices[3].PositionY = 0;
                        tempVertices[3].PositionZ = 0;
                        break;
                    }
                case Direction.Backward:
                    {
                        tempVertices[0].PositionX = 1;
                        tempVertices[0].PositionY = 1;
                        tempVertices[0].PositionZ = 1;

                        tempVertices[1].PositionX = 0;
                        tempVertices[1].PositionY = 1;
                        tempVertices[1].PositionZ = 1;

                        tempVertices[2].PositionX = 1;
                        tempVertices[2].PositionY = 0;
                        tempVertices[2].PositionZ = 1;

                        tempVertices[3].PositionX = 0;
                        tempVertices[3].PositionY = 0;
                        tempVertices[3].PositionZ = 1;
                        break;
                    }
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

            float inverted = 1.0F / (float)Constants.Content.Textures.TerrainTextureIndexWidthHeigh;

            tempVertices[0].TextureX = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (0 + GetTextureOffset(texture).X));
            tempVertices[0].TextureY = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (0 + GetTextureOffset(texture).Y));
            tempVertices[1].TextureX = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (inverted + GetTextureOffset(texture).X));
            tempVertices[1].TextureY = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (0 + GetTextureOffset(texture).Y));
            tempVertices[2].TextureX = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (0 + GetTextureOffset(texture).X));
            tempVertices[2].TextureY = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (inverted + GetTextureOffset(texture).Y));
            tempVertices[3].TextureX = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (inverted + GetTextureOffset(texture).X));
            tempVertices[3].TextureY = (byte)(Constants.Content.Textures.TerrainTextureIndexWidthHeigh * (inverted + GetTextureOffset(texture).Y));

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

            tempVertices[4] = tempVertices[2];
            tempVertices[5] = tempVertices[1];

            Vertices.AddRange(tempVertices);
        }

        Vector2 GetTextureOffset(uint ID)
        {
            return new Vector2(((float)ID % (float)Constants.Content.Textures.TerrainTextureIndexWidthHeigh) / (float)Constants.Content.Textures.TerrainTextureIndexWidthHeigh, (float)Math.Floor((float)ID / (float)Constants.Content.Textures.TerrainTextureIndexWidthHeigh) / (float)Constants.Content.Textures.TerrainTextureIndexWidthHeigh);
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
