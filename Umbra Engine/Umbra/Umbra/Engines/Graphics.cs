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
    public class Graphics : DrawableGameComponent
    {
        public GraphicsDeviceManager GraphicsDeviceManager;

        public Effect MainEffect;

        public Graphics(Main main)
            : base(main)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(main);
            GraphicsDeviceManager.PreferredBackBufferWidth = (int)Constants.ScreenResolution.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = (int)Constants.ScreenResolution.Y;
            GraphicsDeviceManager.IsFullScreen = Constants.EnableFullScreen;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDeviceManager.PreferMultiSampling = Constants.AntiAliasingEnabled;
            GraphicsDevice.Clear(Constants.ScreenClearColor);

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
            MainEffect.Parameters["xView"].SetValue(Constants.Player.GetViewMatrix());
            MainEffect.Parameters["xProjection"].SetValue(Constants.Player.GetProjectionMatrix());
            MainEffect.Parameters["xTexture"].SetValue(Constants.Content.Textures);
            MainEffect.Parameters["xViewPort"].SetValue(new float[] { Constants.ScreenResolution.X, Constants.ScreenResolution.Y });
            MainEffect.Parameters["xFlashEnabled"].SetValue(Constants.FlashLightEnabled);

            ClockTime.SetTimeOfDayGraphics(gameTime);
            MainEffect.Parameters["xFogColor"].SetValue(Constants.CurrentFogColor);
            MainEffect.Parameters["xFogStart"].SetValue(Constants.CurrentFogStart);
            MainEffect.Parameters["xFogEnd"].SetValue(Constants.CurrentFogEnd);
            MainEffect.Parameters["xFogEnabled"].SetValue(Constants.FogEnabled);
            MainEffect.Parameters["xFaceLightCoef"].SetValue(Constants.CurrentFaceLightCoef);


            MainEffect.Parameters["xTranslucentBlocks"].SetValue(Constants.TranslucentBlocks);
            MainEffect.Parameters["xIsUnderWater"].SetValue(Block.GetType(Constants.CurrentWorld.GetBlock(new BlockIndex(Constants.Player.Position + Constants.PlayerEyeHeight * Vector3.UnitY))) == Block.Water);
            MainEffect.Parameters["xTime"].SetValue((int)gameTime.TotalGameTime.TotalMilliseconds);
            

            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            foreach (EffectPass pass in MainEffect.CurrentTechnique.Passes)
            {
                foreach (Chunk c in Constants.CurrentWorld.GetChunks())
                {
                    if (
                        c == null ||
                        c.VertexBuffer == null ||
                        c.VertexBuffer.IsDisposed ||
                        c.VertexBuffer.VertexCount == 0 ||
                        !c.IsSetup
                        )
                    {
                        continue;
                    }

                    MainEffect.Parameters["xWorld"].SetValue(Matrix.CreateTranslation(c.Index.Position));
                    MainEffect.Parameters["xCameraPos"].SetValue(Constants.Player.FirstPersonCamera.Position - c.Index.Position);

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
