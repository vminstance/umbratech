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
            MainEffect.Parameters["xViewPort"].SetValue(new float[] { Constants.Graphics.ScreenResolution.X, Constants.Graphics.ScreenResolution.Y });
            MainEffect.Parameters["xFlashEnabled"].SetValue(Variables.Graphics.Lighting.FlashLightEnabled);

            ClockTime.SetTimeOfDayGraphics(gameTime);
            MainEffect.Parameters["xFogColor"].SetValue(Variables.Graphics.Fog.CurrentColor);
            MainEffect.Parameters["xFogStart"].SetValue(Variables.Graphics.Fog.CurrentStart);
            MainEffect.Parameters["xFogEnd"].SetValue(Variables.Graphics.Fog.CurrentEnd);
            MainEffect.Parameters["xFogEnabled"].SetValue(Variables.Graphics.Fog.Enabled);
            MainEffect.Parameters["xFaceLightCoef"].SetValue(Variables.Graphics.DayNight.CurrentFaceLightCoef);


            MainEffect.Parameters["xTranslucentBlocks"].SetValue(Constants.Graphics.TranslucentBlocks);
            MainEffect.Parameters["xIsUnderWater"].SetValue(Constants.World.Current.GetBlock(new BlockIndex(Constants.Engine_Physics.Player.Position + Constants.Player.Physics.EyeHeight * Vector3.UnitY)).Type == (byte)BlockType.Water);
            MainEffect.Parameters["xTime"].SetValue((int)gameTime.TotalGameTime.TotalMilliseconds);





            for (int passNumber = 0; passNumber < MainEffect.CurrentTechnique.Passes.Count; passNumber++)
            {
                if (passNumber == 0)
                {
                    GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
                }
                else if (passNumber == 1)
                {
                    GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                }


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
                    MainEffect.Parameters["xCameraPos"].SetValue(Constants.Engine_Physics.Player.FirstPersonCamera.Position - c.Index.Position);

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
