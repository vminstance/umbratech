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

namespace Umbra.Structures
{
    public class Player : PhysicsObject
    {
        public Camera FirstPersonCamera { get; private set; }
        float CurrentAlterDelay;

        public Player() : base(Constants.PlayerSpawn, new Vector3(Constants.PlayerBoxWidth, Constants.PlayerBoxHeight, Constants.PlayerBoxWidth), Constants.PlayerMass, Constants.PlayerVolume)
        {
            FirstPersonCamera = new Camera(this.Position);
            CurrentAlterDelay = Constants.AlterDelay;
        }

        public Matrix GetViewMatrix()
        {
            return FirstPersonCamera.GetView();
        }

        public Matrix GetProjectionMatrix()
        {
            return FirstPersonCamera.GetProjection();
        }

        public override void Update(GameTime gameTime)
        {
            // Add player movement
            if (Constants.NoclipEnabed)
            {
                //Position += Vector3.Transform(Constants.Input.NoclipDirection(), Matrix.CreateRotationX(FirstPersonCamera.Direction) * Matrix.CreateRotationY(FirstPersonCamera.Pitch)) * Constants.NoclipSpeed;
                Position += Vector3.Transform(Constants.Input.NoclipDirection(), FirstPersonCamera.Rotation) * Constants.NoclipSpeed;
            }
            else
            {
                float oldSpeed = Velocity.Length();

                Velocity += Vector3.Transform(Constants.Input.WalkingDirection(), FirstPersonCamera.Rotation);
                Velocity.Normalize();
                Velocity *= Math.Min(oldSpeed * Constants.CurrentValues[0], Constants.CurrentValues[2]);
            }

            if (Constants.Input.MouseCurrentState.LeftButton == ButtonState.Pressed)
            {
                if (CurrentAlterDelay >= Constants.AlterDelay)
                {
                    BlockIndex target = BlockCursor.GetToDestroy();
                    if (target != null)
                    {
                        Constants.CurrentWorld.SetBlock(target, Block.Air);
                        CurrentAlterDelay = 0.0F;
                        Constants.Sound.PlayerBlockRemoval();
                    }
                }
            }
            else if (Constants.Input.MouseCurrentState.RightButton == ButtonState.Pressed)
            {
                if (CurrentAlterDelay >= Constants.AlterDelay)
                {
                    BlockIndex target = BlockCursor.GetToCreate();
                    if (target != null && !AABB.PlayerBoundingBox(Position).Intersects(target.GetBoundingBox()))
                    {
                        Constants.CurrentWorld.SetBlock(target, Constants.CurrentCursorBlock);
                        CurrentAlterDelay = 0.0F;
                        Constants.Sound.PlayerBlockAdd();
                    }
                }
            }
            else
            {
                CurrentAlterDelay = Constants.AlterDelay;
            }

            FirstPersonCamera.Position = (Position + (Dimensions / 2) * new Vector3(1, 0, 1)) + Vector3.UnitY * Constants.PlayerEyeHeight;

            UpdateCamera(gameTime);

            base.Update(gameTime);
        }


        private void UpdateCamera(GameTime gameTime)
        {
            FirstPersonCamera.Position = Position + Vector3.UnitY * Constants.PlayerEyeHeight;// +CameraBobbing(gameTime);
            FirstPersonCamera.Update();
        }
    }
}
