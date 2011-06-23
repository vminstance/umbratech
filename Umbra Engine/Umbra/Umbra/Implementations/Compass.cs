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

namespace Umbra.Implementations
{
    static public class Compass
    {
        static public void Draw()
        {
            int degrees = 360 - (int)(MathHelper.ToDegrees(MathHelper.WrapAngle(Constants.Engine_Physics.Player.FirstPersonCamera.Direction) + (float)Math.PI));

            Rectangle mainRectangle = new Rectangle();
            mainRectangle.Y = (int)Constants.Overlay.Compass.FrameSize.Y;
            mainRectangle.Height = (int)Constants.Overlay.Compass.StripWindowSize.Y;
            mainRectangle.X = (int)Math.Max(0, degrees - Constants.Overlay.Compass.StripWindowSize.X / 2);
            mainRectangle.Width = (int)Constants.Overlay.Compass.StripWindowSize.X;
            mainRectangle.Width += (int)Math.Min(0, degrees - Constants.Overlay.Compass.StripWindowSize.X / 2);
            mainRectangle.Width -= (int)Math.Max(degrees + Constants.Overlay.Compass.StripWindowSize.X / 2, Constants.Engine_Content.CompassTextures.Width) - Constants.Engine_Content.CompassTextures.Width;



            // Draw frame
            Constants.Engine_Overlay.SpriteBatch.Draw(
                Constants.Engine_Content.CompassTextures,
                Constants.Overlay.Compass.ScreenPosition,
                new Rectangle(0, 0, (int)Constants.Overlay.Compass.FrameSize.X, (int)Constants.Overlay.Compass.FrameSize.Y),
                Color.White);

            // Draw strip 1
            Constants.Engine_Overlay.SpriteBatch.Draw(
                Constants.Engine_Content.CompassTextures,
                Constants.Overlay.Compass.ScreenPosition + Constants.Overlay.Compass.StripOffset + Vector2.UnitX * (int)Math.Max(0, (Constants.Overlay.Compass.StripWindowSize.X / 2 - degrees)),
                mainRectangle,
                Color.White);

            // Draw filler strips
            if (degrees < Constants.Overlay.Compass.StripWindowSize.X / 2)
            {
                Rectangle fillerRectangle = new Rectangle();
                fillerRectangle.Y = (int)Constants.Overlay.Compass.FrameSize.Y;
                fillerRectangle.Height = (int)Constants.Overlay.Compass.StripWindowSize.Y;
                fillerRectangle.X = Constants.Engine_Content.CompassTextures.Width - (int)(Constants.Overlay.Compass.StripWindowSize.X / 2 - degrees);
                fillerRectangle.Width = (int)(Constants.Overlay.Compass.StripWindowSize.X / 2) - degrees;

                Constants.Engine_Overlay.SpriteBatch.Draw(
                    Constants.Engine_Content.CompassTextures,
                    Constants.Overlay.Compass.ScreenPosition + Constants.Overlay.Compass.StripOffset,
                    fillerRectangle,
                    Color.White);
            }
            if (degrees > Constants.Engine_Content.CompassTextures.Width - Constants.Overlay.Compass.StripWindowSize.X / 2)
            {
                Rectangle fillerRectangle = new Rectangle();
                fillerRectangle.Y = (int)Constants.Overlay.Compass.FrameSize.Y;
                fillerRectangle.Height = (int)Constants.Overlay.Compass.StripWindowSize.Y;
                fillerRectangle.X = 0;
                fillerRectangle.Width = (int)(Constants.Overlay.Compass.StripWindowSize.X / 2) - (Constants.Engine_Content.CompassTextures.Width - degrees);

                Constants.Engine_Overlay.SpriteBatch.Draw(
                    Constants.Engine_Content.CompassTextures,
                    Constants.Overlay.Compass.ScreenPosition + Constants.Overlay.Compass.StripOffset + Vector2.UnitX * (Constants.Overlay.Compass.StripWindowSize.X + Constants.Engine_Content.CompassTextures.Width - degrees - Constants.Overlay.Compass.StripWindowSize.X / 2),
                    fillerRectangle,
                    Color.White);
            }
        }
    }
}
