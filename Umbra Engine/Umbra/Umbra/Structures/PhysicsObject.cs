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
    abstract class PhysicsObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        private Vector3 ForceAccumulator;
        public AABB BoundingBox { get; protected set; }

        public float Volume { get; protected set; }
        public float Mass { get; protected set; }
        public float DragCoefficient { get; protected set; }
        public float SurfaceFrictionCoefficient { get; protected set; }

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
