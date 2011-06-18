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

namespace Umbra.Implementations
{
    static public class Physics
    {
        static public void UpdatePlayer(Player player, Vector3 rawMovement)
        {
            Vector3 lastPos = player.Position;

            if (PlayerIsOnGround())
            {
                // Add jumping
                if (Constants.Player.ShouldJump && player.Velocity.Y == 0.0F)
                {
                    player.Velocity.Y = Constants.JumpForce;
                }
            }
            else
            {
                // Add gravity
                player.Velocity += Vector3.Down * Constants.Gravity;
            }

            // Add player movement
            if (!PlayerIsOnGround())
            {
                Constants.CurrentValues = Constants.FlyingValues;
            }
            else
            {
                if (player.IsRunning)
                {
                    Constants.CurrentValues = Constants.RunningValues;
                }
                else
                {
                    Constants.CurrentValues = Constants.WalkingValues;
                }

                if (new Vector3(player.Velocity.X, 0, player.Velocity.Z).Length() > Constants.CurrentValues[2])
                {
                    player.Velocity = Vector3.Normalize(player.Velocity * new Vector3(1, 0, 1)) * Constants.CurrentValues[2] + (player.Velocity.Y * Vector3.UnitY);
                }
            }

            player.Velocity += Vector3.Transform(rawMovement, Matrix.CreateRotationY(Constants.Player.FirstPersonCamera.Direction)) * (Constants.CurrentValues[0] + Constants.CurrentValues[1]);



            // Add friction
            if (new Vector3(player.Velocity.X, 0, player.Velocity.Z).Length() > Constants.CurrentValues[3])
            {
                player.Velocity -= Vector3.Normalize(new Vector3(player.Velocity.X, 0, player.Velocity.Z)) * Constants.CurrentValues[1];
            }
            else
            {
                player.Velocity.X = 0;
                player.Velocity.Z = 0;
            }

            player.Velocity += Vector3.Up * Math.Sign(player.Velocity.Y) * (float)Math.Pow(player.Velocity.Y, 2) * Constants.AirFriction;


            // Check Dimensional Collision
            CalculateCollision(ref player.Velocity, ref player.Position);
        }

        static private void CalculateCollision(ref Vector3 velocity, ref Vector3 position)
        {
            if (velocity.Y != 0)
            {
                if (!PlayerPlaceFree(position + Vector3.UnitY * velocity.Y))
                {
                    MoveContact(Vector3.UnitY * Math.Sign(velocity.Y), ref position);
                    velocity.Y = 0;
                }
                else
                {
                    position.Y += velocity.Y;
                }
            }


            if (velocity.X != 0)
            {
                if (!PlayerPlaceFree(position + Vector3.UnitX * velocity.X))
                {
                    MoveContact(Vector3.UnitX * Math.Sign(velocity.X), ref position);

                    velocity.X = 0;
                }
                else
                {
                    position.X += velocity.X;
                }
            }



            if (velocity.Z != 0)
            {
                if (!PlayerPlaceFree(position + Vector3.UnitZ * velocity.Z))
                {
                    MoveContact(Vector3.UnitZ * Math.Sign(velocity.Z), ref position);

                    velocity.Z = 0;
                }
                else
                {
                    position.Z += velocity.Z;
                }
            }
        }

        static private void MoveContact(Vector3 direction, ref Vector3 position)
        {
            float stepSize = (float)Math.Min(Constants.Gravity, Constants.CurrentValues[0]);

            // Avoid a stepsize of zero, which would cause an infinite loop below.
            stepSize += stepSize == 0 ? 0.1F : 0;

            while (PlayerPlaceFree(position + direction * stepSize))
            {
                position += direction * stepSize;
            }
        }


        static public bool PlayerIsOnGround()
        {
            if (PlayerPlaceFree(Constants.Player.Position + Vector3.Down * Constants.PlayerMinDistanceToGround))
            {
                return false;
            }

            return true;
        }

        static private bool PlayerPlaceFree(Vector3 position)
        {
            for (int x = (int)Math.Floor(position.X - Constants.PlayerBoxWidth / 2); x <= Math.Floor(position.X + Constants.PlayerBoxWidth / 2); x++)
            {
                for (int y = (int)Math.Floor(position.Y); y <= Math.Floor(position.Y + Constants.PlayerBoxHeight); y++)
                {
                    for (int z = (int)Math.Floor(position.Z - Constants.PlayerBoxWidth / 2); z <= Math.Floor(position.Z + Constants.PlayerBoxWidth / 2); z++)
                    {
                        if (Block.IsSolid(Constants.CurrentWorld.GetBlock(new BlockIndex(x, y, z))))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
