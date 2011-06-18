using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Internal;
using Vector3 = Umbra.Structures.Internal.Vector3;
using Console = Umbra.Implementations.Console;

namespace Umbra.Engines
{
    public class Overlay : IEngine
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont FrameCounterFont { get; set; }

        public Overlay()
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

        public override void Render(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            SpriteBatch.Draw(Constants.Content.CrosshairTexture, new Vector2((Constants.ScreenResolution.X - Constants.Content.CrosshairTexture.Width) / 2, (Constants.ScreenResolution.Y - Constants.Content.CrosshairTexture.Height) / 2), Color.White);
            Console.Draw();
            Popup.Draw();
            Compass.Draw();
            if (Constants.DisplayFPS)
            {
                SpriteBatch.DrawString(FrameCounterFont, Convert.ToString(1000f / (float)gameTime.ElapsedGameTime.Milliseconds), new Vector2(10, 5), Color.Yellow, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
