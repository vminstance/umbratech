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

namespace Umbra.Structures
{
    public abstract class PhysicsObject
    {
        public bool PhysicsEnabled;

        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 ForceAccumulator { get; protected set; }
        public AABB BoundingBox { get { return new AABB(Position, Position + Dimensions); } }
        public Vector3 Dimensions { get; protected set; }

        public float Volume { get; protected set; }
        public float Mass { get; protected set; }
        public float DragCoefficient { get; protected set; }
        public float SurfaceFrictionCoefficient { get; protected set; }

        public float BuoyancyMagnitude
        {
            get
            {
                float buoyancy = 0;

                foreach (BlockIndex index in BoundingBox.IntersectionIndices)
                {
                    buoyancy += Constants.World.Current.GetBlock(index).Density * BoundingBox.IntersectionVolume(index.GetBoundingBox());
                }

                return (2 * Constants.Physics.Gravity * Mass * buoyancy) / (Mass + buoyancy);
            }
        }

        public float AverageViscosity
        {
            get
            {
                float average = 0;

                foreach (BlockIndex index in BoundingBox.IntersectionIndices)
                {
                    average += Constants.World.Current.GetBlock(index).Viscosity * (index.GetBoundingBox().IntersectionVolume(BoundingBox) / Volume);
                }

                return average;
            }
        }

        public float KineticFrictionCoefficient
        {
            get
            {
                float maxFriction = 0;

                foreach (BlockIndex index in Constants.Engine_Physics.BlocksBeneath(this))
                {
                    if (maxFriction < Constants.World.Current.GetBlock(index).KineticFrictionCoefficient)
                    {
                        maxFriction = Constants.World.Current.GetBlock(index).KineticFrictionCoefficient;
                    }
                }

                return maxFriction;
            }
        }

        public PhysicsObject(Vector3 position, float mass)
        {
            Position = position;
            Dimensions = Vector3.One;
            Velocity = Vector3.Zero;
            ForceAccumulator = Vector3.Zero;
            Volume = Dimensions.X * Dimensions.Y * Dimensions.Z;
            Mass = mass;
            PhysicsEnabled = true;
        }

        public PhysicsObject(Vector3 position, Vector3 dimension, float mass)
        {
            Position = position;
            Dimensions = dimension;
            Velocity = Vector3.Zero;
            ForceAccumulator = Vector3.Zero;
            Volume = Dimensions.X * Dimensions.Y * Dimensions.Z;
            Mass = mass;
            PhysicsEnabled = true;
        }

        public PhysicsObject(Vector3 position, Vector3 dimension, float mass, float volume)
        {
            Position = position;
            Dimensions = dimension;
            Velocity = Vector3.Zero;
            ForceAccumulator = Vector3.Zero;
            Volume = volume;
            Mass = mass;
            PhysicsEnabled = true;
        }

        public void ResetForceAccumulator()
        {
            ForceAccumulator = Vector3.Zero;
        }

        public void ApplyForce(Vector3 force)
        {
            ForceAccumulator += force;
        }

        public void UpdateVelocity(float timespan)
        {
            Velocity += (ForceAccumulator / Mass) * timespan;
        }

        virtual public void Update(GameTime gameTime)
        {
        }
    }
}
