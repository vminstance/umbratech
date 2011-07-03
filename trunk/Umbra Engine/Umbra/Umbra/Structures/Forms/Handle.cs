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
    public class Handle
    {
        public Rectangle Bound;
        Point? Hold;
        public bool Enabled;

        public bool MouseOver
        {
            get
            {
                return Bound.Contains(Constants.Engine_Input.MouseCurrentState.X, Constants.Engine_Input.MouseCurrentState.Y);
            }
        }

        public bool MouseHold
        {
            get
            {
                return Hold.HasValue;
            }
        }

        public Handle(int x, int y, int width, int height)
        {
            Bound = new Rectangle(x, y, width, height);
            Hold = null;
        }

        public void Update()
        {
            if (!Enabled)
            {
                Hold = null;
                return;
            }

            if (Hold.HasValue)
            {
                if (Constants.Engine_Input.MouseCurrentState.LeftButton == ButtonState.Pressed)
                {
                    Bound.X = Constants.Engine_Input.MouseCurrentState.X + Hold.Value.X;
                    Bound.Y = Constants.Engine_Input.MouseCurrentState.Y + Hold.Value.Y;
                }
                else
                {
                    Hold = null;
                }
            }
            else if (Constants.Engine_Input.MouseCurrentState.LeftButton == ButtonState.Pressed)
            {
                if (Bound.Contains(Constants.Engine_Input.MouseCurrentState.X, Constants.Engine_Input.MouseCurrentState.Y))
                {
                    Hold = new Point(Constants.Engine_Input.MouseCurrentState.X - Bound.X, Constants.Engine_Input.MouseCurrentState.Y - Bound.Y);
                }
            }
        }
    }
}
