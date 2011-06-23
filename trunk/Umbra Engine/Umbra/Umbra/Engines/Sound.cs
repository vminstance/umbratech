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
    public class Sound : DrawableGameComponent
    {

        bool IsPlaying;
        Random Random;
        public Song MainMusic;
        public SoundEffect AddBlock;
        public SoundEffect Walk;

        public Sound(Main main)
            : base(main)
        {
            MediaPlayer.IsRepeating = true;
            IsPlaying = false;
            Random = new Random();
        }


        public override void Update(GameTime gameTime)
        {
            if (!IsPlaying && Variables.Sounds.MusicEnabled)
            {
                MediaPlayer.Play(MainMusic);
                IsPlaying = true;
            }
            base.Update(gameTime);
        }

        public void PlayerBlockRemoval()
        {
            if (Variables.Sounds.InteractSoundEnabled)
            {
                AddBlock.Play();
            }
        }

        public void PlayerBlockAdd()
        {
            if (Variables.Sounds.InteractSoundEnabled)
            {
                AddBlock.Play();
            }
        }

        public void PlayerWalk()
        {
            if (Variables.Sounds.WalkSoundEnabled)
            {
                Walk.Play((float)(Random.NextDouble() * 0.3F) + 0.7F, (float)(Random.NextDouble() * 0.5F) - 0.25F, 0.0F);
            }
        }
    }
}
