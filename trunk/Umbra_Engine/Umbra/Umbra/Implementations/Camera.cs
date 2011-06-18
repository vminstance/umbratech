using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Console = Umbra.Implementations.Console;

namespace Umbra.Implementations
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation;
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
            Direction = MathHelper.WrapAngle(direction);
            Pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
            Rotation = Quaternion.Identity;
        }

        public void Update()
        {
            if (Constants.Main.IsActive && !Console.IsActive)
            {
                Direction -= MathHelper.WrapAngle((float)(Constants.Input.MouseCurrentState.X - Constants.ScreenResolution.X / 2) / Constants.MouseSensitivityInv);
                Pitch = MathHelper.Clamp(Pitch - (float)(Constants.Input.MouseCurrentState.Y - Constants.ScreenResolution.Y / 2) / Constants.MouseSensitivityInv, -MathHelper.PiOver2, MathHelper.PiOver2);
                Mouse.SetPosition((int)Constants.ScreenResolution.X / 2, (int)Constants.ScreenResolution.Y / 2);
            }

            Rotation = Quaternion.Lerp(Rotation, Quaternion.CreateFromYawPitchRoll(Direction, Pitch, 0), Constants.SmoothCameraRespons);
        }

        public void SetRotation(float direction, float pitch)
        {
            Direction = MathHelper.WrapAngle(direction);
            Pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void Rotate(float direction, float pitch)
        {
            Direction = MathHelper.WrapAngle(Direction + direction);
            Pitch = MathHelper.Clamp(Pitch + pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public Matrix GetProjection()
        {
            return Matrix.CreatePerspectiveFieldOfView(Constants.FieldOfView, Constants.AspectRatio, Constants.CameraNearPlane, Constants.CameraFarPlane);
        }

        public Matrix GetView()
        {
            if (Constants.SmoothCameraEnabled)
            {
                return Matrix.CreateLookAt(
                    /*cam pos*/     Position,
                    /*Look pos*/    Position + Vector3.Transform(Vector3.Forward, Rotation),
                    /*Up dir*/      Vector3.Transform(Vector3.Up, Rotation));
            }
            else
            {
                return Matrix.CreateLookAt(
                    /*cam pos*/     Position,
                    /*Look pos*/    Position + Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Direction)),
                    /*Up dir*/      Vector3.Transform(Vector3.Up, Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Direction)));
            }
        }
    }
}
