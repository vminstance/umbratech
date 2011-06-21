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

        public float IntersectionVolume(BlockIndex block)
        {
            Vector3 min = Position - new Vector3(Constants.PlayerBoxWidth / 2, 0, Constants.PlayerBoxWidth / 2);
            Vector3 max = Position + new Vector3(Constants.PlayerBoxWidth / 2, Constants.PlayerEyeHeight, Constants.PlayerBoxWidth / 2);

            float intersectionVolume = 1;

            if(!block.GetBoundingBox().Intersects(new BoundingBox(min, max)))
            {
                return 0;
            }

            List<float> points = new List<float>();
            points.Add(min.X);
            points.Add(block.X);
            points.Add(max.X);
            points.Add(block.X + 1);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            points.Clear();
            points.Add(min.Y);
            points.Add(block.Y);
            points.Add(max.Y);
            points.Add(block.Y + 1);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            points.Clear();
            points.Add(min.Z);
            points.Add(block.Z);
            points.Add(max.Z);
            points.Add(block.Z + 1);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            return intersectionVolume;
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(Position - new Vector3(Constants.PlayerBoxWidth / 2, 0, Constants.PlayerBoxWidth / 2), Position + new Vector3(Constants.PlayerBoxWidth / 2, Constants.PlayerEyeHeight, Constants.PlayerBoxWidth / 2));
        }
    }
}
