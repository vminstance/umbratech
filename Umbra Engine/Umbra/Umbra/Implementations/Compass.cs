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
    static public class Compass
    {
        static public void Draw()
        {
            int degrees = 360 - (int)(MathHelper.ToDegrees(MathHelper.WrapAngle(Constants.Physics.Player.FirstPersonCamera.Direction) + (float)Math.PI));

            Rectangle mainRectangle = new Rectangle();
            mainRectangle.Y = (int)Constants.CompassFrameSize.Y;
            mainRectangle.Height = (int)Constants.CompassStripWindowSize.Y;
            mainRectangle.X = (int)Math.Max(0, degrees - Constants.CompassStripWindowSize.X / 2);
            mainRectangle.Width = (int)Constants.CompassStripWindowSize.X;
            mainRectangle.Width += (int)Math.Min(0, degrees - Constants.CompassStripWindowSize.X / 2);
            mainRectangle.Width -= (int)Math.Max(degrees + Constants.CompassStripWindowSize.X / 2, Constants.Content.CompassTextures.Width) - Constants.Content.CompassTextures.Width;



            // Draw frame
            Constants.Overlay.SpriteBatch.Draw(
                Constants.Content.CompassTextures,
                Constants.CompassScreenPosition,
                new Rectangle(0, 0, (int)Constants.CompassFrameSize.X, (int)Constants.CompassFrameSize.Y),
                Color.White);

            // Draw strip 1
            Constants.Overlay.SpriteBatch.Draw(
                Constants.Content.CompassTextures,
                Constants.CompassScreenPosition + Constants.CompassStripOffset + Vector2.UnitX * (int)Math.Max(0, (Constants.CompassStripWindowSize.X / 2 - degrees)),
                mainRectangle,
                Color.White);

            // Draw filler strips
            if (degrees < Constants.CompassStripWindowSize.X / 2)
            {
                Rectangle fillerRectangle = new Rectangle();
                fillerRectangle.Y = (int)Constants.CompassFrameSize.Y;
                fillerRectangle.Height = (int)Constants.CompassStripWindowSize.Y;
                fillerRectangle.X = Constants.Content.CompassTextures.Width - (int)(Constants.CompassStripWindowSize.X / 2 - degrees);
                fillerRectangle.Width = (int)(Constants.CompassStripWindowSize.X / 2) - degrees;

                Constants.Overlay.SpriteBatch.Draw(
                    Constants.Content.CompassTextures,
                    Constants.CompassScreenPosition + Constants.CompassStripOffset,
                    fillerRectangle,
                    Color.White);
            }
            if (degrees > Constants.Content.CompassTextures.Width - Constants.CompassStripWindowSize.X / 2)
            {
                Rectangle fillerRectangle = new Rectangle();
                fillerRectangle.Y = (int)Constants.CompassFrameSize.Y;
                fillerRectangle.Height = (int)Constants.CompassStripWindowSize.Y;
                fillerRectangle.X = 0;
                fillerRectangle.Width = (int)(Constants.CompassStripWindowSize.X / 2) - (Constants.Content.CompassTextures.Width - degrees);

                Constants.Overlay.SpriteBatch.Draw(
                    Constants.Content.CompassTextures,
                    Constants.CompassScreenPosition + Constants.CompassStripOffset + Vector2.UnitX * (Constants.CompassStripWindowSize.X + Constants.Content.CompassTextures.Width - degrees - Constants.CompassStripWindowSize.X / 2),
                    fillerRectangle,
                    Color.White);
            }
        }
    }
}
