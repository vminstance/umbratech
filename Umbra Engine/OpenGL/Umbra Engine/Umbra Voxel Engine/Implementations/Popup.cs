using System;
using System.Linq;
using System.Text;
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
using Console = Umbra.Implementations.Console;

namespace Umbra.Implementations
{
    static public class Popup
    {
        //static public Bitmap Background { get; set; }
        static double LastMessageTimeStamp = 0;
        static double LastTimeStamp = 0;
        static string LastMessage = "";
        static int alpha = 0;

        static public void Post(string message)
        {
            LastMessage = message;
            LastMessageTimeStamp = LastTimeStamp;
        }

        static public void Update(FrameEventArgs e)
        {
            LastTimeStamp = e.Time * 1000.0F;
        }

        static public void Draw()
        {
            if (LastMessageTimeStamp != 0)
            {
                if (LastTimeStamp - LastMessageTimeStamp < Constants.Overlay.Popup.Timein)
                {
                    // Fade in
                    alpha += 4;
                    if (alpha >= 255)
                    {
                        alpha = 255;
                    }
                }
                else if (LastTimeStamp - LastMessageTimeStamp > Constants.Overlay.Popup.Timeout)
                {
                    // Fade out
                    alpha -= 4;
                    if (alpha < 0)
                    {
                        alpha = 0;
                    }
                }

                //Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(0, 140, (int)Constants.Graphics.ScreenResolution.X, (int)Console.Font.MeasureString(LastMessage).Y), new Color(20, 20, 20, alpha / 3));
                //Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(0, 139, (int)Constants.Graphics.ScreenResolution.X, 1), new Color(200, 200, 200, alpha / 2));
                //Constants.Engine_Overlay.SpriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(0, 140 + (int)Console.Font.MeasureString(LastMessage).Y, (int)Constants.Graphics.ScreenResolution.X, 1), new Color(200, 200, 200, alpha / 2));
                //Constants.Engine_Overlay.SpriteBatch.DrawString(Console.Font, LastMessage, new Vector2((Constants.Graphics.ScreenResolution.X - Console.Font.MeasureString(LastMessage).X) / 2, 141), new Color(255, 255, 255, alpha));
            }
        }
    }
}
