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
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures
{
    public class Camera
    {
        public Vector3d Position { get; set; }
        public Quaterniond Rotation
        {
            get
            {
                return QuaternionCreateFromYPR(Direction, Pitch, 0.0F);
            }
        }

        public double Direction { get; private set; }
        public double Pitch { get; private set; }

        public Camera(Vector3d position)
        {
            SetupCamera(position, 0.0F, 0.0F);
        }

        public Camera(Vector3d position, double direction, double pitch)
        {
            SetupCamera(position, direction, pitch);
        }

        void SetupCamera(Vector3d position, double direction, double pitch)
        {
            Position = position;
            Direction = Mathematics.WrapAngleRadians(direction);
            Pitch = Mathematics.Clamp(pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void UpdateMouse(Point delta)
        {
            Direction = Mathematics.WrapAngleRadians(Direction - (double)delta.X / Constants.Controls.MouseSensitivityInv);
            Pitch = Mathematics.Clamp(Pitch - (double)delta.Y / Constants.Controls.MouseSensitivityInv, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void SetRotation(double direction, double pitch)
        {
            Direction = Mathematics.WrapAngleRadians(direction);
            Pitch = Mathematics.Clamp(pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void Rotate(double direction, double pitch)
        {
            Direction = Mathematics.WrapAngleRadians(Direction + direction);
            Pitch = Mathematics.Clamp(Pitch + pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public Matrix4 GetProjection()
        {
            return Matrix4.CreatePerspectiveFieldOfView(Constants.Graphics.FieldOfView, Constants.Graphics.AspectRatio, Constants.Graphics.CameraNearPlane, Constants.Graphics.CameraFarPlane);
        }

        public Matrix4 GetView()
        {
            return Matrix4.LookAt(
                /*cam pos*/     (Vector3)Position,
                /*Look pos*/    (Vector3)Position + (Vector3)Vector3d.Transform(-Vector3d.UnitZ, QuaternionCreateFromYPR(Direction, Pitch, 0.0)),
                /*Up dir*/      (Vector3)Vector3d.Transform(Vector3d.UnitY, QuaternionCreateFromYPR(Direction, Pitch, 0.0)));
        }

        private Quaterniond QuaternionCreateFromYPR(double yaw, double pitch, double roll)
        {
            return Quaterniond.FromAxisAngle(Vector3d.UnitY, yaw) * Quaterniond.FromAxisAngle(Vector3d.UnitX, pitch) * Quaterniond.FromAxisAngle(Vector3d.UnitZ, roll);
        }
    }
}
