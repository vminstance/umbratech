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
using Console = Umbra.Implementations.Console;

namespace Umbra.Implementations
{
    static public class BlockCursor
    {
        static public VertexBuffer GetVertexBuffer()
        {

            BlockIndex currentAim = GetToDestroy();

            if(currentAim == null)
            {
                return null;
            }

            CursorVertex[] vertices = new CursorVertex[24];

            Vector3 currentAimPosition = currentAim.Position;
            Vector3 UnitX = Vector3.UnitX;
            Vector3 UnitY = Vector3.UnitY;
            Vector3 UnitZ = Vector3.UnitZ;
            Vector3 One = Vector3.One;

            vertices[0] = new CursorVertex(currentAimPosition, Color.Black);
            vertices[1] = new CursorVertex(UnitX + currentAimPosition, Color.Black);
            vertices[2] = new CursorVertex(currentAimPosition, Color.Black);
            vertices[3] = new CursorVertex(UnitY + currentAimPosition, Color.Black);
            vertices[4] = new CursorVertex(currentAimPosition, Color.Black);
            vertices[5] = new CursorVertex(UnitZ + currentAimPosition, Color.Black);

            vertices[6] = new CursorVertex(UnitY + currentAimPosition, Color.Black);
            vertices[7] = new CursorVertex(UnitZ + UnitY + currentAimPosition, Color.Black);
            vertices[8] = new CursorVertex(UnitY + currentAimPosition, Color.Black);
            vertices[9] = new CursorVertex(UnitX + UnitY + currentAimPosition, Color.Black);
            vertices[10] = new CursorVertex(UnitZ + currentAimPosition, Color.Black);
            vertices[11] = new CursorVertex(UnitZ + UnitY + currentAimPosition, Color.Black);
            vertices[12] = new CursorVertex(UnitX + currentAimPosition, Color.Black);
            vertices[13] = new CursorVertex(UnitX + UnitY + currentAimPosition, Color.Black);
            vertices[14] = new CursorVertex(UnitZ + currentAimPosition, Color.Black);
            vertices[15] = new CursorVertex(UnitX + UnitZ + currentAimPosition, Color.Black);
            vertices[16] = new CursorVertex(UnitX + currentAimPosition, Color.Black);
            vertices[17] = new CursorVertex(UnitX + UnitZ + currentAimPosition, Color.Black);

            vertices[18] = new CursorVertex(UnitZ + UnitY + currentAimPosition, Color.Black);
            vertices[19] = new CursorVertex(One + currentAimPosition, Color.Black);
            vertices[20] = new CursorVertex(UnitX + UnitY + currentAimPosition, Color.Black);
            vertices[21] = new CursorVertex(One + currentAimPosition, Color.Black);
            vertices[22] = new CursorVertex(UnitX + UnitZ + currentAimPosition, Color.Black);
            vertices[23] = new CursorVertex(One + currentAimPosition, Color.Black);

            VertexBuffer returnVal = new VertexBuffer(Constants.Graphics.GraphicsDevice, VertexPositionColor.VertexDeclaration, 24, BufferUsage.None);
            returnVal.SetData(vertices.ToArray());

            return returnVal;
        }

        static public BlockIndex GetToDestroy()
        {
            Vector3 outVar;
            return Cursor(Constants.PlayerReach, out outVar);
        }

        static public BlockIndex GetToCreate()
        {
            Vector3 intersection;
            BlockIndex targetBlock = Cursor(Constants.PlayerReach, out intersection);

            if (targetBlock == null)
            {
                return null;
            }

            Vector3 distanceFromCenter = intersection - (targetBlock.Position + Vector3.One / 2);
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

        static private BlockIndex Cursor(float maxReach, out Vector3 intersectionPoint)
        {
            Vector3 direction = Vector3.Transform(Vector3.Forward, Constants.Physics.Player.FirstPersonCamera.Rotation);
            Vector3 startPosition = Constants.Physics.Player.FirstPersonCamera.Position;
            Ray ray = new Ray(startPosition, direction);
            float distance = 0.0F;
            BlockIndex index;
            float? intersect;

            while(distance <= Constants.PlayerReach)
            {
                index = new BlockIndex(direction * distance + startPosition);

                intersect = index.GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && Constants.CurrentWorld.GetBlock(index).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index;
                }

                intersect = (index + BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.CurrentWorld.GetBlock(index + BlockIndex.UnitX).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitX;
                }
                intersect = (index - BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.CurrentWorld.GetBlock(index - BlockIndex.UnitX).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitX;
                }
                intersect = (index + BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.CurrentWorld.GetBlock(index + BlockIndex.UnitY).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitY;
                }
                intersect = (index - BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.CurrentWorld.GetBlock(index - BlockIndex.UnitY).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitY;
                }
                intersect = (index + BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.CurrentWorld.GetBlock(index + BlockIndex.UnitZ).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitZ;
                }
                intersect = (index - BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
                if (intersect != null && Constants.CurrentWorld.GetBlock(index - BlockIndex.UnitZ).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitZ;
                }

                distance += 1.0F;
            }

            intersectionPoint = Vector3.Zero;
            return null;
        }
    }
}
