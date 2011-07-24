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

namespace Umbra.Engines
{
    public class Physics : Engine
    {
        List<PhysicsObject> PhysicsObjects;

        public Player Player { get { return (Player)PhysicsObjects.First(); } }

        public Physics()
        {
            PhysicsObjects = new List<PhysicsObject>();
            PhysicsObjects.Add(new Player());

        }

        public override void Update(FrameEventArgs e)
        {
            foreach (PhysicsObject currentObject in PhysicsObjects)
            {
                currentObject.ResetForceAccumulator();
                currentObject.Update(e);
                if (currentObject.PhysicsEnabled)
                {
                    UpdateVelocity(currentObject, e);
                    UpdatePosition(currentObject, e);
                }
            }

            base.Update(e);
        }

        private void UpdateVelocity(PhysicsObject currentObject, FrameEventArgs e)
        {
            // To avoid errors, remove velocity if too small.
            currentObject.Velocity.X = (float)Math.Round(currentObject.Velocity.X, 4);
            currentObject.Velocity.Y = (float)Math.Round(currentObject.Velocity.Y, 4);
            currentObject.Velocity.Z = (float)Math.Round(currentObject.Velocity.Z, 4);

            // Gravity
            currentObject.ApplyForce(-Vector3.UnitY * Constants.Physics.Gravity * currentObject.Mass);

            // Buoyancy
            currentObject.ApplyForce(Vector3.UnitY * currentObject.BuoyancyMagnitude);

            // Drag
            currentObject.ApplyForce(-0.5F *
                currentObject.AverageViscosity *
                currentObject.BoundingBox.SurfaceArea *
                currentObject.DragCoefficient *
                (float)currentObject.Velocity.LengthSquared *
                (currentObject.Velocity == Vector3.Zero ? Vector3.One : Vector3.Normalize(currentObject.Velocity)));

            // Surface friction
            Vector3 horizontalVelocity = Vector3.Multiply(currentObject.Velocity, new Vector3(1, 0, 1));

            if (IsOnGround(currentObject) && horizontalVelocity != Vector3.Zero)
            {
                currentObject.ApplyForce((-Vector3.Normalize(horizontalVelocity) * currentObject.KineticFrictionCoefficient * currentObject.Mass * Constants.Physics.Gravity) * horizontalVelocity.Length * Constants.Physics.FrictionSignificance);
            }

            // Update velocity
            currentObject.UpdateVelocity((float)e.Time);
        }

        private void UpdatePosition(PhysicsObject obj, FrameEventArgs e)
        {
            Vector3 newPos = obj.Position + obj.Velocity * (float)e.Time;

            if (PlaceFree(obj, newPos))
            {
                obj.Position = newPos;
            }
            else
            {
                UpdatePositionOneDimension(obj, ref obj.Position.Y, ref obj.Velocity.Y, Vector3.UnitY, e);

                if (Math.Abs(obj.Velocity.X) > Math.Abs(obj.Velocity.Z))
                {
                    UpdatePositionOneDimension(obj, ref obj.Position.X, ref obj.Velocity.X, Vector3.UnitX, e);
                    UpdatePositionOneDimension(obj, ref obj.Position.Z, ref obj.Velocity.Z, Vector3.UnitZ, e);
                }
                else
                {
                    UpdatePositionOneDimension(obj, ref obj.Position.Z, ref obj.Velocity.Z, Vector3.UnitZ, e);
                    UpdatePositionOneDimension(obj, ref obj.Position.X, ref obj.Velocity.X, Vector3.UnitX, e);
                }
            }
        }

        private bool PlaceFree(PhysicsObject obj, Vector3 position)
        {
            foreach (BlockIndex index in obj.BoundingBox.At(position).IntersectionIndices)
            {
                if (Constants.World.Current.GetBlock(index).Solidity)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdatePositionOneDimension(PhysicsObject obj, ref float position, ref float velocity, Vector3 axis, FrameEventArgs e)
        {
            while (!PlaceFree(obj, velocity * axis * (float)e.Time + obj.Position))
            {
                if (Math.Round(velocity, 4) != 0.0F)
                {
                    velocity = velocity / 1.5F;
                }
                else
                {
                    velocity = 0.0F;
                    return;
                }
            }

            position += velocity * (float)e.Time;
        }

        public List<BlockIndex> BlocksBeneath(PhysicsObject obj)
        {
            List<BlockIndex> returnList = new List<BlockIndex>();

            for (int x = (int)Math.Floor(obj.BoundingBox.Min.X); x <= Math.Floor(obj.BoundingBox.Max.X); x++)
            {
                for (int z = (int)Math.Floor(obj.BoundingBox.Min.Z); z <= Math.Floor(obj.BoundingBox.Max.Z); z++)
                {
                    returnList.Add(new BlockIndex(x, (int)Math.Floor(obj.Position.Y - Constants.Player.MinDistanceToGround), z));
                }
            }

            return returnList;
        }

        public bool IsOnGround(PhysicsObject obj)
        {
            return !PlaceFree(obj, obj.Position - Vector3.UnitY * Constants.Player.MinDistanceToGround);
        }
    }
}
