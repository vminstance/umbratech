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

        uniform sampler2D texture;

        void main()
        {
            gl_FragColor = gl_Color;
            gl_FragColor *= texture2D(texture, gl_TexCoord[0].xy);

            if(gl_FragColor.a < 1.0)
            {
                discard;
            }
        }";

        static public readonly string AlphaShader = @"

        uniform sampler2D texture;

        void main()
        {
            gl_FragColor = gl_Color;
            gl_FragColor *= texture2D(texture, gl_TexCoord[0].xy);

            if(gl_FragColor.a == 0.0 || gl_FragColor.a == 1.0)
            {
                discard;
            }
        }";
    }
}
