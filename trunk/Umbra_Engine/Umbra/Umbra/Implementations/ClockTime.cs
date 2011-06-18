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
    static public class ClockTime
    {
        static float Time;
        static public void SetTimeOfDayGraphics(GameTime gameTime)
        {
            if (Constants.DayNightCycleEnabled)
            {

                Time += MathHelper.Lerp(0, 360, (float)gameTime.ElapsedGameTime.TotalSeconds / Constants.TotalDuration);
                if (Time > 360)
                {
                    Time = 0;
                }
            }

            if (GetCurrentTimeOfDay() == TimeOfDay.Night)
            {
                Constants.ScreenClearColor = Constants.NightColor;

                Constants.CurrentFogColor = new float[] { Constants.ScreenClearColor.R / 255F, Constants.ScreenClearColor.G / 255F, Constants.ScreenClearColor.B / 255F, 1F };
                Constants.CurrentFogStart = Constants.NightFogStart;
                Constants.CurrentFogEnd = Constants.NightFogEnd;
                Constants.CurrentFaceLightCoef = Constants.NightFaceLightCoef;
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.SunRise)
            {
                float sunDegreeRelative = (Time * (Constants.DayDuration + Constants.NightDuration + 2 * Constants.TransitionDuration) / 360) - (Constants.NightDuration / 2);
                Constants.ScreenClearColor = new Color(
                    MathHelper.Lerp(Constants.NightColor.R, Constants.DayColor.R, sunDegreeRelative / Constants.TransitionDuration) / 255F,
                    MathHelper.Lerp(Constants.NightColor.G, Constants.DayColor.G, sunDegreeRelative / Constants.TransitionDuration) / 255F,
                    MathHelper.Lerp(Constants.NightColor.B, Constants.DayColor.B, sunDegreeRelative / Constants.TransitionDuration) / 255F
                );
                Constants.CurrentFogColor = new float[] { Constants.ScreenClearColor.R / 255F, Constants.ScreenClearColor.G / 255F, Constants.ScreenClearColor.B / 255F, 1F };
                Constants.CurrentFogStart = MathHelper.Lerp(Constants.NightFogStart, Constants.DayFogStart, sunDegreeRelative / Constants.TransitionDuration);
                Constants.CurrentFogEnd = MathHelper.Lerp(Constants.NightFogEnd, Constants.DayFogEnd, sunDegreeRelative / Constants.TransitionDuration);
                Constants.CurrentFaceLightCoef = MathHelper.Lerp(Constants.NightFaceLightCoef, Constants.DayFaceLightCoef, sunDegreeRelative / Constants.TransitionDuration);
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.Day)
            {
                Constants.ScreenClearColor = Constants.DayColor;

                Constants.CurrentFogColor = new float[] { Constants.ScreenClearColor.R / 255F, Constants.ScreenClearColor.G / 255F, Constants.ScreenClearColor.B / 255F, 1F };
                Constants.CurrentFogStart = Constants.DayFogStart;
                Constants.CurrentFogEnd = Constants.DayFogEnd;
                Constants.CurrentFaceLightCoef = Constants.DayFaceLightCoef;
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.SunSet)
            {
                float sunDegreeRelative = (Time * (Constants.DayDuration + Constants.NightDuration + 2 * Constants.TransitionDuration) / 360) - (Constants.NightDuration / 2 + Constants.TransitionDuration + Constants.DayDuration);
                Constants.ScreenClearColor = new Color(
                    MathHelper.Lerp(Constants.DayColor.R, Constants.NightColor.R, sunDegreeRelative / Constants.TransitionDuration) / 255F,
                    MathHelper.Lerp(Constants.DayColor.G, Constants.NightColor.G, sunDegreeRelative / Constants.TransitionDuration) / 255F,
                    MathHelper.Lerp(Constants.DayColor.B, Constants.NightColor.B, sunDegreeRelative / Constants.TransitionDuration) / 255F
                );
                Constants.CurrentFogColor = new float[] { Constants.ScreenClearColor.R / 255F, Constants.ScreenClearColor.G / 255F, Constants.ScreenClearColor.B / 255F, 1F };
                Constants.CurrentFogStart = MathHelper.Lerp(Constants.DayFogStart, Constants.NightFogStart, sunDegreeRelative / Constants.TransitionDuration);
                Constants.CurrentFogEnd = MathHelper.Lerp(Constants.DayFogEnd, Constants.NightFogEnd, sunDegreeRelative / Constants.TransitionDuration);
                Constants.CurrentFaceLightCoef = MathHelper.Lerp(Constants.DayFaceLightCoef, Constants.NightFaceLightCoef, sunDegreeRelative / Constants.TransitionDuration);
            }
        }

        static public void SetTimeOfDay(TimeOfDay timeOfDay)
        {
            switch (timeOfDay)
            {
                case TimeOfDay.SunRise:
                    SetTimeOfDay((Constants.NightDuration / 2 + Constants.TransitionDuration / 2) / Constants.TotalDuration * 360);
                    break;

                case TimeOfDay.Day:
                    SetTimeOfDay((Constants.NightDuration / 2 + Constants.TransitionDuration + Constants.DayDuration / 2) / Constants.TotalDuration * 360);
                    break;

                case TimeOfDay.SunSet:
                    SetTimeOfDay((Constants.NightDuration / 2 + Constants.TransitionDuration + Constants.DayDuration + Constants.TransitionDuration / 2) / Constants.TotalDuration * 360);
                    break;

                default: // Day
                    SetTimeOfDay(0.0F);
                    break;
            }
        }

        static public void SetTimeOfDay(float time)
        {
            Time = time % 360;
        }

        static private TimeOfDay GetCurrentTimeOfDay()
        {
            return GetTimeOfDay(Time);
        }

        static private TimeOfDay GetTimeOfDay(float time)
        {
            float totalDuration = Constants.DayDuration + Constants.NightDuration + 2 * Constants.TransitionDuration;
            float sunDegreeRelative = Time * totalDuration / 360;

            if (sunDegreeRelative < Constants.NightDuration / 2)
            {
                return TimeOfDay.Night;
            }
            else if (sunDegreeRelative < Constants.NightDuration / 2 + Constants.TransitionDuration)
            {
                return TimeOfDay.SunRise;
            }
            else if (sunDegreeRelative < Constants.NightDuration / 2 + Constants.TransitionDuration + Constants.DayDuration)
            {
                return TimeOfDay.Day;
            }
            else if (sunDegreeRelative < Constants.NightDuration / 2 + Constants.TransitionDuration + Constants.DayDuration + Constants.TransitionDuration)
            {
                return TimeOfDay.SunSet;
            }

            return TimeOfDay.Night;
        }
    }
}
