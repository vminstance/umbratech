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

namespace Umbra.Engines
{
    public class Content : DrawableGameComponent
    {
        public ContentManager ContentManager;

        //Content
        public Texture2D Textures { get; private set; }
        public Texture2D BlankTexture { get; private set; }
        public SpriteFont DefaultFont { get; private set; }
        public Texture2D CompassTextures { get; private set; }
        public Texture2D CrosshairTexture { get; private set; }

        public Content(Main main)
            : base(main)
        {
            ContentManager = main.Content;
            ContentManager.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {


            //Load textures
            Textures = ContentManager.Load<Texture2D>(Constants.Content.Textures.TerrainFilename);

            BlankTexture = new Texture2D(GraphicsDevice, 1, 1);
            BlankTexture.SetData<Color>(new Color[] { Color.White });

            CrosshairTexture = ContentManager.Load<Texture2D>(Constants.Content.Textures.CrosshairFilename);

            CompassTextures = ContentManager.Load<Texture2D>(Constants.Content.Textures.CompassFilename);

            //Load fonts
            Console.Font = ContentManager.Load<SpriteFont>(Constants.Content.Fonts.ConsoleFilename);
            Constants.Engine_Overlay.DebugFont = ContentManager.Load<SpriteFont>(Constants.Content.Fonts.DebugFilename);
            Popup.Font = ContentManager.Load<SpriteFont>(Constants.Content.Fonts.PopupFilename);

            // Load Effects
            Constants.Engine_Graphics.MainEffect = ContentManager.Load<Effect>("effects/main");

            // Load Sounds
            Constants.Engine_Sound.MainMusic = ContentManager.Load<Song>(Constants.Content.Sounds.SongFilename + Variables.Sounds.CurrentTrack);
            Constants.Engine_Sound.AddBlock = ContentManager.Load<SoundEffect>(Constants.Content.Sounds.BlockEditFilename);
            Constants.Engine_Sound.Walk = ContentManager.Load<SoundEffect>(Constants.Content.Sounds.WalkFilename);

            base.LoadContent();
        }
    }
}
