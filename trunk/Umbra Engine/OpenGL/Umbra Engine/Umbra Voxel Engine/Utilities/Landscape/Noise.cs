using System;
using System.Linq;
using System.Text;
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

namespace Umbra.Utilities.Landscape
{
    static public class Noise
    {
        static public float GetPerlin(int x, int y, int seed)
        {
            return GetPerlin(x, y, Constants.Landscape.PerlinOctaves, seed);
        }

        static public float GetPerlin(int x, int y, int depth, int seed)
        {
            float returnValue = 0.0F;

            for (int level = 1; level <= depth; level++)
            {
                returnValue += GetBilinearlyInterpolated(x, y, (int)Math.Pow(2, level + 1), seed + level) * (float)(Math.Pow(2, level) / Math.Pow(2, depth + 1));
            }

            return returnValue;
        }

        static public float GetByValues(int a, int b, int c)
        {
            return Math.Abs((float)(((
                ((float)a + 0.1F) *
                ((float)b + 0.1F) *
                ((float)c + 0.1F) +
                ((float)b + 0.1F) *
                ((float)c + 0.1F))/*.ToString().GetHashCode()*/) % 10000)) / 10000.0F;
        }

        static private float GetBilinearlyInterpolated(int x, int y, int squareSize, int seed)
        {
            int xInSquare = Mathematics.AbsModulo(x, squareSize);
            int yInSquare = Mathematics.AbsModulo(y, squareSize);

            float[,] data = new float[2, 2];

            data[0, 0] = GetByValues(x - xInSquare, y - yInSquare, seed);
            data[0, 1] = GetByValues(x - xInSquare, y - yInSquare + squareSize, seed);
            data[1, 0] = GetByValues(x - xInSquare + squareSize, y - yInSquare, seed);
            data[1, 1] = GetByValues(x - xInSquare + squareSize, y - yInSquare + squareSize, seed);

            return Interpolation.Bilinear(data, (float)Math.Abs(xInSquare) / (float)(squareSize), (float)Math.Abs(yInSquare) / (float)(squareSize));

        }

        static private float GetBicubiclyInterpolated(int x, int y, int squareSize, int seed)
        {
            int xInSquare = Mathematics.AbsModulo(y, squareSize);
            int yInSquare = Mathematics.AbsModulo(y, squareSize);

            float[,] data = new float[4, 4];

            data[0, 0] = GetByValues(x - xInSquare - squareSize, y - yInSquare - squareSize, seed);
            data[0, 1] = GetByValues(x - xInSquare - squareSize, y - yInSquare, seed);
            data[0, 2] = GetByValues(x - xInSquare - squareSize, y - yInSquare + squareSize, seed);
            data[0, 3] = GetByValues(x - xInSquare - squareSize, y - yInSquare + squareSize * 2, seed);

            data[1, 0] = GetByValues(x - xInSquare, y - yInSquare - squareSize, seed);
            data[1, 1] = GetByValues(x - xInSquare, y - yInSquare, seed);
            data[1, 2] = GetByValues(x - xInSquare, y - yInSquare + squareSize, seed);
            data[1, 3] = GetByValues(x - xInSquare, y + y - yInSquare + squareSize * 2, seed);

            data[2, 0] = GetByValues(x - xInSquare + squareSize, y - yInSquare - squareSize, seed);
            data[2, 1] = GetByValues(x - xInSquare + squareSize, y - yInSquare, seed);
            data[2, 2] = GetByValues(x - xInSquare + squareSize, y - yInSquare + squareSize, seed);
            data[2, 3] = GetByValues(x - xInSquare + squareSize, y - yInSquare + squareSize * 2, seed);

            data[3, 0] = GetByValues(x - xInSquare + squareSize * 2, y - yInSquare - squareSize, seed);
            data[3, 1] = GetByValues(x - xInSquare + squareSize * 2, y - yInSquare, seed);
            data[3, 2] = GetByValues(x - xInSquare + squareSize * 2, y - yInSquare + squareSize, seed);
            data[3, 3] = GetByValues(x - xInSquare + squareSize * 2, y - yInSquare + squareSize * 2, seed);

            Interpolation.UpdateBicubicCoefficients(data);

            return Interpolation.Bicubic((float)Math.Abs(xInSquare) / (float)(squareSize), (float)Math.Abs(yInSquare) / (float)(squareSize));

        }
    }
}
