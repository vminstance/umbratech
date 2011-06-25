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
        public Rectangle Frame { get; protected set; }
        protected bool Dragable;
        string Title;

        public Window(string title)
        {
            Title = title;
        }

        abstract public void Update(GameTime gameTime);
        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, Frame, new Color(20, 20, 20, 100));
            spriteBatch.DrawString(Constants.Engine_Content.DefaultFont, Title, new Vector2(Frame.X + (int)((Frame.Width - Constants.Engine_Content.DefaultFont.MeasureString(Title).X) / 2), Frame.Y + 10 - Constants.Engine_Content.DefaultFont.MeasureString(Title).Y / 2), Color.White);
        }
    }
}
