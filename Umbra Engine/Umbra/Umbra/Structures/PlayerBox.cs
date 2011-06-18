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

namespace Umbra.Structures
{
    public class PlayerBox
    {
        Vector3 Position;

        public PlayerBox(Vector3 position)
        {
            Position = position;
        }

        public bool Intersects(BlockIndex block)
        {
            Vector3 Min = Position - new Vector3(Constants.PlayerBoxWidth / 2, 0, Constants.PlayerBoxWidth / 2);
            Vector3 Max = Position + new Vector3(Constants.PlayerBoxWidth / 2, Constants.PlayerEyeHeight, Constants.PlayerBoxWidth / 2);

            if (block.X >= Max.X)
            {
                return false;
            }

            if (block.Y >= Max.Y)
            {
                return false;
            }

            if (block.Z >= Max.Z)
            {
                return false;
            }

            if (block.X + 1 <= Min.X)
            {
                return false;
            }

            if (block.Y + 1 <= Min.Y)
            {
                return false;
            }

            if (block.Z + 1 <= Min.Z)
            {
                return false;
            }

            return true;
        }
    }
}
