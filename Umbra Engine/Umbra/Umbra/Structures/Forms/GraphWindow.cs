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
    public delegate float[] GraphFunction();

    public class GraphWindow : Window
    {
        float Speed;
        double TimeSinceLastDatapoint;
        GraphFunction GraphFunction;
        Queue<float[]> OldValues;
        Color GraphColor;
        SpriteFont Font;
        Rectangle ContentFrame;

        public GraphWindow(string title, Rectangle frame, float speed, Color graphColors, GraphFunction graphFunction)
            : base(title, frame)
        {
            Frame = frame;
            ContentFrame = new Rectangle(Frame.X + 35, Frame.Y + 20, Frame.Width - 37, Frame.Height - 22);

            Dragable = true;
            Resizeable = true;
            TimeSinceLastDatapoint = 0.0F;
            Speed = speed;
            GraphColor = graphColors;
            Font = Constants.Engine_Content.DefaultFont;

            GraphFunction = graphFunction;
            float[] setupValue = graphFunction.Invoke();

            OldValues = new Queue<float[]>();

            for (int x = 0; x < ContentFrame.Width; x++)
            {
                OldValues.Enqueue(setupValue);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (TimeSinceLastDatapoint >= 1 / Speed)
            {
                OldValues.Dequeue();
                OldValues.Enqueue(GraphFunction.Invoke());
            }
            else
            {
                TimeSinceLastDatapoint += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            string arrow = ">";

            Texture2D contents = new Texture2D(spriteBatch.GraphicsDevice, ContentFrame.Width, ContentFrame.Height);
            Color[] data = new Color[ContentFrame.Width * ContentFrame.Height];

            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (float[] values in OldValues)
            {
                for (int i = 0; i < OldValues.Last().Length; i++)
                {
                    float value = values[i];
                    if (value < min)
                    {
                        min = value;
                    }

                    if (value > max)
                    {
                        max = value;
                    }
                }
            }

            if (min == max)
            {
                max++;
                min--;
            }

            for (int x = 0; x < ContentFrame.Width; x++)
            {
                for (int y = 0; y < ContentFrame.Height; y++)
                {
                    data[x + y * ContentFrame.Width] = Color.Transparent;
                }

                for (int i = 0; i < OldValues.Last().Length; i++)
                {
                    float value = OldValues.ElementAt(ContentFrame.Width - (x + 1))[i];

                    int height = (int)MathHelper.Clamp((int)(((value - min) / (max - min)) * (ContentFrame.Height - 1)), 0, ContentFrame.Height - 1);

                    data[x + (ContentFrame.Height - height - 1) * ContentFrame.Width] = GraphColor;
                }
            }

            contents.SetData(data);


            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Frame.X + 2, Frame.Y + 20, 31, Frame.Height - 22), Color.DarkGray);
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, ContentFrame, Color.DarkGray);
            spriteBatch.Draw(contents, ContentFrame, Color.White);

            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Frame.X + 35, Frame.Y + 20, 35, 15), new Color(20, 20, 20, 100));
            spriteBatch.DrawString(Font, Math.Round(max, 1) + "", new Vector2(Frame.X + 37, Frame.Y + 22), Color.White);
            spriteBatch.Draw(Constants.Engine_Content.BlankTexture, new Rectangle(Frame.X + 35, Frame.Y + Frame.Height - 17, 35, 15), new Color(20, 20, 20, 100));
            spriteBatch.DrawString(Font, Math.Round(min, 1) + "", new Vector2(Frame.X + 37, Frame.Y + Frame.Height - 15), Color.White);

            for (int i = 0; i < OldValues.Last().Length; i++)
            {
                float value = OldValues.Last()[i];
                int height = (ContentFrame.Height - (int)MathHelper.Clamp((int)(((value - min) / (max - min)) * (ContentFrame.Height - 1)), 0, ContentFrame.Height - 1) - 1);

                string stringValue = Math.Round(value, 1) + "";

                spriteBatch.DrawString(Font, arrow, new Vector2(Frame.X + 34 - Font.MeasureString(arrow).X, height + Frame.Y + 15), Color.White);
                spriteBatch.DrawString(Font, stringValue, new Vector2(Frame.X + 26 - Font.MeasureString(stringValue).X, MathHelper.Clamp(height, Font.MeasureString(stringValue).Y / 2, ContentFrame.Height - Font.MeasureString(stringValue).Y / 2) + Frame.Y + 15), Color.White);
            }
        }
    }
}
