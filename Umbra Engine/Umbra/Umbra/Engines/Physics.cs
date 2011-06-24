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
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra.Engines
{
    public class Physics : DrawableGameComponent
    {
        List<PhysicsObject> PhysicsObjects;

        public Player Player { get { return (Player)PhysicsObjects.First(); } }

        public Physics(Main main)
            : base(main)
        {
            PhysicsObjects = new List<PhysicsObject>();
            PhysicsObjects.Add(new Player());

        }

        public override void Update(GameTime gameTime)
        {
            foreach (PhysicsObject currentObject in PhysicsObjects)
            {
                currentObject.ResetForceAccumulator();
                currentObject.Update(gameTime);
                if (currentObject.PhysicsEnabled)
                {
                    UpdateVelocity(currentObject, gameTime);
                    UpdatePosition(currentObject, gameTime);
                }
            }

            base.Update(gameTime);
        }

        private void UpdateVelocity(PhysicsObject currentObject, GameTime gameTime)
        {

            // Gravity
            currentObject.ApplyForce(Vector3.Down * Constants.Physics.Gravity * currentObject.Mass);

            // Buoyancy
            currentObject.ApplyForce(Vector3.Up * currentObject.BuoyancyMagnitude);

            // Drag
            currentObject.ApplyForce(-0.5F *
                currentObject.AverageViscosity *
                currentObject.BoundingBox.SurfaceArea *
                currentObject.DragCoefficient *
                (float)currentObject.Velocity.LengthSquared() *
                (currentObject.Velocity == Vector3.Zero ? Vector3.One : Vector3.Normalize(currentObject.Velocity)));
            
            // Surface friction
            Vector3 horizontalVelocity = (currentObject.Velocity * new Vector3(1, 0, 1));

            if (IsOnGround(currentObject) && horizontalVelocity != Vector3.Zero)
            {
                currentObject.ApplyForce((-Vector3.Normalize(horizontalVelocity) * currentObject.KineticFrictionCoefficient * currentObject.Mass * Constants.Physics.Gravity) * horizontalVelocity.Length() * Constants.Physics.FrictionSignificance);
            }

            // Update velocity
            currentObject.UpdateVelocity((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void UpdatePosition(PhysicsObject obj, GameTime gameTime)
        {
            Vector3 newPos = obj.Position + obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (PlaceFree(obj, newPos))
            {
                obj.Position = newPos;
            }
            else
            {
                UpdatePositionOneDimension(obj, ref obj.Position.Y, ref obj.Velocity.Y, Vector3.UnitY, gameTime);

                if (Math.Abs(obj.Velocity.X) > Math.Abs(obj.Velocity.Z))
                {
                    UpdatePositionOneDimension(obj, ref obj.Position.X, ref obj.Velocity.X, Vector3.UnitX, gameTime);
                    UpdatePositionOneDimension(obj, ref obj.Position.Z, ref obj.Velocity.Z, Vector3.UnitZ, gameTime);
                }
                else
                {
                    UpdatePositionOneDimension(obj, ref obj.Position.Z, ref obj.Velocity.Z, Vector3.UnitZ, gameTime);
                    UpdatePositionOneDimension(obj, ref obj.Position.X, ref obj.Velocity.X, Vector3.UnitX, gameTime);
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

        private void UpdatePositionOneDimension(PhysicsObject obj, ref float position, ref float velocity, Vector3 axis, GameTime gameTime)
        {
            while (!PlaceFree(obj, velocity * axis * (float)gameTime.ElapsedGameTime.TotalSeconds + obj.Position))
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

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public List<BlockIndex> BlocksBeneath(PhysicsObject obj)
        {
            List<BlockIndex> returnList = new List<BlockIndex>();

            for (int x = (int)Math.Floor(obj.BoundingBox.Min.X); x <= Math.Floor(obj.BoundingBox.Max.X); x++)
            {
                for (int z = (int)Math.Floor(obj.BoundingBox.Min.Z); z <= Math.Floor(obj.BoundingBox.Max.Z); z++)
                {
                    returnList.Add(new BlockIndex(x, (int)(obj.Position.Y - Constants.Player.MinDistanceToGround), z));
                }
            }

            return returnList;
        }

        public bool IsOnGround(PhysicsObject obj)
        {
            return !PlaceFree(obj, obj.Position + Vector3.Down * Constants.Player.MinDistanceToGround);
        }
    }
}
