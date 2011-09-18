using System;
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

namespace Umbra.Structures.Forms
{
    class Form
    {
        Rectangle FormRectangle;
        public Panel Content;

        bool IsOpen;
        bool HasFrame;
        bool Resizable;
        bool Dragable;
        public Corner HandlePosition;

        #region - Properties -
        Rectangle TopHandle
        {
            get
            {
                if (Dragable)
                {
                    return new Rectangle(FormRectangle.X + 3, FormRectangle.Y + 3, FormRectangle.Width - 6, 20);
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        Rectangle ClientFrame
        {
            get
            {
                if (HasFrame)
                {
                    if (Dragable)
                    {
                        return new Rectangle(FormRectangle.X + 3, FormRectangle.Y + 26, FormRectangle.Width - 6, FormRectangle.Height - 29);
                    }
                    else
                    {
                        return new Rectangle(FormRectangle.X + 3, FormRectangle.Y + 3, FormRectangle.Width - 6, FormRectangle.Height - 6);
                    }
                }
                else
                {
                    return FormRectangle;
                }
            }
        }
        #endregion

        public Form(int x, int y, int width, int height)
        {
            FormRectangle = new Rectangle(x, y, width, height);
            Content = null;

            IsOpen = false;
            HasFrame = true;
            Resizable = true;
            Dragable = true;
            HandlePosition = Corner.BottomRight;
        }

        public void Show()
        {
            IsOpen = true;
        }

        public void Render()
        {
            if (!IsOpen)
            {
                return;
            }

            if (HasFrame)
            {
                // Main body
                RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, FormRectangle, Color.FromArgb(120, Color.Black));
            }

            if (Dragable)
            {
                // Top handle
                RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, TopHandle, Color.FromArgb(120, Color.Black));
            }

            if (Resizable)
            {
                // Resize handle
                switch (HandlePosition)
                {
                    case Corner.TopLeft:
                        {
                            RenderHelp.RenderTexture(
                                Constants.Engines.Overlay.BlankTextureID,
                                new Rectangle(FormRectangle.X, FormRectangle.Y, 4, 4),
                                Color.FromArgb(120, Color.Black));
                            break;
                        }
                    case Corner.TopRight:
                        {
                            RenderHelp.RenderTexture(
                                Constants.Engines.Overlay.BlankTextureID,
                                new Rectangle(FormRectangle.X + FormRectangle.Width - 4, FormRectangle.Y, 4, 4),
                                Color.FromArgb(120, Color.Black));
                            break;
                        }
                    case Corner.BottomLeft:
                        {
                            RenderHelp.RenderTexture(
                                Constants.Engines.Overlay.BlankTextureID,
                                new Rectangle(FormRectangle.X, FormRectangle.Y + FormRectangle.Height - 4, 4, 4),
                                Color.FromArgb(120, Color.Black));
                            break;
                        }
                    case Corner.BottomRight:
                        {
                            RenderHelp.RenderTexture(
                                Constants.Engines.Overlay.BlankTextureID,
                                new Rectangle(FormRectangle.X + FormRectangle.Width - 4, FormRectangle.Y + FormRectangle.Height - 4, 4, 4),
                                Color.FromArgb(120, Color.Black));
                            break;
                        }
                }
            }

            // Client frame
            Content.Render(ClientFrame);
        }

        public void Update()
        {
            if(TopHandle.Contains(Constants.Engines.Input.MousePosition))
            {

            }

            // Resizing

            Content.Update();
        }
    }

    enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
