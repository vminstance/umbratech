﻿using System;
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
    public class Player : PhysicsObject
    {
        public Camera FirstPersonCamera { get; private set; }
        float CurrentAlterDelay;
        public bool IsReleased;

        public Player()
            : base(Constants.Player.Spawn, new Vector3(Constants.Player.Physics.Box.Width, Constants.Player.Physics.Box.Height, Constants.Player.Physics.Box.Width), Constants.Player.Physics.Mass)
        {
            FirstPersonCamera = new Camera(this.Position);
            CurrentAlterDelay = Constants.Controls.AlterDelay;
            IsReleased = false;
        }

        public Matrix GetViewMatrix()
        {
            return FirstPersonCamera.GetView();
        }

        public Matrix GetProjectionMatrix()
        {
            return FirstPersonCamera.GetProjection();
        }

        public void Initialize()
        {
            Position.Y = (float)Math.Ceiling(Math.Max(LandscapeGenerator.GetLandscapeHeight(new Vector2(Position.X, Position.Z)), 0.0F));
            IsReleased = false;
        }

        public void Release()
        {

            while (Constants.World.Current.GetBlock(new BlockIndex(Position + Vector3.UnitY)).Type != Block.Air.Type || Constants.World.Current.GetBlock(new BlockIndex(Position + Vector3.UnitY * 2)).Type != Block.Air.Type)
            {
                Position.Y++;
            }

            IsReleased = true;
            Variables.Player.NoclipEnabled = false;
        }

        public override void Update(GameTime gameTime)
        {

            PhysicsEnabled = !Variables.Player.NoclipEnabled;

            if (!Variables.Player.NoclipEnabled && Constants.World.DynamicWorld)
            {
                Vector3 currentWorldCenter = Constants.World.Current.Offset.Position + Vector3.One * ((float)Constants.World.WorldSize / 2.0F) * (float)Constants.World.ChunkSize;

                if (new ChunkIndex(Position) != new ChunkIndex(currentWorldCenter))
                {
                    if ((Position - currentWorldCenter).Length() > Constants.World.UpdateLengthFromCenter)
                    {
                        Constants.World.Current.OffsetChunks(new ChunkIndex(Position - currentWorldCenter));
                    }
                }
            }

            if (!Console.IsActive)
            {
                UpdateMovement(gameTime);
                UpdateBlockCursor(gameTime);
            }
            UpdateCamera(gameTime);


            base.Update(gameTime);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            // Add player movement
            if (Variables.Player.NoclipEnabled)
            {
                // Noclip

                Position += Vector3.Transform(Constants.Engine_Input.NoclipDirection(), FirstPersonCamera.Rotation) * Constants.Player.Movement.NoclipSpeed;
            }
            else
            {

                Vector3 horizontalVelocity = Velocity * new Vector3(1, 0, 1);

                if (Constants.Engine_Physics.IsOnGround(this))
                {
                    // Walking on ground

                    Vector3 newVelocity = horizontalVelocity + Vector3.Transform(Constants.Engine_Input.WalkingDirection(), Matrix.CreateRotationY(FirstPersonCamera.Direction)) * (Constants.Player.Movement.WalkForce / Mass * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (newVelocity != Vector3.Zero)
                    {
                        newVelocity = Vector3.Normalize(newVelocity) * Math.Min(newVelocity.Length(), Constants.Player.Movement.MaxSpeed);
                    }

                    ApplyForce((newVelocity - horizontalVelocity) * Mass / (float)gameTime.ElapsedGameTime.TotalSeconds * GripCoefficient * Constants.Player.Movement.GripSignificance);

                    if (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.Space) && Math.Round(Velocity.Y, 2) == 0)
                    {
                        ApplyForce(Vector3.Up * Constants.Player.Movement.JumpForce);
                    }
                }
                else
                {
                    // Swimming or in air

                    Vector3 newVelocity = horizontalVelocity + Vector3.Transform(Constants.Engine_Input.WalkingDirection(), Matrix.CreateRotationY(FirstPersonCamera.Direction)) * (Constants.Player.Movement.SwimForce / Mass * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (newVelocity != Vector3.Zero)
                    {
                        newVelocity = Vector3.Normalize(newVelocity) * Math.Min(newVelocity.Length(), Constants.Player.Movement.MaxSpeed);
                    }

                    ApplyForce((newVelocity - horizontalVelocity) * Mass / (float)gameTime.ElapsedGameTime.TotalSeconds * GripCoefficient * Constants.Player.Movement.GripSignificance);

                    if (Constants.Engine_Input.KeyboardCurrentState.IsKeyDown(Keys.Space))
                    {
                        float magnitude = MathHelper.Lerp(Constants.World.Current.GetBlock(new BlockIndex(Position)).Viscosity, Constants.World.Current.GetBlock(new BlockIndex(Position + Vector3.UnitY)).Viscosity, Position.Y % 1) / 5000.0F;

                        ApplyForce(Vector3.Up * Constants.Player.Movement.SwimForce * magnitude);
                    }
                }
            }
        }

        private void UpdateBlockCursor(GameTime gameTime)
        {

            if (Constants.Engine_Input.MouseCurrentState.LeftButton == ButtonState.Pressed)
            {
                if (CurrentAlterDelay >= Constants.Controls.AlterDelay)
                {
                    BlockIndex target = BlockCursor.GetToDestroy();
                    if (target != null)
                    {
                        Constants.World.Current.SetBlock(target, Block.Air);
                        CurrentAlterDelay = 0.0F;
                        Constants.Engine_Sound.PlayerBlockRemoval();
                    }
                }
            }
            else if (Constants.Engine_Input.MouseCurrentState.RightButton == ButtonState.Pressed)
            {
                if (CurrentAlterDelay >= Constants.Controls.AlterDelay)
                {
                    BlockIndex target = BlockCursor.GetToCreate();
                    if (target != null && !AABB.PlayerBoundingBox(Position).Intersects(target.GetBoundingBox()))
                    {
                        Constants.World.Current.SetBlock(target, Variables.Player.BlockEditing.CurrentCursorBlock);
                        CurrentAlterDelay = 0.0F;
                        Constants.Engine_Sound.PlayerBlockAdd();
                    }
                }
            }
            else
            {
                CurrentAlterDelay = Constants.Controls.AlterDelay;
            }
        }


        private void UpdateCamera(GameTime gameTime)
        {
            FirstPersonCamera.Position = (Position + (Dimensions / 2) * new Vector3(1, 0, 1)) + Vector3.UnitY * Constants.Player.Physics.EyeHeight;

            FirstPersonCamera.Update();
        }
    }
}
