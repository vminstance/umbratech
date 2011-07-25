using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbra.Structures.Graphics
{
    static public class VertexShader
    {

        // READ: http://www.opengl.org/wiki/GLSL_:_common_mistakes

        static public readonly string Shader = @"

        in vec4 pos_data;
        in vec4 col_data;
        in vec4 tex_data;

        uniform mat4 projection_mat;
        uniform mat4 view_mat;
        uniform mat4 world_mat;

        //out vec4 colorShade;
        //out vec4 textureCoord;
 
        void main() 
        {
            gl_FrontColor = col_data;
            //textureCoord = tex_data / 16.0;

            gl_Position = projection_mat * view_mat * world_mat * pos_data;
        }";
    }
}
