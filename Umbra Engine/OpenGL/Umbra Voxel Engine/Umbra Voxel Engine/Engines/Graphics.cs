using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Graphics;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Engines
{
    public class Graphics : Engine
    {
        int TextureID;

        public override void Initialize(EventArgs e)
        {
            Shaders.CompileShaders();

            InitShader(Shaders.DefaultShaderProgram.ProgramID);
            InitShader(Shaders.ChunkAlphaShaderProgram.ProgramID);

            base.Initialize(e);
        }

        private void InitShader(int shader)
        {
            GL.UseProgram(shader);

            Matrix4 proj = Constants.Engine_Physics.Player.ProjectionMatrix;
            GL.UniformMatrix4(Shaders.ProjectionMatrixID, false, ref proj);

            RenderHelp.CreateTexture2DArray(out TextureID, Block.GetBlockTexturePaths());
            RenderHelp.BindTexture(TextureID, TextureUnit.Texture0);

            Matrix4 view = Constants.Engine_Physics.Player.ViewMatrix;
            GL.UniformMatrix4(Shaders.ViewMatrixID, false, ref view);

            GL.UseProgram(0);
        }

        private void UseShader(int shader)
        {
            GL.UseProgram(shader);

            if (shader == 0)
            {
                return;
            }

            RenderHelp.BindTexture(TextureID, TextureUnit.Texture0);

            Matrix4 view = Constants.Engine_Physics.Player.ViewMatrix;
            GL.UniformMatrix4(Shaders.ViewMatrixID, false, ref view);

            GL.Uniform3(Shaders.LightDirectionID, Constants.Graphics.Lighting.DiffuseLightDirection);

            GL.Uniform1(Shaders.ViewTypeID, Constants.Engine_Physics.Player.GetViewType());
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Render World
            {
                UseShader(Shaders.DefaultShaderProgram.ProgramID);
                RenderChunks();


                GL.Disable(EnableCap.CullFace);

                UseShader(Shaders.ChunkAlphaShaderProgram.ProgramID);
                RenderChunks();

                GL.Enable(EnableCap.CullFace);
            }

			GL.Disable(EnableCap.Texture2D);

			UseShader(0);
			RenderCursor();

            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
			GL.Disable(EnableCap.Blend);

            base.Render(e);
        }

        void RenderChunks()
        {
            foreach (Chunk c in Constants.World.Current.GetChunks())
            {
                if (
                    c == null ||
                    c.SetupState == 0 ||
                    c.SetupState == 4 ||
                    !c.ValidateVertexBuffer()
                    )
                {
                    continue;
                }

                c.VertexBuffer.Render();
            }
        }

        void RenderCursor()
        {
            if (Constants.Graphics.BlockCursorType == 0)
            {
                return;
            }

            BlockIndex currentAim = BlockCursor.GetToDestroy();

            if (currentAim == null)
            {
                return;
            }

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 proj = Constants.Engine_Physics.Player.ProjectionMatrix;
            GL.LoadMatrix(ref proj);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 view = Constants.Engine_Physics.Player.ViewMatrix;
            GL.LoadMatrix(ref view);

            if (Constants.Graphics.BlockCursorType == 1 || Constants.Graphics.BlockCursorType == 3)
            {
                GL.Color4(0.0, 0.0, 0.0, 0.2);
                GL.Begin(BeginMode.Quads);
                {
                    GL.Vertex3(new Vector3d(1.004, -0.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, 1.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, 1.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, -0.004, 1.004) + currentAim.Position);

                    GL.Vertex3(new Vector3d(-0.004, -0.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, 1.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, 1.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, -0.004, -0.004) + currentAim.Position);

                    GL.Vertex3(new Vector3d(1.004, 1.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, 1.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, 1.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, 1.004, 1.004) + currentAim.Position);

                    GL.Vertex3(new Vector3d(-0.004, -0.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, -0.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, -0.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, -0.004, 1.004) + currentAim.Position);

                    GL.Vertex3(new Vector3d(1.004, -0.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, 1.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, 1.004, 1.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, -0.004, 1.004) + currentAim.Position);

                    GL.Vertex3(new Vector3d(-0.004, -0.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(-0.004, 1.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, 1.004, -0.004) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1.004, -0.004, -0.004) + currentAim.Position);
                }
                GL.End();
            }

            if (Constants.Graphics.BlockCursorType == 2 || Constants.Graphics.BlockCursorType == 3)
            {
                GL.Disable(EnableCap.DepthTest);

                GL.Color4(0.0, 0.0, 0.0, 0.2);
                GL.Begin(BeginMode.Lines);
                {
                    GL.Vertex3(new Vector3d(0, 0, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 0, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 0, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 0, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 1, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 1, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 1, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 1, 1) + currentAim.Position);

                    GL.Vertex3(new Vector3d(0, 0, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 1, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 0, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 1, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 0, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 1, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 0, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 1, 1) + currentAim.Position);

                    GL.Vertex3(new Vector3d(0, 0, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 0, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 1, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(0, 1, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 0, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 0, 1) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 1, 0) + currentAim.Position);
                    GL.Vertex3(new Vector3d(1, 1, 1) + currentAim.Position);
                }
                GL.End();
                GL.Enable(EnableCap.DepthTest);
            }
        }

        public override void Update(FrameEventArgs e)
        {
            ClockTime.SetTimeOfDayGraphics(e);
            base.Update(e);
        }
    }
}
