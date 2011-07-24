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

namespace Umbra.Structures
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation
        {
            get
            {
                return QuaternionCreateFromYPR(Direction, Pitch, 0.0F);
            }
        }

        public float Direction { get; private set; }
        public float Pitch { get; private set; }

        public Camera(Vector3 position)
        {
            SetupCamera(position, 0.0F, 0.0F);
        }

        public Camera(Vector3 position, float direction, float pitch)
        {
            SetupCamera(position, direction, pitch);
        }

        void SetupCamera(Vector3 position, float direction, float pitch)
        {
            Position = position;
            Direction = Mathematics.WrapAngleRadians(direction);
            Pitch = Mathematics.Clamp(pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void UpdateMouse(Point delta)
        {
            Direction = Mathematics.WrapAngleRadians(Direction - (float)delta.X / Constants.Controls.MouseSensitivityInv);
            Pitch = Mathematics.Clamp(Pitch - (float)delta.Y / Constants.Controls.MouseSensitivityInv, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void SetRotation(float direction, float pitch)
        {
            Direction = Mathematics.WrapAngleRadians(direction);
            Pitch = Mathematics.Clamp(pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void Rotate(float direction, float pitch)
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
                /*cam pos*/     Position,
                /*Look pos*/    Position + Vector3.Transform(-Vector3.UnitZ, QuaternionCreateFromYPR(Direction, Pitch, 0.0F)),
                /*Up dir*/      Vector3.Transform(Vector3.UnitY, QuaternionCreateFromYPR(Direction, Pitch, 0.0F)));
        }

        private Quaternion QuaternionCreateFromYPR(float yaw, float pitch, float roll)
        {
            return Quaternion.FromAxisAngle(Vector3.UnitY, yaw) * Quaternion.FromAxisAngle(Vector3.UnitX, pitch) * Quaternion.FromAxisAngle(Vector3.UnitZ, roll);
        }
    }
}
