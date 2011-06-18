using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;
using Umbra_Engine;

namespace Umbra_Engine.Engines
{
    class GraphicsEngine : IComponent
    {
        Game Game;
        GameWindow Main;
        IGraphicsContext Context;

        Matrix4 cameraMatrix;
        Matrix4 projectionMatrix;

        Chunk[,,] Chunks;

        int count;
        LinkedList<uint> Textures;

        public Camera Camera;

        public GraphicsEngine(Game game)
        {
            Game = game;
            Main = Game.Main;
            Context = Game.Context;

            cameraMatrix = Matrix4.LookAt(new Vector3(0, 0, 0), Vector3.Zero, Vector3.UnitY);
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver6, (float)Main.Size.Width / (float)Main.Size.Height, 0.1f, 64000f);

            Camera = new Umbra_Engine.Camera(new Vector3(0, 32, 0), 0f, 0f, Game);
            Game.AddComponent(Camera);


            int Xsize = 2;
            int Ysize = 1;
            int Zsize = 2;

            Chunks = new Chunk[Xsize, Ysize, Zsize];

            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    for (int z = 0; z < Zsize; z++)
                    {
                        Chunks[x, y, z] = new Chunk(new Vector3((Xsize - x) * 32, (Ysize - y) * 32, (Zsize - z) * 32));
                    }
                }
            }


            Textures = new LinkedList<uint>();
            Textures.AddLast(Content.LoadTexture(@"Content\\sample.png"));
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
            GL.Light(OpenGL.LightName.Light0, OpenGL.LightParameter.Position, new float[] {100, 100, 100});
        }

        public void Update()
        {

            cameraMatrix = Camera.GetView();
            projectionMatrix = Camera.GetProjection((float)Main.Size.Width / (float)Main.Size.Height);
        }


        uint VBOid;
        uint IBOid;

        public void Render()
        {

            GL.EnableClientState(OpenGL.ArrayCap.VertexArray);
            GL.EnableClientState(OpenGL.ArrayCap.ColorArray);
            GL.EnableClientState(OpenGL.ArrayCap.NormalArray);
            GL.EnableClientState(OpenGL.ArrayCap.TextureCoordArray);


            GL.ClearColor(Color.CornflowerBlue);

            GL.MatrixMode(OpenGL.MatrixMode.Projection);
            //GL.LoadIdentity();
            GL.LoadMatrix(ref projectionMatrix);


            GL.MatrixMode(OpenGL.MatrixMode.Modelview);
            //GL.LoadIdentity();
            GL.LoadMatrix(ref cameraMatrix);

            foreach (Chunk c in Chunks)
            {
                //uint PBOid;
                GL.GenBuffers(1, out VBOid);
                GL.GenBuffers(1, out IBOid);
                //GL.GenBuffers(1, out PBOid);
                
                //GL.ClientActiveTexture(OpenGL.TextureUnit.Texture0);
                //GL.BindTexture(OpenGL.TextureTarget.ProxyTexture2DArray, TBOid);

                //GL.TexBuffer(OpenGL.TextureBufferTarget.TextureBuffer, OpenGL.SizedInternalFormat.Rgba32f, TBOid);
                //GL.BindBuffer(OpenGL.BufferTarget.PixelUnpackBuffer, PBOid);
                //GL.BufferData(OpenGL.BufferTarget.PixelUnpackBuffer, (IntPtr)(sizeof(uint) * c.Textures.Length), c.Textures, OpenGL.BufferUsageHint.StreamDraw);

                GL.BindBuffer(OpenGL.BufferTarget.ArrayBuffer, VBOid);
                GL.BufferData(OpenGL.BufferTarget.ArrayBuffer, (IntPtr)(c.Vertices.Length * 12 * sizeof(float)), c.Vertices, OpenGL.BufferUsageHint.StaticDraw);

                GL.BindBuffer(OpenGL.BufferTarget.ElementArrayBuffer, IBOid);
                GL.BufferData(OpenGL.BufferTarget.ElementArrayBuffer, (IntPtr)(c.Indices.Length * sizeof(uint)), c.Indices, OpenGL.BufferUsageHint.StaticDraw);

                GL.InterleavedArrays(OpenGL.InterleavedArrayFormat.T2fC4fN3fV3f, 0, IntPtr.Zero);

                GL.DrawElements(OpenGL.BeginMode.Triangles, c.Indices.Length, OpenGL.DrawElementsType.UnsignedInt, 0);
            }


            GL.DisableClientState(OpenGL.ArrayCap.VertexArray);
            GL.DisableClientState(OpenGL.ArrayCap.ColorArray);
            GL.DisableClientState(OpenGL.ArrayCap.NormalArray);
            GL.DisableClientState(OpenGL.ArrayCap.TextureCoordArray);
        }
    }
}
