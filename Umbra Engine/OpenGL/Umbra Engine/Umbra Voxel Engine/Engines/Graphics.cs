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
using Console = Umbra.Implementations.Console;

namespace Umbra.Engines
{
    public class Graphics : Engine
    {
        int TextureID;

        public Graphics()
        {
        }

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

            RenderHelp.CreateTexture(out TextureID, "content/textures.png");
            RenderHelp.BindTexture(TextureID, TextureUnit.Texture0);

            Matrix4 view = Constants.Engine_Physics.Player.ViewMatrix;
            GL.UniformMatrix4(Shaders.ViewMatrixID, false, ref view);
        }

        private void UseShader(int shader)
        {
            GL.UseProgram(shader);

            RenderHelp.BindTexture(TextureID, TextureUnit.Texture0);

            Matrix4 view = Constants.Engine_Physics.Player.ViewMatrix;
            GL.UniformMatrix4(Shaders.ViewMatrixID, false, ref view);
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Render
            {
                UseShader(Shaders.DefaultShaderProgram.ProgramID);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                RenderChunks();

                UseShader(Shaders.ChunkAlphaShaderProgram.ProgramID);
                RenderChunks();

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                BlockCursor.GetVertexBuffer().Render();
            }


            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.PolygonSmooth);
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

        public override void Update(FrameEventArgs e)
        {
            ClockTime.SetTimeOfDayGraphics(e);
            base.Update(e);
        }
    }
}
