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

        public Physics(Main main) : base(main)
        {
            PhysicsObjects = new List<PhysicsObject>();
            PhysicsObjects.Add(new Player(Constants.PlayerMass, Constants.PlayerVolume));

        }

        public override void Update(GameTime gameTime)
        {
            foreach (PhysicsObject physObj in PhysicsObjects)
            {
                physObj.Update(gameTime);
                UpdatePhysics(physObj, gameTime);
            }

            base.Update(gameTime);
        }

        private void UpdatePhysics(PhysicsObject physObj, GameTime gameTime)
        {
            physObj.ResetForceAccumulator();

            physObj.ApplyForce(Vector3.Down * Constants.Gravity * physObj.Mass);
            physObj.ApplyForce(Vector3.Up * GetBuoyancy(physObj) * physObj.Mass);

            physObj.UpdateVelocity((float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        private float GetBuoyancy(PhysicsObject physObj)
        {
            float buoyancy = 0;

            foreach (BlockIndex index in physObj.BoundingBox.IntersectionSpace)
            {
                buoyancy += Block.GetMass(Constants.CurrentWorld.GetBlock(index));
            }

            return buoyancy;
        }
    }
}
