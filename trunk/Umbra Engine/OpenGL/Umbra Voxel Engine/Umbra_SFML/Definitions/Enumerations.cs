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

namespace Umbra.Definitions
{
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

    public enum BlockVisibility
    {
        Invisible,
        Translucent,
        Opaque
    }

    public enum GraphingVariable
    {
        PlayerPositionX,
        PlayerPositionY,
        PlayerPositionZ,
        PlayerVelocityX,
        PlayerVelocityY,
        PlayerVelocityZ,
        PlayerAccelerationX,
        PlayerAccelerationY,
        PlayerAccelerationZ,
        RAM
    }

    public enum ScalingMode
    {
        NoScaling,
        ScaleToFit,
        FreeMove,
        FreeZoom,
        FullFree
    }

    // Operators:

    public struct ConsoleMessage
    {
        public string Message;
        public double Timestamp;
        public SpriteString SpriteString;

        public ConsoleMessage(string message, double timestamp, Color color)
        {
            Message = message;
            Timestamp = timestamp;

            SpriteString = new SpriteString(Console.Font, message, color);
        }
    }

    public class Direction
    {

        private enum Dir : byte
        {
            Left,
            Right,
            Up,
            Down,
            Forward,
            Backward
        }

        public byte Value
        {
            get
            {
                return (byte)DirectionEnum;
            }
        }

        Dir DirectionEnum;

        private Direction(Dir direction)
        {
            DirectionEnum = direction;
        }

        static public Direction Backward { get { return new Direction(Dir.Backward); } }
        static public Direction Forward { get { return new Direction(Dir.Forward); } }
        static public Direction Left { get { return new Direction(Dir.Left); } }
        static public Direction Right { get { return new Direction(Dir.Right); } }
        static public Direction Up { get { return new Direction(Dir.Up); } }
        static public Direction Down { get { return new Direction(Dir.Down); } }

        public Direction Opposite()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Direction.Forward;
                case Dir.Forward: return Direction.Backward;
                case Dir.Left: return Direction.Right;
                case Dir.Right: return Direction.Left;
                case Dir.Up: return Direction.Down;
                case Dir.Down: return Direction.Up;
                default: throw new Exception("This shouldn't happen.");
            }
        }

        public Vector3 GetVector3()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Vector3.UnitZ;
                case Dir.Forward: return -Vector3.UnitZ;
                case Dir.Right: return Vector3.UnitX;
                case Dir.Left: return -Vector3.UnitX;
                case Dir.Up: return Vector3.UnitY;
                case Dir.Down: return -Vector3.UnitY;
                default: throw new Exception("This shouldn't happen.");
            }
        }

        public byte GetFaceShade()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Constants.Graphics.Lighting.BackwardShade;
                case Dir.Forward: return Constants.Graphics.Lighting.ForwardShade;
                case Dir.Left: return Constants.Graphics.Lighting.LeftShade;
                case Dir.Right: return Constants.Graphics.Lighting.RightShade;
                case Dir.Up: return Constants.Graphics.Lighting.UpShade;
                case Dir.Down: return Constants.Graphics.Lighting.DownShade;
                default: return 0;
            }
        }

        static public bool operator ==(Direction dir1, Direction dir2)
        {
            return (dir1.DirectionEnum == dir2.DirectionEnum);
        }

        static public bool operator !=(Direction dir1, Direction dir2)
        {
            return (dir1.DirectionEnum != dir2.DirectionEnum);
        }
    }
}
