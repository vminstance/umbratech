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
    public class AABB
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public List<BlockIndex> IntersectionIndices
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
                            if(Intersects(new BlockIndex(x,y,z).GetBoundingBox()))
                            {

                            }
                            returnList.Add(new BlockIndex(x, y, z));
                        }
                    }
                }

                return returnList;
            }
        }

        public float SurfaceArea
        {
            get
            {
                return 2 * ((Max.X - Min.X) * (Max.Y - Min.Y) + (Max.X - Min.X) * (Max.Z - Min.Z) + (Max.Y - Min.Y) * (Max.Z - Min.Z));
            }
        }

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        static public AABB PlayerBoundingBox(Vector3 position)
        {
            return new AABB(position, position + new Vector3(Constants.Player.Physics.Box.Width, Constants.Player.Physics.Box.Height, Constants.Player.Physics.Box.Width));
        }

        public bool Intersects(AABB box)
        {
            if (box.Min.X >= Max.X || box.Min.X >= Max.X)
            {
                return false;
            }

            if (box.Min.Y >= Max.Y || box.Min.Y >= Max.Y)
            {
                return false;
            }

            if (box.Min.Z >= Max.Z || box.Min.Z >= Max.Z)
            {
                return false;
            }

            return true;
        }

        public float? Intersects(Ray ray)
        {
            return ray.Intersects(new BoundingBox(Min, Max));
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

        public AABB At(Vector3 pos)
        {
            return new AABB(pos, (Max - Min) + pos);
        }
    }
}
