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

        uniform sampler2DArray texture;

        void main()
        {
            gl_FragColor = texture2DArray(texture, gl_TexCoord[0].xyz);

            if(gl_FragColor.a < 1.0)
            {
                discard;
            }

            gl_FragColor *= gl_Color;
        }";

        static public readonly string AlphaShader = @"

        uniform sampler2DArray texture;

        void main()
        {
            gl_FragColor = texture2DArray(texture, gl_TexCoord[0].xyz);

            if(gl_FragColor.a == 0.0 || gl_FragColor.a == 1.0)
            {
                discard;
            }

            gl_FragColor *= gl_Color;
        }";
    }
}
