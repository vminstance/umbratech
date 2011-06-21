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
    static public class Physics
    {
        static public void UpdatePlayer(Player player, Vector3 rawMovement)
        {
            Vector3 lastPos = player.Position;

            if (PlayerIsOnGround())
            {
                // Add jumping
                if (player.ShouldJump && player.Velocity.Y == 0.0F)
                {
                    player.Velocity.Y = Constants.JumpForce;
                }

                // Set ground values
                if (player.IsRunning)
                {
                    Constants.CurrentValues = Constants.RunningValues;
                }
                else
                {
                    Constants.CurrentValues = Constants.WalkingValues;
                }
            }
            else
            {
                //if (player.ShouldJump && player.MaxViscosity > 1)
                //{
                //    player.Velocity.Y = Math.Min(player.Velocity.Y + 0.1F, 0.4F);
                //}

                // Add gravity
                player.Velocity += Vector3.Down * Constants.Gravity;

                // Add air friction
                player.Velocity -= Vector3.UnitY * Math.Sign(player.Velocity.Y) * (float)Math.Pow(player.Velocity.Y, 2) * Constants.AirFriction;

                // Set flying values
                Constants.CurrentValues = Constants.FlyingValues;
            }

            // Add buoyency
            player.Velocity += Vector3.Up * player.Buoyancy;

            // Add horizontal movement
            float currentSpeed = (player.Velocity * new Vector3(1,0,1)).Length();

            player.Velocity += Vector3.Transform(rawMovement, Matrix.CreateRotationY(Constants.Player.FirstPersonCamera.Direction)) * (Constants.CurrentValues[0] + Constants.CurrentValues[1]);

            if (new Vector3(player.Velocity.X, 0, player.Velocity.Z).Length() > Constants.CurrentValues[2])
            {
                player.Velocity = Vector3.Normalize(player.Velocity * new Vector3(1, 0, 1)) * currentSpeed + Vector3.UnitY * player.Velocity.Y;
            }

            // Add friction
            if (new Vector3(player.Velocity.X, 0, player.Velocity.Z).Length() > Constants.CurrentValues[3] && rawMovement != Vector3.Zero)
            {
                player.Velocity -= Vector3.Normalize(new Vector3(player.Velocity.X, 0, player.Velocity.Z)) * Constants.CurrentValues[1];
            }
            else
            {
                player.Velocity.X = 0;
                player.Velocity.Z = 0;
            }

            // Check Collision
            MovePlayer(ref player.Velocity, ref player.Position);
        }

        static private void MovePlayer(ref Vector3 velocity, ref Vector3 position)
        {
            if (velocity.Y != 0)
            {
                if (!PlaceFree(position + Vector3.UnitY * velocity.Y))
                {
                    MoveContact(Vector3.UnitY * Math.Sign(velocity.Y), ref position);
                    velocity.Y = 0;
                }
                else
                {
                    position.Y += velocity.Y;
                }
            }

            if (velocity.X >= velocity.Z)
            {
                MovePlayerX(ref velocity, ref position);
            }

            if (velocity.Z != 0)
            {
                if (!PlaceFree(position + Vector3.UnitZ * velocity.Z))
                {
                    MoveContact(Vector3.UnitZ * Math.Sign(velocity.Z), ref position);

                    velocity.Z = 0;
                }
                else
                {
                    position.Z += velocity.Z;
                }
            }

            if (velocity.X < velocity.Z)
            {
                MovePlayerX(ref velocity, ref position);
            }
        }

        static private void MovePlayerX(ref Vector3 velocity, ref Vector3 position)
        {
            if (velocity.X != 0)
            {
                if (!PlaceFree(position + Vector3.UnitX * velocity.X))
                {
                    MoveContact(Vector3.UnitX * Math.Sign(velocity.X), ref position);

                    velocity.X = 0;
                }
                else
                {
                    position.X += velocity.X;
                }
            }
        }

        static private void MoveContact(Vector3 direction, ref Vector3 position)
        {
            float stepSize = (float)Math.Min(Constants.Gravity, Constants.CurrentValues[0]);

            // Avoid a stepsize of zero, which would cause an infinite loop below.
            stepSize += stepSize == 0 ? 0.1F : 0;

            while (PlaceFree(position + direction * stepSize))
            {
                position += direction * stepSize;
            }
        }


        static public bool PlayerIsOnGround()
        {
            if (PlaceFree(Constants.Player.Position + Vector3.Down * Constants.PlayerMinDistanceToGround))
            {
                return false;
            }

            return true;
        }

        static public bool PlaceFree(Vector3 position)
        {
            for (int x = (int)Math.Floor(position.X - Constants.PlayerBoxWidth / 2); x <= Math.Floor(position.X + Constants.PlayerBoxWidth / 2); x++)
            {
                for (int y = (int)Math.Floor(position.Y); y <= Math.Floor(position.Y + Constants.PlayerBoxHeight); y++)
                {
                    for (int z = (int)Math.Floor(position.Z - Constants.PlayerBoxWidth / 2); z <= Math.Floor(position.Z + Constants.PlayerBoxWidth / 2); z++)
                    {
                        if (Constants.CurrentWorld.GetBlock(new BlockIndex(x, y, z)).Solidity)
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
