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
    public abstract class PhysicsObject
    {
        public bool PhysicsEnabled;

        public Vector3 Position;
        public Vector3 Velocity;
        private Vector3 ForceAccumulator;
        public AABB BoundingBox { get { return new AABB(Position, Position + Dimensions); } }
        public Vector3 Dimensions { get; protected set; }

        public float Volume { get; protected set; }
        public float Mass { get; protected set; }
        public float DragCoefficient { get; protected set; }
        public float SurfaceFrictionCoefficient { get; protected set; }

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
