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
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra.Structures.Forms
{
    public delegate void EventFunction(GameTime gameTime, object[] args);

    static public class WindowManager
    {
        static List<Window> Windows;

        static public void Initialize()
        {
            Windows = new List<Window>();
        }

        static public void AddWindow(Window window)
        {
            Windows.Add(window);
        }

        static public void CloseWindow(Window window)
        {
            Windows.Remove(window);
        }

        static public void Update(GameTime gameTime)
        {
            MouseState mouse = Constants.Engine_Input.MouseCurrentState;
            Point mousePos = new Point(mouse.X, mouse.Y);

            foreach (Window window in Windows)
            {
                window.Event_OnUpdate.Invoke(gameTime, new object[]{});
            }
        }

        static public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Window window in Windows)
            {
                window.Event_OnUpdate.Invoke(gameTime, new object[] { spriteBatch });
            }
        }
    }
}
