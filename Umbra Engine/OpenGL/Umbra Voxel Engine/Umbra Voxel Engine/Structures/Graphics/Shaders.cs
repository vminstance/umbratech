﻿using System;
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
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures.Graphics
{
    static public class Shaders
    {
        static public Shader DefaultShaderProgram { get; private set; }
        static public Shader ChunkAlphaShaderProgram { get; private set; }

        static public void CompileShaders()
        {
            DefaultShaderProgram = new Shader(VertexShader.Shader, FragmentShader.Shader);
            ChunkAlphaShaderProgram = new Shader(VertexShader.Shader, FragmentShader.AlphaShader);

            GetVariables(DefaultShaderProgram.ProgramID);
        }

        static public int ProjectionMatrixID { get; private set; }
        static public int ViewMatrixID { get; private set; }
        static public int WorldMatrixID { get; private set; }
        static public int LightDirectionID { get; private set; }

        static public int TextureID { get; private set; }
        static public int ViewTypeID { get; private set; }

        static public int DataID { get; private set; }


        static private void GetVariables(int shaderProgram)
        {

            ProjectionMatrixID = GL.GetUniformLocation(shaderProgram, "projection_mat");
            ViewMatrixID = GL.GetUniformLocation(shaderProgram, "view_mat");
            WorldMatrixID = GL.GetUniformLocation(shaderProgram, "world_mat");
            LightDirectionID = GL.GetUniformLocation(shaderProgram, "light_dir");

            TextureID = GL.GetUniformLocation(shaderProgram, "texture");
            ViewTypeID = GL.GetUniformLocation(shaderProgram, "view_type");

            DataID = GL.GetAttribLocation(shaderProgram, "data");
        }
    }

    public class Shader
    {
        public int ProgramID { get; private set; }

        public Shader(string vertexShader, string fragmentShader)
        {
            CompileShader(vertexShader, fragmentShader);
            GL.UseProgram(ProgramID);
        }

        public void CompileShader(string vertexShader, string fragmentShader)
        {
            int VertexShaderID;
            //int GeometryShaderID;
            int FragmentShaderID;

            ProgramID = GL.CreateProgram();

            VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
            //GeometryShaderID = GL.CreateShader(ShaderType.GeometryShaderExt);
            FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(VertexShaderID, vertexShader);
            //GL.ShaderSource(GeometryShaderID, GeometryShader.Shader);
            GL.ShaderSource(FragmentShaderID, fragmentShader);

            GL.CompileShader(VertexShaderID);
            //GL.CompileShader(GeometryShaderID);
            GL.CompileShader(FragmentShaderID);


            int compileResult;
            GL.GetShader(VertexShaderID, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                throw new Exception("Error while compiling the vertex shader. This means that you probably have an outdated graphics card driver.");
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
                throw new Exception("Error while compiling the fragment shader. This means that you probably have an outdated graphics card driver.");
            }


            GL.AttachShader(ProgramID, VertexShaderID);
            //GL.AttachShader(ShaderProgram, GeometryShaderID);
            GL.AttachShader(ProgramID, FragmentShaderID);

            GL.LinkProgram(ProgramID);

            string info = GL.GetProgramInfoLog(ProgramID);
            System.Console.WriteLine(info);


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
        }
    }
}
