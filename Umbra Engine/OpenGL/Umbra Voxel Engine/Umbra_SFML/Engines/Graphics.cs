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
            Shaders.CompileShader();

            Matrix4 proj = Constants.Engine_Physics.Player.ProjectionMatrix;
            GL.UniformMatrix4(Shaders.ProjectionMatrixID, false, ref proj);

            RenderHelp.CreateTexture(out TextureID, "content/textures.png");

            base.Initialize(e);
        }

        public override void Render(FrameEventArgs e)
        {
            //Console.Write("FPS: " + 1.0F / e.Time);

            RenderChunks();

            base.Render(e);
        }

        void RenderChunks()
        {
            GL.UseProgram(Shaders.ChunkShaderProgram);

            RenderHelp.BindTexture(TextureID, TextureUnit.Texture0);

            Matrix4 view = Constants.Engine_Physics.Player.ViewMatrix;
            GL.UniformMatrix4(Shaders.ViewMatrixID, false, ref view);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

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

                c.VertexBuffer.Render(c.Index);
            }

            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        public override void Update(FrameEventArgs e)
        {
            ClockTime.SetTimeOfDayGraphics(e);
            base.Update(e);
        }
    }
}
