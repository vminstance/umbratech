using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbra.Structures.Graphics
{
    static public class VertexShader
    {

        // READ: http://www.opengl.org/wiki/GLSL_:_common_mistakes

        static public string Shader
        {
            get
            {
                return @"

                in uint data;

                uniform mat4 projection_mat;
                uniform mat4 view_mat;
                uniform mat4 world_mat;

                varying float diffuse;
 
                void main() 
                {

                    uint PosX   = ( data % 32 );
                    uint PosY   = ( ( data / 32 ) % 32 );
                    uint PosZ   = ( ( ( data / 32 ) / 32 ) % 32 );
                    uint normal = ( ( ( ( data / 32 ) / 32 ) / 32 ) % 6 );
                    uint corner = ( ( ( ( ( data / 32 ) / 32 ) / 32 ) / 6 ) % 4 );
                    uint type   = ( ( ( ( ( ( data / 32 ) / 32 ) / 32 ) / 6 ) / 4 ) % 256 );
                    uint shade  = ( ( ( ( ( ( ( data / 32 ) / 32 ) / 32 ) / 6 ) / 4 ) / 256 ) % 21);

                    vec4 position;

                    " + SetPolygonCoords + @"

                    position += vec4(PosX, PosY, PosZ, 0.0);

                    gl_Position = projection_mat * view_mat * world_mat * position;

                    gl_FrontColor = vec4(shade, shade, shade, 20.0) / 20.0; 
                    gl_TexCoord[0] = vec4(type % 16, type >> 4, 1.0, 1.0);
                    
                    " + SetTextureCoords + @"
                    
                    " + SetDiffuseLighting + @"

                    gl_TexCoord[0] /= 16.0;
                }";
            }
        }


        static public readonly string SetPolygonCoords = @"

        switch (normal)
        {
            case 0:
            {
                switch(corner)
                {
                    case 0:
                    {
                        position = vec4(1.0, 0.0, 0.0, 1.0);
                        break;
                    }
                    case 1:
                    {
                        position = vec4(1.0, 1.0, 0.0, 1.0);
                        break;
                    }
                    case 2:
                    {
                        position = vec4(1.0, 1.0, 1.0, 1.0);
                        break;
                    }
                    case 3:
                    {
                        position = vec4(1.0, 0.0, 1.0, 1.0);
                        break;
                    }
                }
                break;
            }
            case 1:
            {
                switch(corner)
                {
                    case 0:
                    {
                        position = vec4(0.0, 0.0, 1.0, 1.0);
                        break;
                    }
                    case 1:
                    {
                        position = vec4(0.0, 1.0, 1.0, 1.0);
                        break;
                    }
                    case 2:
                    {
                        position = vec4(0.0, 1.0, 0.0, 1.0);
                        break;
                    }
                    case 3:
                    {
                        position = vec4(0.0, 0.0, 0.0, 1.0);
                        break;
                    }
                }
                break;
            }
            case 2:
            {
                switch(corner)
                {
                    case 0:
                    {
                        position = vec4(1.0, 1.0, 1.0, 1.0);
                        break;
                    }
                    case 1:
                    {
                        position = vec4(1.0, 1.0, 0.0, 1.0);
                        break;
                    }
                    case 2:
                    {
                        position = vec4(0.0, 1.0, 0.0, 1.0);
                        break;
                    }
                    case 3:
                    {
                        position = vec4(0.0, 1.0, 1.0, 1.0);
                        break;
                    }
                }
                break;
            }
            case 3:
            {
                switch(corner)
                {
                    case 0:
                    {
                        position = vec4(0.0, 0.0, 1.0, 1.0);
                        break;
                    }
                    case 1:
                    {
                        position = vec4(0.0, 0.0, 0.0, 1.0);
                        break;
                    }
                    case 2:
                    {
                        position = vec4(1.0, 0.0, 0.0, 1.0);
                        break;
                    }
                    case 3:
                    {
                        position = vec4(1.0, 0.0, 1.0, 1.0);
                        break;
                    }
                }
                break;
            }
            case 4:
            {
                switch(corner)
                {
                    case 0:
                    {
                        position = vec4(1.0, 0.0, 1.0, 1.0);
                        break;
                    }
                    case 1:
                    {
                        position = vec4(1.0, 1.0, 1.0, 1.0);
                        break;
                    }
                    case 2:
                    {
                        position = vec4(0.0, 1.0, 1.0, 1.0);
                        break;
                    }
                    case 3:
                    {
                        position = vec4(0.0, 0.0, 1.0, 1.0);
                        break;
                    }
                }
                break;
            }
            case 5:
            {
                switch(corner)
                {
                    case 0:
                    {
                        position = vec4(0.0, 0.0, 0.0, 1.0);
                        break;
                    }
                    case 1:
                    {
                        position = vec4(0.0, 1.0, 0.0, 1.0);
                        break;
                    }
                    case 2:
                    {
                        position = vec4(1.0, 1.0, 0.0, 1.0);
                        break;
                    }
                    case 3:
                    {
                        position = vec4(1.0, 0.0, 0.0, 1.0);
                        break;
                    }
                }
                break;
            }
        }";


        static public readonly string SetTextureCoords = @"

        switch(corner)
        {
            case 0:
            {
                gl_TexCoord[0].x += 0.001;
                gl_TexCoord[0].y += 0.999;
                break;
            }
            case 1:
            {
                gl_TexCoord[0].xy += 0.001;
                break;
            }
            case 2:
            {
                gl_TexCoord[0].x += 0.999;
                gl_TexCoord[0].y += 0.001;
                break;
            }
            case 3:
            {
                gl_TexCoord[0].xy += 0.999;
                break;
            }
        }";


        static public readonly string SetDiffuseLighting = @"

        switch(normal)
        {
            case 0:
            {
                diffuse = 0.7;
                break;
            }
            case 1:
            {
                diffuse = 0.7;
                break;
            }
            case 2:
            {
                diffuse = 1.0;
                break;
            }
            case 3:
            {
                diffuse = 0.4;
                break;
            }
            case 4:
            {
                diffuse = 0.5;
                break;
            }
            case 5:
            {
                diffuse = 0.5;
                break;
            }
        }";
    }
}
