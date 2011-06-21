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
        public Player(float mass, float volume)
        {
            this.Mass = mass;
            this.Volume = volume;
        }

        public override void Update(GameTime gameTime)
        {
            // Add player movement
            if (Constants.NoclipEnabed)
            {
                Position += Constants.Input.NoclipDirection() *  Constants.NoclipSpeed;
            }
            else
            {
                float oldSpeed = Velocity.Length();
                
                Velocity += Constants.Input.WalkingDirection();
                Velocity.Normalize();
                Velocity *= Math.Min(oldSpeed * Constants.CurrentValues[0], Constants.CurrentValues[2]);
            }

            base.Update(gameTime);
        }
    }
}
