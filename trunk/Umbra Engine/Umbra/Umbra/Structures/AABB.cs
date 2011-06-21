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
    public class AABB
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public List<BlockIndex> IntersectionSpace
        {
            get
            {
                List<BlockIndex> returnList = new List<BlockIndex>();

                for (int x = (int)Math.Floor(Min.X); x <= Math.Floor(Max.X); x++)
                {
                    for (int y = (int)Math.Floor(Min.Y); y <= Math.Floor(Max.Y); y++)
                    {
                        for (int z = (int)Math.Floor(Min.Z); z <= Math.Floor(Max.Z); z++)
                        {
                            returnList.Add(new BlockIndex(x, y, z));
                        }
                    }
                }

                return returnList;
            }
        }

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        static public AABB PlayerBoundingBox(Vector3 position)
        {
            return new AABB(
                position - new Vector3(Constants.PlayerBoxWidth / 2, 0, Constants.PlayerBoxWidth / 2),
                position + new Vector3(Constants.PlayerBoxWidth / 2, Constants.PlayerEyeHeight, Constants.PlayerBoxWidth / 2));
        }

        public bool Intersects(AABB block)
        {

            if (block.Max.X >= Max.X)
            {
                return false;
            }

            if (block.Max.X >= Max.Y)
            {
                return false;
            }

            if (block.Max.Z >= Max.Z)
            {
                return false;
            }

            if (block.Min.X + 1 <= Min.X)
            {
                return false;
            }

            if (block.Min.Y + 1 <= Min.Y)
            {
                return false;
            }

            if (block.Min.Z + 1 <= Min.Z)
            {
                return false;
            }

            return true;
        }

        public float IntersectionVolume(AABB box)
        {
            float intersectionVolume = 1;

            if (!Intersects(box))
            {
                return 0;
            }

            List<float> points = new List<float>();
            points.Add(Min.X);
            points.Add(box.Min.X);
            points.Add(Max.X);
            points.Add(box.Max.X);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            points.Clear();
            points.Add(Min.Y);
            points.Add(box.Min.Y);
            points.Add(Max.Y);
            points.Add(box.Max.Y);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            points.Clear();
            points.Add(Min.Z);
            points.Add(box.Min.Z);
            points.Add(Max.Z);
            points.Add(box.Max.Z);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            return intersectionVolume;
        }
    }
}
