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
using Umbra_Engine;

namespace Umbra_Engine.Engines
{
    class InputEngine : IComponent
    {
        Game Game;
        GameWindow Main;
        IGraphicsContext Context;

        public InputEngine(Game game)
        {
            Game = game;
            Main = Game.Main;
            Context = Game.Context;

            Main.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
            Main.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonUp);
            Main.Mouse.Move += new EventHandler<MouseMoveEventArgs>(Mouse_Move);

            Main.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyDown);
            Main.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyUp);
        }

        void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
                if (e.Key == Key.W)
                {
                    Game.gfxEng.Camera.StopMove(Vector3.UnitZ);
                }
                else if (e.Key == Key.S)
                {
                    Game.gfxEng.Camera.StopMove(-Vector3.UnitZ);
                }
                else if (e.Key == Key.A)
                {
                    Game.gfxEng.Camera.StopMove(Vector3.UnitX);
                }
                else if (e.Key == Key.D)
                {
                    Game.gfxEng.Camera.StopMove(-Vector3.UnitX);
                }
                else if (e.Key == Key.Space)
                {
                    Game.gfxEng.Camera.StopMove(Vector3.UnitY);
                }
                else if (e.Key == Key.ControlLeft)
                {
                    Game.gfxEng.Camera.StopMove(-Vector3.UnitY);
                }
        }

        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                Game.gfxEng.Camera.StartMove(Vector3.UnitZ);
            }
            else if (e.Key == Key.S)
            {
                Game.gfxEng.Camera.StartMove(-Vector3.UnitZ);
            }
            else if (e.Key == Key.A)
            {
                Game.gfxEng.Camera.StartMove(Vector3.UnitX);
            }
            else if (e.Key == Key.D)
            {
                Game.gfxEng.Camera.StartMove(-Vector3.UnitX);
            }
            else if (e.Key == Key.Space)
            {
                Game.gfxEng.Camera.StartMove(Vector3.UnitY);
            }
            else if (e.Key == Key.ControlLeft)
            {
                Game.gfxEng.Camera.StartMove(-Vector3.UnitY);
            }
        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            Game.gfxEng.Camera.MouseMove(e.Position, e.XDelta, e.YDelta);
        }

        void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
        }


        public void Update()
        {
        }

        public void Render()
        {
        }
    }
}
