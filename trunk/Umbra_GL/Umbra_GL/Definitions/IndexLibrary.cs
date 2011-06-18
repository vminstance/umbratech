using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Internal;
using Vector3 = Umbra.Structures.Internal.Vector3;
using Console = Umbra.Implementations.Console;

namespace Umbra.Definitions
{
    public class WorldIndex
    {
        public float X;
        public float Y;

        public Vector3 Position
        {
            get { return new Vector3(X * Constants.ChunkSize * Constants.WorldSize, 0, Y * Constants.ChunkSize * Constants.WorldSize); }
        }

        public WorldIndex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public WorldIndex(Vector2 position)
        {
            X = (int)Math.Floor((double)position.X / (double)(Constants.ChunkSize * Constants.WorldSize));
            Y = (int)Math.Floor((double)position.Y / (double)(Constants.ChunkSize * Constants.WorldSize));
        }

        public WorldIndex(Vector3 position)
        {
            X = (int)Math.Floor((double)position.X / (double)(Constants.ChunkSize * Constants.WorldSize));
            Y = (int)Math.Floor((double)position.Z / (double)(Constants.ChunkSize * Constants.WorldSize));
        }

        public WorldIndex(ChunkIndex index)
        {
            X = (int)Math.Floor((double)index.X / (double)Constants.WorldSize);
            Y = (int)Math.Floor((double)index.Z / (double)Constants.WorldSize);
        }

        public BlockIndex ToBlockIndex()
        {
            return new BlockIndex((int)(X * Constants.ChunkSize * Constants.WorldSize), 0, (int)(Y * Constants.ChunkSize * Constants.WorldSize));
        }

        public float DistanceFromOrigo()
        {
            return (float)Math.Sqrt(Math.Pow(X * Constants.ChunkSize * Constants.WorldSize, 2) + Math.Pow(Y * Constants.ChunkSize * Constants.WorldSize, 2));
        }

        public static WorldIndex Zero { get { return new WorldIndex(0, 0); } }
        public static WorldIndex UnitX { get { return new WorldIndex(1, 0); } }
        public static WorldIndex UnitY { get { return new WorldIndex(0, 1); } }
        public static WorldIndex One { get { return new WorldIndex(1, 1); } }

        public static WorldIndex operator +(WorldIndex part1, WorldIndex part2)
        {
            return new WorldIndex(part1.X + part2.X, part1.Y + part2.Y);
        }

        public static WorldIndex operator +(WorldIndex part1, ChunkIndex part2)
        {
            return new WorldIndex(part1.X - (float)part2.X / (float)Constants.WorldSize, part1.Y - (float)part2.Z / (float)Constants.WorldSize);
        }

        public static WorldIndex operator -(WorldIndex part1, WorldIndex part2)
        {
            return new WorldIndex(part1.X - part2.X, part1.Y - part2.Y);
        }

        public static WorldIndex operator *(WorldIndex part1, int part2)
        {
            return new WorldIndex(part1.X * part2, part1.Y * part2);
        }

        public static WorldIndex operator *(WorldIndex part1, float part2)
        {
            return new WorldIndex((int)((float)part1.X * part2), (int)((float)part1.Y * part2));
        }

        public static WorldIndex operator *(WorldIndex part1, WorldIndex part2)
        {
            return new WorldIndex(part1.X * part2.X, part1.Y * part2.Y);
        }

        public static WorldIndex operator /(WorldIndex part1, float part2)
        {
            return new WorldIndex(part1.X / part2, part1.Y / part2);
        }
        public static WorldIndex operator %(WorldIndex part1, int part2)
        {
            return new WorldIndex(part1.X % part2, part1.Y % part2);
        }

        public static bool operator ==(WorldIndex part1, WorldIndex part2)
        {
            return (part1.X == part2.X && part1.Y == part2.Y);
        }

        public static bool operator !=(WorldIndex part1, WorldIndex part2)
        {
            return !(part1.X == part2.X && part1.Y == part2.Y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class ChunkIndex
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3 Position
        {
            get { return new Vector3(X * Constants.ChunkSize, Y * Constants.ChunkSize, Z * Constants.ChunkSize); }
        }

        public ChunkIndex(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public ChunkIndex(Vector2 position)
        {
            X = (int)Math.Floor((double)position.X / (double)Constants.ChunkSize);
            Y = 0;
            Z = (int)Math.Floor((double)position.Y / (double)Constants.ChunkSize);
        }

        public ChunkIndex(Vector3 position)
        {
            X = (int)Math.Floor((double)position.X / (double)Constants.ChunkSize);
            Y = (int)Math.Floor((double)position.Y / (double)Constants.ChunkSize);
            Z = (int)Math.Floor((double)position.Z / (double)Constants.ChunkSize);
        }

        public BlockIndex ToBlockIndex()
        {
            return new BlockIndex(X * Constants.ChunkSize, Y * Constants.ChunkSize, Z * Constants.ChunkSize);
        }

        public float DistanceFromOrigo()
        {
            return (float)Math.Sqrt(Math.Pow(X * Constants.ChunkSize, 2) + Math.Pow(Y * Constants.ChunkSize, 2) + Math.Pow(Z * Constants.ChunkSize, 2));
        }

        public static ChunkIndex Zero { get { return new ChunkIndex(0, 0, 0); } }
        public static ChunkIndex UnitX { get { return new ChunkIndex(1, 0, 0); } }
        public static ChunkIndex UnitY { get { return new ChunkIndex(0, 1, 0); } }
        public static ChunkIndex UnitZ { get { return new ChunkIndex(0, 0, 1); } }
        public static ChunkIndex One { get { return new ChunkIndex(1, 1, 1); } }

        public static ChunkIndex operator +(ChunkIndex part1, ChunkIndex part2)
        {
            return new ChunkIndex(part1.X + part2.X, part1.Y + part2.Y, part1.Z + part2.Z);
        }

        public static ChunkIndex operator +(ChunkIndex part1, WorldIndex part2)
        {
            return new ChunkIndex(part1.X + (int)(part2.X * Constants.WorldSize), part1.Y + (int)(part2.Y * Constants.WorldSize), part1.Z + (int)(part2.Y * Constants.WorldSize));
        }

        public static ChunkIndex operator -(ChunkIndex part1, ChunkIndex part2)
        {
            return new ChunkIndex(part1.X - part2.X, part1.Y - part2.Y, part1.Z - part2.Z);
        }

        public static ChunkIndex operator -(ChunkIndex part1, WorldIndex part2)
        {
            return new ChunkIndex(part1.X - (int)(part2.X * Constants.WorldSize), part1.Y - (int)(part2.Y * Constants.WorldSize), part1.Z - (int)(part2.Y * Constants.WorldSize));
        }

        public static ChunkIndex operator *(ChunkIndex part1, int part2)
        {
            return new ChunkIndex(part1.X * part2, part1.Y * part2, part1.Z * part2);
        }

        public static ChunkIndex operator *(ChunkIndex part1, float part2)
        {
            return new ChunkIndex((int)((float)part1.X * part2), (int)((float)part1.Y * part2), (int)((float)part1.Z * part2));
        }

        public static ChunkIndex operator *(ChunkIndex part1, ChunkIndex part2)
        {
            return new ChunkIndex(part1.X * part2.X, part1.Y * part2.Y, part1.Z * part2.Z);
        }

        public static ChunkIndex operator /(ChunkIndex part1, float part2)
        {
            return new ChunkIndex((int)((float)part1.X / part2), (int)((float)part1.Y / part2), (int)((float)part1.Z / part2));
        }
        public static ChunkIndex operator %(ChunkIndex part1, int part2)
        {
            return new ChunkIndex(part1.X % part2, part1.Y % part2, part1.Z % part2);
        }

        public static bool operator ==(ChunkIndex part1, ChunkIndex part2)
        {
            return (part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
        }

        public static bool operator !=(ChunkIndex part1, ChunkIndex part2)
        {
            return !(part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class BlockIndex
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3 Position
        {
            get { return new Vector3(X, Y, Z); }
        }

        public static BlockIndex Zero { get { return new BlockIndex(0, 0, 0); } }
        public static BlockIndex UnitX { get { return new BlockIndex(1, 0, 0); } }
        public static BlockIndex UnitY { get { return new BlockIndex(0, 1, 0); } }
        public static BlockIndex UnitZ { get { return new BlockIndex(0, 0, 1); } }
        public static BlockIndex One { get { return new BlockIndex(1, 1, 1); } }

        public BlockIndex(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public BlockIndex(Vector3 position)
        {
            X = (int)Math.Floor(position.X);
            Y = (int)Math.Floor(position.Y);
            Z = (int)Math.Floor(position.Z);
        }

        public ChunkIndex ToChunkIndex()
        {
            return new ChunkIndex((int)Math.Floor((float)X / Constants.ChunkSize), (int)Math.Floor((float)Y / Constants.ChunkSize), (int)Math.Floor((float)Z / Constants.ChunkSize));
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(this.Position, (this + BlockIndex.One).Position);
        }

        public static BlockIndex operator +(BlockIndex part1, BlockIndex part2)
        {
            return new BlockIndex(part1.X + part2.X, part1.Y + part2.Y, part1.Z + part2.Z);
        }

        public static BlockIndex operator +(BlockIndex part1, ChunkIndex part2)
        {
            return new BlockIndex(part1.X + part2.X * Constants.ChunkSize, part1.Y + part2.Y * Constants.ChunkSize, part1.Z + part2.Z * Constants.ChunkSize);
        }

        public static BlockIndex operator -(BlockIndex part1, BlockIndex part2)
        {
            return new BlockIndex(part1.X - part2.X, part1.Y - part2.Y, part1.Z - part2.Z);
        }

        public static BlockIndex operator -(BlockIndex part1, ChunkIndex part2)
        {
            return new BlockIndex(part1.X - part2.X * Constants.ChunkSize, part1.Y - part2.Y * Constants.ChunkSize, part1.Z - part2.Z * Constants.ChunkSize);
        }

        public static BlockIndex operator *(BlockIndex part1, int part2)
        {
            return new BlockIndex(part1.X * part2, part1.Y * part2, part1.Z * part2);
        }

        public static BlockIndex operator /(BlockIndex part1, int part2)
        {
            return new BlockIndex(part1.X / part2, part1.Y / part2, part1.Z / part2);
        }

        public static BlockIndex operator %(BlockIndex part1, int part2)
        {
            return new BlockIndex(part1.X % part2, part1.Y % part2, part1.Z % part2);
        }

        public static BlockIndex operator -(BlockIndex part)
        {
            return BlockIndex.Zero - part;
        }

        public static bool operator ==(BlockIndex part1, BlockIndex part2)
        {
            if (object.Equals(part1, null) != object.Equals(part2, null))
            {
                return false;
            }
            else if (object.Equals(part1, null) || object.Equals(part2, null))
            {
                return true;
            }
            return (part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
        }

        public static bool operator !=(BlockIndex part1, BlockIndex part2)
        {
            if (object.Equals(part1, null) != object.Equals(part2, null))
            {
                return true;
            }
            else if (object.Equals(part1, null) || object.Equals(part2, null))
            {
                return false;
            }
            return !(part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
