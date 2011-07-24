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
using Console = Umbra.Implementations.Console;

namespace Umbra.Implementations
{
    static public class BlockCursor
    {
        static public VertexBuffer GetVertexBuffer()
        {

            BlockIndex currentAim = GetToDestroy();

            if (currentAim == null)
            {
                return null;
            }

            //CursorVertex[] vertices = new CursorVertex[24];

            //Vector3d currentAimPosition = currentAim;
            //Vector3d UnitX = Vector3d.UnitX;
            //Vector3d UnitY = Vector3d.UnitY;
            //Vector3d UnitZ = Vector3d.UnitZ;
            //Vector3d One = Vector3d.One;

            //vertices[0] = new CursorVertex(currentAimPosition, Color.Black);
            //vertices[1] = new CursorVertex(UnitX + currentAimPosition, Color.Black);
            //vertices[2] = new CursorVertex(currentAimPosition, Color.Black);
            //vertices[3] = new CursorVertex(UnitY + currentAimPosition, Color.Black);
            //vertices[4] = new CursorVertex(currentAimPosition, Color.Black);
            //vertices[5] = new CursorVertex(UnitZ + currentAimPosition, Color.Black);

            //vertices[6] = new CursorVertex(UnitY + currentAimPosition, Color.Black);
            //vertices[7] = new CursorVertex(UnitZ + UnitY + currentAimPosition, Color.Black);
            //vertices[8] = new CursorVertex(UnitY + currentAimPosition, Color.Black);
            //vertices[9] = new CursorVertex(UnitX + UnitY + currentAimPosition, Color.Black);
            //vertices[10] = new CursorVertex(UnitZ + currentAimPosition, Color.Black);
            //vertices[11] = new CursorVertex(UnitZ + UnitY + currentAimPosition, Color.Black);
            //vertices[12] = new CursorVertex(UnitX + currentAimPosition, Color.Black);
            //vertices[13] = new CursorVertex(UnitX + UnitY + currentAimPosition, Color.Black);
            //vertices[14] = new CursorVertex(UnitZ + currentAimPosition, Color.Black);
            //vertices[15] = new CursorVertex(UnitX + UnitZ + currentAimPosition, Color.Black);
            //vertices[16] = new CursorVertex(UnitX + currentAimPosition, Color.Black);
            //vertices[17] = new CursorVertex(UnitX + UnitZ + currentAimPosition, Color.Black);

            //vertices[18] = new CursorVertex(UnitZ + UnitY + currentAimPosition, Color.Black);
            //vertices[19] = new CursorVertex(One + currentAimPosition, Color.Black);
            //vertices[20] = new CursorVertex(UnitX + UnitY + currentAimPosition, Color.Black);
            //vertices[21] = new CursorVertex(One + currentAimPosition, Color.Black);
            //vertices[22] = new CursorVertex(UnitX + UnitZ + currentAimPosition, Color.Black);
            //vertices[23] = new CursorVertex(One + currentAimPosition, Color.Black);

            VertexBuffer returnVal = new VertexBuffer();
            //returnVal.SetData<CursorVertex>(vertices.ToArray(), CursorVertex.Size);

            return returnVal;
        }

        static public BlockIndex GetToDestroy()
        {
            Vector3d outVar;
            return Cursor(Constants.Player.BlockEditing.Reach, out outVar);
        }

        static public BlockIndex GetToCreate()
        {
            Vector3d intersection;
            BlockIndex targetBlock = Cursor(Constants.Player.BlockEditing.Reach, out intersection);

            if (targetBlock == null)
            {
                return null;
            }

            Vector3d distanceFromCenter = intersection - (targetBlock.Position + Vector3d.One / 2.0);
            BlockIndex targetIndex = BlockIndex.UnitY;

            if (Math.Abs(distanceFromCenter.Y) > Math.Max(Math.Abs(distanceFromCenter.X), Math.Abs(distanceFromCenter.Z)))
            {
                targetIndex = BlockIndex.UnitY * Math.Sign(distanceFromCenter.Y);
            }
            else if (Math.Abs(distanceFromCenter.X) > Math.Max(Math.Abs(distanceFromCenter.Y), Math.Abs(distanceFromCenter.Z)))
            {
                targetIndex = BlockIndex.UnitX * Math.Sign(distanceFromCenter.X);
            }
            else if (Math.Abs(distanceFromCenter.Z) > Math.Max(Math.Abs(distanceFromCenter.X), Math.Abs(distanceFromCenter.Y)))
            {
                targetIndex = BlockIndex.UnitZ * Math.Sign(distanceFromCenter.Z);
            }

            return targetBlock + targetIndex;
        }

        static private BlockIndex Cursor(double maxReach, out Vector3d intersectionPoint)
        {
            Vector3d direction = Vector3d.Transform(-Vector3d.UnitZ, Constants.Engine_Physics.Player.FirstPersonCamera.Rotation);
            Vector3d startPosition = Constants.Engine_Physics.Player.FirstPersonCamera.Position;
            Ray ray = new Ray(startPosition, direction);
            double distance = 0.0;
            BlockIndex index;
            double? intersect;

            while (distance <= Constants.Player.BlockEditing.Reach)
            {
                index = new BlockIndex(direction * distance + startPosition);

                intersect = index.GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && Constants.World.Current.GetBlock(index).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index;
                }

                intersect = (index + BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.World.Current.GetBlock(index + BlockIndex.UnitX).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitX;
                }
                intersect = (index - BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.World.Current.GetBlock(index - BlockIndex.UnitX).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitX;
                }
                intersect = (index + BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.World.Current.GetBlock(index + BlockIndex.UnitY).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitY;
                }
                intersect = (index - BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.World.Current.GetBlock(index - BlockIndex.UnitY).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitY;
                }
                intersect = (index + BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.World.Current.GetBlock(index + BlockIndex.UnitZ).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitZ;
                }
                intersect = (index - BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.World.Current.GetBlock(index - BlockIndex.UnitZ).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitZ;
                }

                distance += 1.0F;
            }

            intersectionPoint = Vector3d.Zero;
            return null;
        }
    }
}
