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
    public abstract class WindowComponent
    {
        protected List<WindowComponent> Children;
        protected Dictionary<string, Handle> Handles;
        protected WindowComponent Parent;
        protected Rectangle Frame;

        public WindowComponent()
        {
            Handles = new Dictionary<string, Handle>();
            Children = new List<WindowComponent>();
            //Event_OnClick += Component_OnClick;
            //Event_OnPaint += Component_OnPaint;
            //Event_OnUpdate += Component_OnUpdate;
        }

        #region Event Declarations
        public EventFunction Event_OnClick;
        public EventFunction Event_OnResize;
        public EventFunction Event_OnPaint;
        public EventFunction Event_OnUpdate;
        public EventFunction Event_OnGainFocus;
        public EventFunction Event_OnLostFocus;
        public EventFunction Event_MouseEnter;
        public EventFunction Event_MouseLeave;
        public EventFunction Event_MouseClick;
        public EventFunction Event_MouseMove;
        public EventFunction Event_KeyPress;
        
        // Windows
        public EventFunction Event_OnOpen;
        public EventFunction Event_OnClose;
        #endregion

        #region Event Functions
        //virtual void Component_OnClick(GameTime gameTime, object[] args)
        //{
        //}
        //virtual void Component_OnPaint(GameTime gameTime, object[] args)
        //{
        //}

        //virtual void Component_OnUpdate(GameTime gameTime, object[] args)
        //{
        //}
        #endregion
    }
}
