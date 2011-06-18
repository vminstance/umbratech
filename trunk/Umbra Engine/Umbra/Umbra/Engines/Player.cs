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

namespace Umbra.Engines
{
    public class Player : DrawableGameComponent
    {
        public Camera FirstPersonCamera { get; private set; }

        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 LastChunkUpdatePosition;
        public Vector3 LastGridUpdatePosition;

        float CurrentAlterDalay;

        public bool IsRunning
        {
            get { return Constants.Input.IsRunning(); }
        }

        public bool ShouldJump
        {
            get { return Constants.Input.ShouldJump(); }
        }

        public Player(Main main)
            : base(main)
        {
            FirstPersonCamera = new Camera(Constants.PlayerSpawn + Vector3.UnitY * Constants.PlayerEyeHeight);
            Position = Constants.PlayerSpawn;
            LastChunkUpdatePosition = Position;
            LastGridUpdatePosition = Position;
            CurrentAlterDalay = Constants.AlterDelay;
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
            if (Constants.GameIsActive)
            {
                if (Constants.NoclipEnabed)
                {
                    MoveNoclip();
                }
                else
                {
                    Physics.UpdatePlayer(this, Constants.Input.WalkingDirection());


                    if ((Position - LastGridUpdatePosition).LengthSquared() > Constants.UpdateGridsMoveLength && Constants.DynamicWorld)
                    {
                        LandscapeGenerator.OffsetLayerGrids(new WorldIndex(Position) - new WorldIndex(LastGridUpdatePosition));
                        LastGridUpdatePosition = Position;
                    }

                    if ((Position - LastChunkUpdatePosition).LengthSquared() > Constants.UpdateChunksMoveLength && Constants.DynamicWorld)
                    {
                        Constants.CurrentWorld.OffsetChunks(new ChunkIndex(Position) - new ChunkIndex(LastChunkUpdatePosition));
                        LastChunkUpdatePosition = Position;
                    }
                }

                UpdateCamera(gameTime);

                if (Constants.Input.MouseCurrentState.LeftButton == ButtonState.Pressed)
                {
                    if (CurrentAlterDalay >= Constants.AlterDelay)
                    {
                        BlockIndex target = BlockCursor.GetToDestroy();
                        if (target != null)
                        {
                            Constants.CurrentWorld.SetBlock(target, Block.Air);
                            CurrentAlterDalay = 0.0F;
                            Constants.Sound.PlayerBlockRemoval();
                        }
                    }
                }
                else if (Constants.Input.MouseCurrentState.RightButton == ButtonState.Pressed)
                {
                    if (CurrentAlterDalay >= Constants.AlterDelay)
                    {
                        BlockIndex target = BlockCursor.GetToCreate();
                        if (target != null && !new PlayerBox(Position).Intersects(target))
                        {
                            Constants.CurrentWorld.SetBlock(target, Constants.CurrentCursorBlock);
                            CurrentAlterDalay = 0.0F;
                            Constants.Sound.PlayerBlockAdd();
                        }
                    }
                }
                else
                {
                    CurrentAlterDalay = Constants.AlterDelay;
                }

                if (CurrentAlterDalay < Constants.AlterDelay)
                {
                    CurrentAlterDalay += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            base.Update(gameTime);
        }

        private void MoveNoclip()
        {
            Position += Vector3.Transform(Constants.Input.NoclipDirection(), Matrix.CreateRotationX(FirstPersonCamera.Pitch) * Matrix.CreateRotationY(FirstPersonCamera.Direction)) * Constants.NoclipSpeed;
        }

        private void UpdateCamera(GameTime gameTime)
        {
            FirstPersonCamera.Position = Position + Vector3.UnitY * Constants.PlayerEyeHeight + CameraBobbing(gameTime);
            FirstPersonCamera.Update();
        }

        private Vector3 CameraBobbing(GameTime gameTime)
        {
            if (Constants.CameraBobbingEnabled && Physics.PlayerIsOnGround())
            {

                float counter = (float)gameTime.TotalGameTime.TotalMilliseconds * Constants.CameraBobbingSpeed;
                float speed = new Vector2(Velocity.X, Velocity.Z).Length() * Constants.CameraBobbingMagnitude;

                if (Math.Abs(Math.Sin(MathHelper.ToRadians(counter + 90))) >= 0.997 && speed != 0)
                {
                    Constants.Sound.PlayerWalk();
                }

                return new Vector3((float)(Math.Cos(FirstPersonCamera.Direction) * Math.Sin(MathHelper.ToRadians(counter)) * speed), (float)(Math.Sin(FirstPersonCamera.Direction) * Math.Cos(MathHelper.ToRadians(counter) * 2) * speed), 0f);
            }
            else
            {
                return Vector3.Zero;
            }
        }
    }
}
