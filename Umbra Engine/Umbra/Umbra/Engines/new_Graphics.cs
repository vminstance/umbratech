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
    public class Graphics : DrawableGameComponent
    {
        public GraphicsDeviceManager GraphicsDeviceManager;

        public Effect MainEffect;

        public Graphics(Main main)
            : base(main)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(main);
            GraphicsDeviceManager.PreferredBackBufferWidth = (int)Constants.Graphics.ScreenResolution.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = (int)Constants.Graphics.ScreenResolution.Y;
            GraphicsDeviceManager.IsFullScreen = Constants.Graphics.EnableFullScreen;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDeviceManager.PreferMultiSampling = Constants.Graphics.AntiAliasingEnabled;
            GraphicsDevice.Clear(Variables.Graphics.ScreenClearColor);

            // Fucking reset the fucking RenderStates after fucking drawing with fucking SpriteBatches, so it fucking doesn't fucking fuck up. FUCK!
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            GraphicsDevice.DepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.GreaterEqual,
                ReferenceStencil = 254,
                DepthBufferEnable = true
            };


            //Setup Effect
            MainEffect.CurrentTechnique = MainEffect.Techniques["Voxel"];
            MainEffect.Parameters["xView"].SetValue(Constants.Engine_Physics.Player.GetViewMatrix());
            MainEffect.Parameters["xProjection"].SetValue(Constants.Engine_Physics.Player.GetProjectionMatrix());
            MainEffect.Parameters["xTexture"].SetValue(Constants.Engine_Content.Textures);


            for (int passNumber = 0; passNumber < MainEffect.CurrentTechnique.Passes.Count; passNumber++)
            {
                EffectPass pass = MainEffect.CurrentTechnique.Passes[passNumber];

                foreach (Chunk c in Constants.World.Current.GetChunks())
                {
                    if (
                        c == null ||
                        c.VertexBuffer == null ||
                        c.VertexBuffer.IsDisposed ||
                        c.VertexBuffer.VertexCount == 0 ||
                        c.SetupState == 0 ||
                        c.SetupState == 4
                        )
                    {
                        continue;
                    }

                    MainEffect.Parameters["xWorld"].SetValue(Matrix.CreateTranslation(c.Index.Position));

                    GraphicsDevice.SetVertexBuffers(c.VertexBuffer);
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, c.VertexBuffer.VertexCount / 3);
                }
            }

            MainEffect.CurrentTechnique = MainEffect.Techniques["Cursor"];
            MainEffect.Parameters["xWorld"].SetValue(Matrix.Identity);

            // Draw Block Cursor
            VertexBuffer vertexBuffer = BlockCursor.GetVertexBuffer();
            if (vertexBuffer != null)
            {
                foreach (EffectPass pass in MainEffect.CurrentTechnique.Passes)
                {
                    GraphicsDevice.SetVertexBuffers(vertexBuffer);
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, vertexBuffer.VertexCount / 2);
                }
            }

            base.Draw(gameTime);
        }
    }
}
