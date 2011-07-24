using System;
using System.Linq;
using System.Text;
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

namespace Umbra.Structures
{
    public class Player : PhysicsObject
    {
        public Camera FirstPersonCamera { get; private set; }
        float CurrentAlterDelay;
        public bool IsReleased;
        Vector3 MoveDirection;
        bool SpacePressed;
        bool LeftShiftPressed;

        public Player()
            : base(Constants.Player.Spawn, new Vector3(Constants.Player.Physics.Box.Width, Constants.Player.Physics.Box.Height, Constants.Player.Physics.Box.Width), Constants.Player.Physics.Mass)
        {
            FirstPersonCamera = new Camera(this.Position);
            CurrentAlterDelay = Constants.Controls.AlterDelay;
            IsReleased = false;
            MoveDirection = Vector3.Zero;
            SpacePressed = false;
            LeftShiftPressed = false;
        }

        public Matrix4 ViewMatrix
        {
            get
            {
                return FirstPersonCamera.GetView();
            }
        }

        public Matrix4 ProjectionMatrix
        {
            get
            {
                return FirstPersonCamera.GetProjection();
            }
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
            Position.Y += 1.0F;

            IsReleased = true;
            Variables.Player.NoclipEnabled = false;
        }

        public override void Update(FrameEventArgs e)
        {

            PhysicsEnabled = !Variables.Player.NoclipEnabled;

            if (!Variables.Player.NoclipEnabled && Constants.World.DynamicWorld)
            {
                Vector3 currentWorldCenter = Constants.World.Current.Offset.Position + Vector3.One * ((float)Constants.World.WorldSize / 2.0F) * (float)Constants.World.ChunkSize;

                if (new ChunkIndex(Position) != new ChunkIndex(currentWorldCenter))
                {
                    if ((Position - currentWorldCenter).Length > Constants.World.UpdateLengthFromCenter)
                    {
                        Constants.World.Current.OffsetChunks(new ChunkIndex(Position - currentWorldCenter));
                    }
                }
            }

            UpdateMovement(e);
            UpdateCamera(e);


            base.Update(e);
        }

        public void UpdateKeyboard(KeyboardDevice keyboard)
        {
            if (Variables.Player.NoclipEnabled)
            {
                MoveDirection = NoclipDirection(keyboard);
            }
            else
            {
                MoveDirection = WalkingDirection(keyboard);
                SpacePressed = keyboard[Key.Space];
                LeftShiftPressed = keyboard[Key.ShiftLeft];
            }
        }


        public void UpdateMouse(MouseDevice mouse)
        {
            if (!Constants.Controls.CanPlaceBlocks)
            {
                return;
            }

            if (mouse[MouseButton.Left])
            {
                if (CurrentAlterDelay == 0)
                {
                    BlockIndex target = BlockCursor.GetToDestroy();
                    if (target != null)
                    {
                        Constants.World.Current.SetBlock(target, Block.Air);
                        CurrentAlterDelay = Constants.Controls.AlterDelay;
                    }
                }
            }
            else if (mouse[MouseButton.Right])
            {
                if (CurrentAlterDelay == 0)
                {
                    BlockIndex target = BlockCursor.GetToCreate();
                    if (target != null && !BoundingBox.PlayerBoundingBox(Position).Intersects(target.GetBoundingBox()))
                    {
                        Constants.World.Current.SetBlock(target, Variables.Player.BlockEditing.CurrentCursorBlock);
                        CurrentAlterDelay = Constants.Controls.AlterDelay;
                    }
                }
            }
            else
            {
                CurrentAlterDelay = 0;
            }

            if (CurrentAlterDelay > 0)
            {
                CurrentAlterDelay--;
            }
        }

        public void UpdateMovement(FrameEventArgs e)
        {

            // Add player movement
            if (Variables.Player.NoclipEnabled)
            {
                // Noclip
                Position += Vector3.Transform(MoveDirection, FirstPersonCamera.Rotation) * Constants.Player.Movement.NoclipSpeed;
            }
            else
            {

                Vector3 horizontalVelocity = Vector3.Multiply(Velocity, new Vector3(1, 0, 1));

                if (Constants.Engine_Physics.IsOnGround(this))
                {
                    // Walking on ground

                    Vector3 newVelocity = horizontalVelocity + Vector3.Transform(MoveDirection, Matrix4.CreateRotationY(FirstPersonCamera.Direction)) * (Constants.Player.Movement.WalkForce / Mass * (float)e.Time);

                    if (newVelocity != Vector3.Zero)
                    {
                        newVelocity = Vector3.Normalize(newVelocity) * Math.Min(newVelocity.Length, Constants.Player.Movement.MaxSpeed);
                    }

                    ApplyForce((newVelocity - horizontalVelocity) * Mass / (float)e.Time * GripCoefficient * Constants.Player.Movement.GripSignificance);

                    if (SpacePressed && Velocity.Y == 0)
                    {
                        ApplyForce(Vector3.UnitY * Constants.Player.Movement.JumpForce);
                    }
                }
                else
                {
                    // Swimming or in air
                    Vector3 newVelocity;

                    if (LeftShiftPressed)
                    {
                        newVelocity = horizontalVelocity + Vector3.Transform(MoveDirection, FirstPersonCamera.Rotation) * (Constants.Player.Movement.SwimForce / Mass * (float)e.Time);
                    }
                    else
                    {
                        newVelocity = horizontalVelocity + Vector3.Transform(MoveDirection, Matrix4.CreateRotationY(FirstPersonCamera.Direction)) * (Constants.Player.Movement.SwimForce / Mass * (float)e.Time);
                    }

                    if (newVelocity != Vector3.Zero)
                    {
                        newVelocity = Vector3.Normalize(newVelocity) * Math.Min(newVelocity.Length, Constants.Player.Movement.MaxSpeed);
                    }

                    ApplyForce((newVelocity - horizontalVelocity) * Mass / (float)e.Time * GripCoefficient * Constants.Player.Movement.GripSignificance);

                    if (SpacePressed)
                    {
                        float magnitude = Interpolation.Linear(Constants.World.Current.GetBlock(new BlockIndex(Position)).Viscosity, Constants.World.Current.GetBlock(new BlockIndex(Position + Vector3.UnitY)).Viscosity, Position.Y % 1) / 5000.0F;

                        ApplyForce(Vector3.UnitY * Constants.Player.Movement.SwimForce * magnitude);
                    }
                }
            }

            MoveDirection = Vector3.Zero;
        }


        private void UpdateCamera(FrameEventArgs e)
        {
            FirstPersonCamera.Position = Position + Vector3.Multiply(Dimensions / 2.0F, new Vector3(1, 0, 1)) + Vector3.UnitY * Constants.Player.Physics.EyeHeight;
        }

        public Vector3 NoclipDirection(KeyboardDevice keyboard)
        {
            Vector3 returnVector = Vector3.Zero;

            if (Variables.Game.IsActive)
            {
                returnVector = new Vector3((keyboard[Key.D] ? 1 : 0) - (keyboard[Key.A] ? 1 : 0),
                    (keyboard[Key.Space] ? 1 : 0) - (keyboard[Key.ShiftLeft] ? 1 : 0),
                    (keyboard[Key.S] ? 1 : 0) - (keyboard[Key.W] ? 1 : 0));
            }

            if (returnVector != Vector3.Zero)
            {
                returnVector = Vector3.Normalize(returnVector);
            }

            return returnVector;
        }

        public Vector3 WalkingDirection(KeyboardDevice keyboard)
        {
            Vector3 returnVector = Vector3.Zero;

            if (Variables.Game.IsActive)
            {
                returnVector = new Vector3((keyboard[Key.D] ? 1 : 0) - (keyboard[Key.A] ? 1 : 0),
                    0,
                    (keyboard[Key.S] ? 1 : 0) - (keyboard[Key.W] ? 1 : 0));
            }

            if (returnVector != Vector3.Zero)
            {
                returnVector = Vector3.Normalize(returnVector);
            }

            return returnVector;
        }
    }
}
