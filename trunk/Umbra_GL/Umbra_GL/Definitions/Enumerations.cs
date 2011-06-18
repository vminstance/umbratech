using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Internal;
using Vector3 = Umbra.Structures.Internal.Vector3;
using Console = Umbra.Implementations.Console;

namespace Umbra.Definitions
{
    public enum Direction : byte
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Backward
    }

    public enum TimeOfDay : byte
    {
        SunRise,
        Day,
        SunSet,
        Night
    }

    public enum FaceValidation
    {
        NoFaces,
        ThisFace,
        OtherFace,
        BothFaces,
        Indeterminate
    }

    public enum ConsoleState : byte
    {
        Open,
        Closed,
        FadeIn,
        FadeOut
    }

    public struct ConsoleMessage
    {
        public string Message;
        public double Timestamp;
        public Color Color;

        public ConsoleMessage(string message, double timestamp, Color color)
        {
            Message = message;
            Timestamp = timestamp;
            Color = color;
        }
    }

    static public class DirOperators
    {
        static public Direction Opposite(Direction dir)
        {
            switch (dir)
            {
            case Direction.Backward: return Direction.Forward;
            case Direction.Forward: return Direction.Backward;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            default: throw new Exception("This shouldn't happen.");
            }
        }

        static public Vector3 GetVector3(Direction dir)
        {
            switch (dir)
            {
            case Direction.Backward: return Vector3.Backward;
            case Direction.Forward: return Vector3.Forward;
            case Direction.Left: return Vector3.Left;
            case Direction.Right: return Vector3.Right;
            case Direction.Up: return Vector3.Up;
            case Direction.Down: return Vector3.Down;
            default: throw new Exception("This shouldn't happen.");
            }
        }

        static public byte GetFaceShade(Direction dir)
        {
            switch (dir)
            {
            case Direction.Backward: return Constants.TextureBackwardShade;
            case Direction.Forward: return Constants.TextureForwardShade;
            case Direction.Left: return Constants.TextureLeftShade;
            case Direction.Right: return Constants.TextureRightShade;
            case Direction.Up: return Constants.TextureUpShade;
            case Direction.Down: return Constants.TextureDownShade;
            default: return 0;
            }
        }
    }

    public enum BlockVisibility
    {
        Invisible,
        Translucent,
        Opaque
    }
}
