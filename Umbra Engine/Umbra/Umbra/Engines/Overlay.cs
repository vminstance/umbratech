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
    public class Overlay : DrawableGameComponent
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont DebugFont { get; set; }

        public Overlay(Main main) 
            : base(main)
        {
        }

        public override void Initialize()
        {
            SpriteBatch = new SpriteBatch(Constants.Graphics.GraphicsDevice);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Console.Update(gameTime);
            Popup.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            SpriteBatch.Draw(Constants.Content.CrosshairTexture, new Vector2((Constants.ScreenResolution.X - Constants.Content.CrosshairTexture.Width) / 2, (Constants.ScreenResolution.Y - Constants.Content.CrosshairTexture.Height) / 2), Color.White);
            Console.Draw(gameTime);
            Popup.Draw();
            Compass.Draw();
            if (Constants.DisplayFPS)
            {
                SpriteBatch.DrawString(DebugFont, Convert.ToString(1000f / (float)gameTime.ElapsedGameTime.Milliseconds), new Vector2(10, 5), Color.Yellow);
            }
            if (Constants.DeveloperMode)
            {
                // Memory
                string memoryUsage = (int)(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024) + " kB";
                SpriteBatch.DrawString(DebugFont, memoryUsage, new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(memoryUsage).X - 10, 100), Color.Yellow);

                // Position
                string[] position = { "Px: " + Math.Round(Constants.Physics.Player.Position.X, 1), "Py: " + Math.Round(Constants.Physics.Player.Position.Y, 1), "Pz: " + Math.Round(Constants.Physics.Player.Position.Z, 1) };
                
                SpriteBatch.DrawString(DebugFont, position[0], new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(position[0]).X - 10, 130), Color.Yellow);
                SpriteBatch.DrawString(DebugFont, position[1], new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(position[1]).X - 10, 150), Color.Yellow);
                SpriteBatch.DrawString(DebugFont, position[2], new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(position[2]).X - 10, 170), Color.Yellow);

                // Velocity
                string[] velocity = { "Vx: " + Math.Round(Constants.Physics.Player.Velocity.X, 1), "Vy: " + Math.Round(Constants.Physics.Player.Velocity.Y, 1), "Vz: " + Math.Round(Constants.Physics.Player.Velocity.Z, 1) };

                SpriteBatch.DrawString(DebugFont, velocity[0], new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(velocity[0]).X - 10, 200), Color.Yellow);
                SpriteBatch.DrawString(DebugFont, velocity[1], new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(velocity[1]).X - 10, 220), Color.Yellow);
                SpriteBatch.DrawString(DebugFont, velocity[2], new Vector2(Constants.ScreenResolution.X - DebugFont.MeasureString(velocity[2]).X - 10, 240), Color.Yellow);
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
