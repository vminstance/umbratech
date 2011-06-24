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
    delegate float ContentFunc();

    class GraphWindow : Window
    {
        float Speed;
        double TimeSinceLastDatapoint;
        GraphingVariable graphingVariable;
        Queue<float> OldValues;

        Color BackgroundColor;
        Color GraphColors;

        public GraphWindow(Rectangle frame, float speed, Color backgroundColor, Color graphColors, GraphingVariable gVal)
        {
            Frame = frame;
            Dragable = true;
            TimeSinceLastDatapoint = 0.0F;
            Speed = speed;

            BackgroundColor = backgroundColor;
            GraphColors = graphColors;

            graphingVariable = gVal;

            OldValues = new Queue<float>();

            for (int x = 0; x < Frame.Width; x++)
            {
                OldValues.Enqueue(0.0F);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (TimeSinceLastDatapoint >= 1 / Speed)
            {
                OldValues.Dequeue();
                OldValues.Enqueue(GetValue(graphingVariable));
            }
            else
            {
                TimeSinceLastDatapoint += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override Texture2D GetContent(GraphicsDevice graphicsDevice)
        {
            Texture2D contents = new Texture2D(graphicsDevice, Frame.Width, Frame.Height);
            Color[] data = new Color[Frame.Width * Frame.Height];

            for (int x = 0; x < Frame.Width; x++)
            {
                for (int y = 0; y < Frame.Height; y++)
                {
                    data[x + y * Frame.Width] = BackgroundColor;
                }

                data[x + (int)MathHelper.Clamp((float)Math.Round(OldValues.ElementAt(x) * -1.0F) + 25, 0, Frame.Height - 1) * Frame.Width] = GraphColors;
            }

            contents.SetData(data);
            return contents;
        }

        float GetValue(GraphingVariable variable)
        {
            Player player = Constants.Engine_Physics.Player;

            switch (variable)
            {
                case GraphingVariable.PlayerAccelerationX: return player.ForceAccumulator.X / player.Mass;
                case GraphingVariable.PlayerAccelerationY: return player.ForceAccumulator.Y / player.Mass;
                case GraphingVariable.PlayerAccelerationZ: return player.ForceAccumulator.Z / player.Mass;
                case GraphingVariable.PlayerVelocityX: return player.Velocity.X;
                case GraphingVariable.PlayerVelocityY: return player.Velocity.Y;
                case GraphingVariable.PlayerVelocityZ: return player.Velocity.Z;
                case GraphingVariable.PlayerPositionX: return player.Position.X;
                case GraphingVariable.PlayerPositionY: return player.Position.Y;
                case GraphingVariable.PlayerPositionZ: return player.Position.Z;
                default: return float.NaN;
            }
        }
    }
}
