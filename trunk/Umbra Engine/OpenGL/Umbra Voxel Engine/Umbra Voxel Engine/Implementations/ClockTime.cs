﻿using System;
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

namespace Umbra.Implementations
{
    static public class ClockTime
    {
        static float Time;
        static public void SetTimeOfDayGraphics(FrameEventArgs e)
        {
            if (Variables.Graphics.DayNight.CycleEnabled)
            {
                Time += Interpolation.Linear(0, 360, (float)e.Time / Constants.Graphics.DayNight.TotalDuration);
                if (Time > 360)
                {
                    Time = 0;
                }
            }
            else
            {
                SetTimeOfDay(TimeOfDay.Day);
            }

            if (GetCurrentTimeOfDay() == TimeOfDay.Night)
            {
                Variables.Graphics.Fog.CurrentColor = new float[] { Variables.Graphics.ScreenClearColor.R / 255F, Variables.Graphics.ScreenClearColor.G / 255F, Variables.Graphics.ScreenClearColor.B / 255F, 1F };
                Variables.Graphics.Fog.CurrentStart = Constants.Graphics.Fog.NightFogStart;
                Variables.Graphics.Fog.CurrentEnd = Constants.Graphics.Fog.NightFogEnd;
                Variables.Graphics.DayNight.CurrentFaceLightCoef = Constants.Graphics.Lighting.NightFaceLightCoef;
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.SunRise)
            {
                float sunDegreeRelative = (Time * (Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.NightDuration + 2 * Constants.Graphics.DayNight.TransitionDuration) / 360) - (Constants.Graphics.DayNight.NightDuration / 2);

                Variables.Graphics.Fog.CurrentColor = new float[] { Variables.Graphics.ScreenClearColor.R / 255F, Variables.Graphics.ScreenClearColor.G / 255F, Variables.Graphics.ScreenClearColor.B / 255F, 1F };
                Variables.Graphics.Fog.CurrentStart = Interpolation.Linear(Constants.Graphics.Fog.NightFogStart, Constants.Graphics.Fog.DayFogStart, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration);
                Variables.Graphics.Fog.CurrentEnd = Interpolation.Linear(Constants.Graphics.Fog.NightFogEnd, Constants.Graphics.Fog.DayFogEnd, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration);
                Variables.Graphics.DayNight.CurrentFaceLightCoef = Interpolation.Linear(Constants.Graphics.Lighting.NightFaceLightCoef, Constants.Graphics.Lighting.DayFaceLightCoef, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration);
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.Day)
            {
                Variables.Graphics.Fog.CurrentColor = new float[] { Variables.Graphics.ScreenClearColor.R / 255F, Variables.Graphics.ScreenClearColor.G / 255F, Variables.Graphics.ScreenClearColor.B / 255F, 1F };
                Variables.Graphics.Fog.CurrentStart = Constants.Graphics.Fog.DayFogStart;
                Variables.Graphics.Fog.CurrentEnd = Constants.Graphics.Fog.DayFogEnd;
                Variables.Graphics.DayNight.CurrentFaceLightCoef = Constants.Graphics.Lighting.DayFaceLightCoef;
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.SunSet)
            {
                float sunDegreeRelative = (Time * (Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.NightDuration + 2 * Constants.Graphics.DayNight.TransitionDuration) / 360) - (Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration + Constants.Graphics.DayNight.DayDuration);

                Variables.Graphics.Fog.CurrentColor = new float[] { Variables.Graphics.ScreenClearColor.R / 255F, Variables.Graphics.ScreenClearColor.G / 255F, Variables.Graphics.ScreenClearColor.B / 255F, 1F };
                Variables.Graphics.Fog.CurrentStart = Interpolation.Linear(Constants.Graphics.Fog.DayFogStart, Constants.Graphics.Fog.NightFogStart, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration);
                Variables.Graphics.Fog.CurrentEnd = Interpolation.Linear(Constants.Graphics.Fog.DayFogEnd, Constants.Graphics.Fog.NightFogEnd, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration);
                Variables.Graphics.DayNight.CurrentFaceLightCoef = Interpolation.Linear(Constants.Graphics.Lighting.DayFaceLightCoef, Constants.Graphics.Lighting.NightFaceLightCoef, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration);
            }
        }

        static public Color GetScreenColor()
        {
            if (GetCurrentTimeOfDay() == TimeOfDay.Night)
            {
                return Constants.Graphics.DayNight.NightColor;
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.SunRise)
            {
                float sunDegreeRelative = (Time * (Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.NightDuration + 2 * Constants.Graphics.DayNight.TransitionDuration) / 360) - (Constants.Graphics.DayNight.NightDuration / 2);
                
                return Color.FromArgb(
                    (int)(Interpolation.Linear(Constants.Graphics.DayNight.NightColor.R, Constants.Graphics.DayNight.DayColor.R, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration) / 255F * 255.0F),
                    (int)(Interpolation.Linear(Constants.Graphics.DayNight.NightColor.G, Constants.Graphics.DayNight.DayColor.G, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration) / 255F * 255.0F),
                    (int)(Interpolation.Linear(Constants.Graphics.DayNight.NightColor.B, Constants.Graphics.DayNight.DayColor.B, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration) / 255F * 255.0F)
                );
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.Day)
            {
                return Constants.Graphics.DayNight.DayColor;
            }
            else if (GetCurrentTimeOfDay() == TimeOfDay.SunSet)
            {
                float sunDegreeRelative = (Time * (Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.NightDuration + 2 * Constants.Graphics.DayNight.TransitionDuration) / 360) - (Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration + Constants.Graphics.DayNight.DayDuration);
                return Color.FromArgb(
                    (int)(Interpolation.Linear(Constants.Graphics.DayNight.DayColor.R, Constants.Graphics.DayNight.NightColor.R, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration) / 255F * 255.0F),
                    (int)(Interpolation.Linear(Constants.Graphics.DayNight.DayColor.G, Constants.Graphics.DayNight.NightColor.G, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration) / 255F * 255.0F),
                    (int)(Interpolation.Linear(Constants.Graphics.DayNight.DayColor.B, Constants.Graphics.DayNight.NightColor.B, sunDegreeRelative / Constants.Graphics.DayNight.TransitionDuration) / 255F * 255.0F)
                );
            }
            else
            {
                //Shouldn't happen

                return Constants.Graphics.DayNight.DayColor;
            }
        }

        static public void SetTimeOfDay(TimeOfDay timeOfDay)
        {
            switch (timeOfDay)
            {
                case TimeOfDay.SunRise:
                    SetTimeOfDay((Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration / 2) / Constants.Graphics.DayNight.TotalDuration * 360);
                    break;

                case TimeOfDay.Day:
                    SetTimeOfDay((Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration + Constants.Graphics.DayNight.DayDuration / 2) / Constants.Graphics.DayNight.TotalDuration * 360);
                    break;

                case TimeOfDay.SunSet:
                    SetTimeOfDay((Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration + Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.TransitionDuration / 2) / Constants.Graphics.DayNight.TotalDuration * 360);
                    break;

                default: // Night
                    SetTimeOfDay(0.0F);
                    break;
            }
        }

        static public void SetTimeOfDay(float time)
        {
            Time = (float)Mathematics.WrapAngleDegrees(time);
        }

        static private TimeOfDay GetCurrentTimeOfDay()
        {
            return GetTimeOfDay(Time);
        }

        static private TimeOfDay GetTimeOfDay(float time)
        {
            float totalDuration = Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.NightDuration + 2 * Constants.Graphics.DayNight.TransitionDuration;
            float sunDegreeRelative = Time * totalDuration / 360;

            if (sunDegreeRelative < Constants.Graphics.DayNight.NightDuration / 2)
            {
                return TimeOfDay.Night;
            }
            else if (sunDegreeRelative < Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration)
            {
                return TimeOfDay.SunRise;
            }
            else if (sunDegreeRelative < Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration + Constants.Graphics.DayNight.DayDuration)
            {
                return TimeOfDay.Day;
            }
            else if (sunDegreeRelative < Constants.Graphics.DayNight.NightDuration / 2 + Constants.Graphics.DayNight.TransitionDuration + Constants.Graphics.DayNight.DayDuration + Constants.Graphics.DayNight.TransitionDuration)
            {
                return TimeOfDay.SunSet;
            }

            return TimeOfDay.Night;
        }
    }
}
