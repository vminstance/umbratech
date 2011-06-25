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

namespace Umbra.Structures
{
    abstract public class Window
    {
        public Rectangle Frame;
        Rectangle DragHandle;
        Rectangle ResizeHandle;
        protected bool Dragable;
        protected bool Resizeable;
        string Title;

        bool DragHold;
        bool ResizeHold;

        public Window(string title, Rectangle frame)
        {
            Title = title;
            ResizeHandle = new Rectangle(frame.X + frame.Width - 7, frame.Y + frame.Height - 7, 11, 11);
            DragHandle = new Rectangle(frame.X, frame.Y, frame.Width, 20);

            DragHold = false;
            ResizeHold = false;
        }

        virtual public void Update(GameTime gameTime)
        {
            MouseState mouse = Constants.Engine_Input.MouseCurrentState;

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (DragHandle.Contains(new Point(mouse.X, mouse.Y)))
                {
                    DragHold = true;
                }
            }
            else
            {
                DragHold = false;
            }

            if (DragHold)
            {
                Frame.X = mouse.X - 5;
                Frame.Y = mouse.Y - 5;
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drag = DragHandle;
            drag.Inflate(-2, -2);
            Rectangle resize = ResizeHandle;
            resize.Inflate(-4, -4);

            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, Frame, new Color(20, 20, 20, 100));
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, resize, new Color(20, 20, 20, 100));
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, drag, new Color(20, 20, 20, 100));
            spriteBatch.DrawString(Constants.Engine_Content.DefaultFont, Title, new Vector2(Frame.X + (int)((Frame.Width - Constants.Engine_Content.DefaultFont.MeasureString(Title).X) / 2), Frame.Y + 12 - Constants.Engine_Content.DefaultFont.MeasureString(Title).Y / 2), Color.White);
        }
    }
}
