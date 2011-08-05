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
        List<VoxelVertex> Vertices;

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
            Vertices = new List<VoxelVertex>();
        }

        public void AddFace(BlockIndex index, Direction face, Block block, byte shade)
        {
            VoxelVertex[] tempVertices = new VoxelVertex[4];

            tempVertices[0] = GetVertex(index, face, 0, block, shade);
            tempVertices[1] = GetVertex(index, face, 1, block, shade);
            tempVertices[2] = GetVertex(index, face, 2, block, shade);
            tempVertices[3] = GetVertex(index, face, 3, block, shade);

            Vertices.AddRange(tempVertices);
        }

        public VoxelVertex GetVertex(BlockIndex index, Direction normal, byte corner, Block block, byte shade)
        {
            uint vertexData = 0;
            vertexData += (uint)Mathematics.Clamp(index.X, 0, 31);
            vertexData += (uint)Mathematics.Clamp(index.Y, 0, 31)                    * 32;
            vertexData += (uint)Mathematics.Clamp(index.Z, 0, 31)                    * 32 * 32;
            vertexData += (uint)Mathematics.Clamp(normal.Value, 0, 5)                * 32 * 32 * 32;
            vertexData += (uint)Mathematics.Clamp(corner, 0, 3)                      * 32 * 32 * 32 * 6;
            vertexData += (uint)Mathematics.Clamp(block.GetFace(normal), 0, 255)     * 32 * 32 * 32 * 6 * 4;
            vertexData += (uint)Mathematics.Clamp(shade, 0, 20)                      * 32 * 32 * 32 * 6 * 4 * 256;

            return new VoxelVertex(vertexData);
        }

        public void FillVertexBuffer(ref VertexBuffer value)
        {
            value.SetData(Vertices.ToArray());
        }

        static public FaceValidation GetFaceValidation(Block thisBlock, Block nextBlock)
        {
            BlockVisibility thisBlockVisibility = thisBlock.Visibility;
            BlockVisibility nextBlockVisibility = nextBlock.Visibility;

            // Special cases:

            if (nextBlock == Block.Vacuum)
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
                    if (thisBlock == nextBlock)
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
                    if (thisBlock == nextBlock)
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
