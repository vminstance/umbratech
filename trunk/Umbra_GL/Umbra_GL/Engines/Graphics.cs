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
    public class Graphics : IEngine
    {
        public IGraphicsContext Context;

        public Graphics()
        {
            Context = GraphicsContext.CurrentContext;

            //GraphicsDeviceManager = new GraphicsDeviceManager(Constants.Main);
            //GraphicsDeviceManager.PreferredBackBufferWidth = (int)Constants.ScreenResolution.X;
            //GraphicsDeviceManager.PreferredBackBufferHeight = (int)Constants.ScreenResolution.Y;
            //GraphicsDeviceManager.IsFullScreen = Constants.EnableFullScreen;

            Constants.Content.LoadTexture(Constants.TerrainTextureFilename);
        }

        public void Run()
        {
            GraphicsContext.Assert();

            GL.ShadeModel(OpenGL.ShadingModel.Smooth);

            GL.Enable(OpenGL.EnableCap.CullFace);
            GL.CullFace(OpenGL.CullFaceMode.Back);

            GL.Enable(OpenGL.EnableCap.DepthTest);
            GL.DepthFunc(OpenGL.DepthFunction.Always);
            GL.DepthMask(true);
            GL.ClearDepth(1);

            GL.Enable(OpenGL.EnableCap.Texture2D);
            GL.Enable(OpenGL.EnableCap.Blend);

            GL.Enable(OpenGL.EnableCap.ScissorTest);

            GL.Hint(OpenGL.HintTarget.PerspectiveCorrectionHint, OpenGL.HintMode.Nicest);

            GL.Enable(OpenGL.EnableCap.Lighting);
            GL.Enable(OpenGL.EnableCap.Light0);
            GL.Enable(OpenGL.EnableCap.ColorMaterial);

            GL.Light(OpenGL.LightName.Light0, OpenGL.LightParameter.Ambient, new Color4(0, 0, 0, 1f));
            GL.Light(OpenGL.LightName.Light0, OpenGL.LightParameter.Diffuse, new Color4(1f, 1f, 1f, 1f));
            GL.Light(OpenGL.LightName.Light0, OpenGL.LightParameter.Position, new float[] { 100, 100, 100 });
        }

        public override void Render(GameTime gameTime)
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
            Effect.CurrentTechnique = Effect.Techniques["Voxel"];
            Effect.Parameters["xView"].SetValue(Constants.Player.GetViewMatrix());
            Effect.Parameters["xProjection"].SetValue(Constants.Player.GetProjectionMatrix());
            Effect.Parameters["xTexture"].SetValue(Constants.Content.Textures);

            ClockTime.SetTimeOfDayGraphics(gameTime);
            Effect.Parameters["xFogColor"].SetValue(Constants.CurrentFogColor);
            Effect.Parameters["xFogStart"].SetValue(Constants.CurrentFogStart);
            Effect.Parameters["xFogEnd"].SetValue(Constants.CurrentFogEnd);
            Effect.Parameters["xFogEnabled"].SetValue(Constants.FogEnabled);
            Effect.Parameters["xFaceLightCoef"].SetValue(Constants.CurrentFaceLightCoef);

            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
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

                    Effect.Parameters["xWorld"].SetValue(Matrix.CreateTranslation(c.Index.Position));
                    Effect.Parameters["xCameraPos"].SetValue(Constants.Player.FirstPersonCamera.Position - c.Index.Position);

                    GraphicsDevice.SetVertexBuffers(c.VertexBuffer);
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, c.VertexBuffer.VertexCount / 3);
                }
            }

            Effect.CurrentTechnique = Effect.Techniques["Cursor"];
            Effect.Parameters["xWorld"].SetValue(Matrix.Identity);

            // Draw Block Cursor
            VertexBuffer vertexBuffer = BlockCursor.GetVertexBuffer();
            if (vertexBuffer != null)
            {
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
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
