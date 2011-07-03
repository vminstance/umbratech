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
    abstract public class Window : WindowComponent
    {
        protected bool Dragable;
        protected bool Resizeable;
        string Title;

        public Window(string title, Rectangle frame)
            : base()
        {
            Title = title;
            Handles.Add("resize", new Handle(frame.X + frame.Width - 5, frame.Y + frame.Height - 5, 7, 7));
            Handles.Add("drag", new Handle(frame.X, frame.Y, frame.Width, 20));

            Event_OnPaint += OnPaint;
            Event_OnUpdate += OnUpdate;
        }

        virtual public void OnUpdate(GameTime gameTime, object[] args)
        {
            foreach (KeyValuePair<string, Handle> handle in Handles)
            {
                handle.Value.Update();
            }
        }

        virtual public void OnPaint(GameTime gameTime, object[] args)
        {
            SpriteBatch spriteBatch = (SpriteBatch)args[0];

            Rectangle drag = Handles["drag"].Bound;
            drag.Inflate(-2, -2);
            Rectangle resize = Handles["resize"].Bound;
            resize.Inflate(-4, -4);

            if (Handles["resize"].MouseOver || Handles["resize"].MouseHold)
            {
                resize.Inflate(4, 4);
            }

            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, Frame, new Color(20, 20, 20, 100));
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, resize, new Color(20, 20, 20, 100));
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, drag, new Color(20, 20, 20, 100));
            spriteBatch.DrawString(Constants.Engine_Content.DefaultFont, Title, new Vector2(Frame.X + (int)((Frame.Width - Constants.Engine_Content.DefaultFont.MeasureString(Title).X) / 2), Frame.Y + 12 - Constants.Engine_Content.DefaultFont.MeasureString(Title).Y / 2), Color.White);
        }
    }
}
