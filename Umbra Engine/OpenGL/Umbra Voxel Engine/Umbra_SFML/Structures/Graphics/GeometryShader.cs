using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbra.Structures.Graphics
{
    static public class GeometryShader
    {

        // READ: http://www.opengl.org/wiki/GLSL_:_common_mistakes

        static public readonly string Shader = @"

        layout(triangles) in;
        layout(triangle_strip, max_vertices = 3) out;
 
        void main() 
        {
            for(int i = 0; i < gl_in.length(); i++) 
            {
                gl_Position = gl_in[i].gl_Position;
                EmitVertex();
            }
            EndPrimitive();
        }";
    }
}
