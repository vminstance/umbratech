using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbra.Structures.Graphics
{
    static public class FragmentShader
    {

        // READ: http://www.opengl.org/wiki/GLSL_:_common_mistakes

        static public readonly string Shader = @"
            
        in vec4 colorShade;
        in vec4 textureCoord;

        uniform sampler2D texture;        

        //out vec4 Output;

        void main()
        {
            gl_FragColor = gl_Color;
            gl_FragColor *= texture2D(texture, textureCoord.xy);

            if(gl_FragColor.a == 0)
            {
                discard;
            }
        }";
    }
}
