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
                currentObject.Update(gameTime);
                UpdateVelocity(currentObject, gameTime);
                UpdatePosition(currentObject, gameTime);
            }

            base.Update(gameTime);
        }

        private void UpdateVelocity(PhysicsObject currentObject, GameTime gameTime)
        {
            currentObject.ResetForceAccumulator();

            // Gravity
            currentObject.ApplyForce(Vector3.Down * Constants.Gravity * currentObject.Mass);

            // Buoyancy
            currentObject.ApplyForce(Vector3.Up * GetBuoyancyForce(currentObject));

            // Drag
            currentObject.ApplyForce(-0.5F * GetAverageViscosity(currentObject) * currentObject.BoundingBox.SurfaceArea * currentObject.DragCoefficient * (float)currentObject.Velocity.LengthSquared() * (currentObject.Velocity == Vector3.Zero ? Vector3.One : Vector3.Normalize(currentObject.Velocity)));

            // Surface friction
            currentObject.ApplyForce(Vector3.Zero);

            // Update velocity
            currentObject.UpdateVelocity((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private float GetBuoyancyForce(PhysicsObject obj)
        {
            float buoyancy = 0;

            foreach (BlockIndex index in obj.BoundingBox.IntersectionSpace)
            {
                buoyancy += Constants.CurrentWorld.GetBlock(index).Mass;
            }

            return (2 * -Constants.Gravity * obj.Mass * buoyancy) / (obj.Mass + buoyancy);
        }

        private float GetAverageViscosity(PhysicsObject obj)
        {
            float average = 0;

            foreach (BlockIndex index in obj.BoundingBox.IntersectionSpace)
            {
                average += Constants.CurrentWorld.GetBlock(index).Viscosity * (index.GetBoundingBox().IntersectionVolume(obj.BoundingBox) / obj.Volume);
            }

            return average;
        }

        private float GetAverageFrictionCoefficient(PhysicsObject obj)
        {
            float average = 0;

            foreach (BlockIndex index in obj.BoundingBox.IntersectionSpace)
            {
                average += Constants.CurrentWorld.GetBlock(index).FrictionCoefficient * (index.GetBoundingBox().IntersectionVolume(obj.BoundingBox) / obj.Volume);
            }

            return average;
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
                UpdatePositionOneDimension(obj, newPos, ref obj.Position.Y, ref obj.Velocity.Y);

                if (obj.Velocity.X > obj.Velocity.Z)
                {
                    UpdatePositionOneDimension(obj, newPos, ref obj.Position.X, ref obj.Velocity.X);
                    UpdatePositionOneDimension(obj, newPos, ref obj.Position.Z, ref obj.Velocity.Z);
                }
                else
                {
                    UpdatePositionOneDimension(obj, newPos, ref obj.Position.Z, ref obj.Velocity.Z);
                    UpdatePositionOneDimension(obj, newPos, ref obj.Position.X, ref obj.Velocity.X);
                }
            }
        }

        private bool PlaceFree(PhysicsObject obj, Vector3 position)
        {
            foreach (BlockIndex index in obj.BoundingBox.At(position).IntersectionSpace)
            {
                if (Constants.CurrentWorld.GetBlock(index).Solidity)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdatePositionOneDimension(PhysicsObject obj, Vector3 newPosition, ref float position, ref float velocity)
        {
            if (velocity != 0)
            {
                if (!PlaceFree(obj, newPosition))
                {
                    // Debounce
                }

                velocity = 0;
            }

            position += velocity;
        }

        public bool ObjectOnGround(PhysicsObject obj)
        {
            return PlaceFree(obj, obj.Position + Vector3.Down * Constants.PlayerMinDistanceToGround);
        }
    }
}
