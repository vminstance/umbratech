using System;
using System.Linq;
using System.Text;
using System.Drawing;
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
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra.Structures.Graphics
{
    static public class Shaders
    {
        static public int ChunkShaderProgram { get; private set; }

        static public void CompileShader()
        {
            int VertexShaderID;
            //int GeometryShaderID;
            int FragmentShaderID;

            ChunkShaderProgram = GL.CreateProgram();

            VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
            //GeometryShaderID = GL.CreateShader(ShaderType.GeometryShaderExt);
            FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(VertexShaderID, VertexShader.Shader);
            //GL.ShaderSource(GeometryShaderID, GeometryShader.Shader);
            GL.ShaderSource(FragmentShaderID, FragmentShader.Shader);

            GL.CompileShader(VertexShaderID);
            //GL.CompileShader(GeometryShaderID);
            GL.CompileShader(FragmentShaderID);


            int compileResult;
            GL.GetShader(VertexShaderID, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                Console.Write("Compile Error!");
                Console.Write(VertexShader.Shader);
                System.Windows.Forms.MessageBox.Show("Error while compiling the vertex shader. This means that you probably have an outdated graphics card driver.", "Vertex Shader Error!");
                throw new Exception("Vertex Shader Error!!");
            }

            //GL.GetShader(GeometryShaderID, ShaderParameter.CompileStatus, out compileResult);
            //if (compileResult != 1)
            //{
            //    Console.Write("Compile Error!");
            //    Console.Write(GeometryShader.Shader);
            //}

            GL.GetShader(FragmentShaderID, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                Console.Write("Compile Error!");
                Console.Write(FragmentShader.Shader);
                System.Windows.Forms.MessageBox.Show("Error while compiling the fragment shader. This means that you probably have an outdated graphics card driver.", "Fragment Shader Error!");
                throw new Exception("Fragment Shader Error!!");
            }


            GL.AttachShader(ChunkShaderProgram, VertexShaderID);
            //GL.AttachShader(ShaderProgram, GeometryShaderID);
            GL.AttachShader(ChunkShaderProgram, FragmentShaderID);

            GL.LinkProgram(ChunkShaderProgram);

            string info;
            GL.GetProgramInfoLog(ChunkShaderProgram, out info);
            Console.Write(info);


            //GL.ProgramParameter(ShaderProgram, Version32.GeometryInputType, (int)All.Lines);
            //GL.ProgramParameter(ShaderProgram, Version32.GeometryOutputType, (int)All.LineStrip);

            //int tmp;
            //GL.GetInteger((GetPName)ExtGeometryShader4.MaxGeometryOutputVerticesExt, out tmp);
            //GL.ProgramParameter(ShaderProgram, Version32.GeometryVerticesOut, tmp);



            if (VertexShaderID != 0)
            {
                GL.DeleteShader(VertexShaderID);
            }

            //if (GeometryShaderID != 0)
            //{
            //    GL.DeleteShader(GeometryShaderID);
            //}

            if (FragmentShaderID != 0)
            {
                GL.DeleteShader(FragmentShaderID);
            }

            GL.UseProgram(ChunkShaderProgram);

            GetVariables(ChunkShaderProgram);
        }


        static public int ProjectionMatrixID { get; private set; }
        static public int ViewMatrixID { get; private set; }
        static public int WorldMatrixID { get; private set; }

        static public int TextureID { get; private set; }

        static public int PositionDataID { get; private set; }
        static public int ColorDataID { get; private set; }
        static public int TextureDataID { get; private set; }


        static private void GetVariables(int shaderProgram)
        {

            ProjectionMatrixID = GL.GetUniformLocation(shaderProgram, "projection_mat");
            ViewMatrixID = GL.GetUniformLocation(shaderProgram, "view_mat");
            WorldMatrixID = GL.GetUniformLocation(shaderProgram, "world_mat");

            TextureID = GL.GetUniformLocation(shaderProgram, "texture");

            PositionDataID = GL.GetAttribLocation(shaderProgram, "pos_data");
            ColorDataID = GL.GetAttribLocation(shaderProgram, "col_data");
            TextureDataID = GL.GetAttribLocation(shaderProgram, "tex_data");
        }
    }
}
