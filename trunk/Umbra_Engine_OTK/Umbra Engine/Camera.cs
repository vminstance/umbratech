using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System.Drawing;
using Umbra_Engine.Engines;

namespace Umbra_Engine
{
    class Camera : IComponent
    {
        Vector3 Position;
        float Direction;
        float Pitch;

        Game Game;
        GameWindow Main;
        IGraphicsContext Context;

        Vector3 Velocity;

        public Camera(Vector3 newPos, float newDirection, float newPitch, Game game)
        {
            Position = newPos;
            Direction = newDirection;
            Pitch = newPitch;

            Game = game;
            Main = Game.Main;
            Context = Game.Context;
            Velocity = Vector3.Zero;
        }

        public void Update()
        {
            Position += Vector3.Transform(Velocity, Matrix4.CreateRotationX(Pitch) * Matrix4.CreateRotationY(Direction));
        }

        public void Render()
        {
        }


        public void MouseMove(Point Position, int dX, int dY)
        {
            Direction += (float)-dX/ 50.0F;
            Pitch = Clamp((float)dY / 50.0F + Pitch, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public float Clamp(float val, float min, float max)
        {
            if( val < min)
                return min;

            if (val > max)
                return max;

            return val;
        }

        public void StartMove(Vector3 Dir)
        {
            Velocity += Dir / 4;
        }

        public void StopMove(Vector3 Dir)
        {
            Velocity += -Dir / 4;
        }

        public Matrix4 GetProjection(float aspectRatio)
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0F), aspectRatio, 1.0F, 1000.0F);
        }

        public Matrix4 GetView()
        {
            return Matrix4.LookAt(
                /*cam pos*/     Position,
                /*Look pos*/    Position + Vector3.Transform(Vector3.UnitZ, Matrix4.CreateRotationX(Pitch) * Matrix4.CreateRotationY(Direction)),
                /*Up dir*/      Vector3.Transform(Vector3.UnitY, Matrix4.CreateRotationX(Pitch) * Matrix4.CreateRotationY(Direction)));
        }
    }
}