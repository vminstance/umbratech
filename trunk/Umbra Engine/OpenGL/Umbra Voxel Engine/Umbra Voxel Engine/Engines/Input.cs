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

namespace Umbra.Engines
{
    public class Input : Engine
    {

        KeyboardDevice Keyboard { get; set; }
        MouseDevice Mouse { get; set; }
        Point MouseDelta { get; set; }

        public override void Initialize(EventArgs e)
        {
            Keyboard = Main.Keyboard;
            Mouse = Main.Mouse;

            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyboardEvent);
            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButton);

            CenterMouse();

            base.Initialize(e);
        }

        public void SetMouseShow(bool show)
        {
            if (show)
            {
                System.Windows.Forms.Cursor.Show();
            }
            else
            {
                System.Windows.Forms.Cursor.Hide();
            }
        }

        public void CenterMouse()
        {
            System.Windows.Forms.Cursor.Position = Main.PointToScreen(new Point((Main.ClientSize.Width / 2), (Main.ClientSize.Height / 2)));
        }

        void MouseMove()
        {
            if (Variables.Game.IsActive)
            {
                MouseDelta = new Point(Mouse.X - (Main.ClientSize.Width / 2), Mouse.Y - (Main.ClientSize.Height / 2));
                CenterMouse();

                Constants.Engine_Physics.Player.FirstPersonCamera.UpdateMouse(MouseDelta);
            }
        }

        void MouseButton(object sender, MouseButtonEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }
        }

        void KeyboardEvent(object sender, KeyboardKeyEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }

            if (Variables.Game.IsActive)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        {
                            Console.Execute("exit");
                        }
                        break;

                    case Key.T:
                        {
                            Console.Toggle();
                        }
                        break;

                    case Key.Q:
                        {
                            Console.Execute("noclip");
                        }
                        break;

                    case Key.F:
                        {
                            Console.Execute("flashlight");
                        }
                        break;
                }
            }
            else
            {
                Console.Input(e, Keyboard);
            }
        }


        public override void Update(FrameEventArgs e)
        {


            if (!Variables.Game.IsActive)
            {
            }
            else if (Variables.Game.IsActive && Main.Focused)
            {
                MouseMove();
                Constants.Engine_Physics.Player.UpdateKeyboard(Keyboard);
                Constants.Engine_Physics.Player.UpdateMouse(Mouse);
            }

            base.Update(e);
        }
    }
}
