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

    static public class Block
    {
        static public ushort Air { get{ return BitConverter.ToUInt16(new byte[] { 0, 0 }, 0); } }
        static public ushort Grass { get { return BitConverter.ToUInt16(new byte[] { 1, 0 }, 0); } }
        static public ushort Stone { get { return BitConverter.ToUInt16(new byte[] { 2, 0 }, 0); } }
        static public ushort Dirt { get { return BitConverter.ToUInt16(new byte[] { 3, 0 }, 0); } }
        static public ushort Water { get { return BitConverter.ToUInt16(new byte[] { 4, 0 }, 0); } }
        static public ushort Glass { get { return BitConverter.ToUInt16(new byte[] { 5, 0 }, 0); } }
        static public ushort Bookshelf { get { return BitConverter.ToUInt16(new byte[] { 6, 0 }, 0); } }
        static public ushort Log { get { return BitConverter.ToUInt16(new byte[] { 7, 0 }, 0); } }
        static public ushort Wood { get { return BitConverter.ToUInt16(new byte[] { 8, 0 }, 0); } }
        static public ushort Snow { get { return BitConverter.ToUInt16(new byte[] { 9, 0 }, 0); } }
        static public ushort Halfstep { get { return BitConverter.ToUInt16(new byte[] { 10, 0 }, 0); } }
        static public ushort CraftingTable { get { return BitConverter.ToUInt16(new byte[] { 11, 0 }, 0); } }
        static public ushort Furnace { get { return BitConverter.ToUInt16(new byte[] { 12, 0 }, 0); } }
        static public ushort Sand { get { return BitConverter.ToUInt16(new byte[] { 13, 0 }, 0); } }
        static public ushort Leaves { get { return BitConverter.ToUInt16(new byte[] { 14, 0 }, 0); } }
        static public ushort Lava { get { return BitConverter.ToUInt16(new byte[] { 15, 0 }, 0); } }
        static public ushort Brick { get { return BitConverter.ToUInt16(new byte[] { 16, 0 }, 0); } }
        static public ushort Cobblestone { get { return BitConverter.ToUInt16(new byte[] { 17, 0 }, 0); } }
        static public ushort Ice { get { return BitConverter.ToUInt16(new byte[] { 18, 0 }, 0); } }
        static public ushort Vacuum { get { return BitConverter.ToUInt16(new byte[] { 255, 0 }, 0); } }

        static public ushort Create(byte type, byte data)
        {
            return BitConverter.ToUInt16(new byte[] { type, data }, 0);
        }

        static public byte[] GetBytes(ushort block)
        {
            byte type = BitConverter.GetBytes(block)[0];
            byte data = BitConverter.GetBytes(block)[1];
            return new byte[] { type, data };
        }

        static public byte GetType(ushort block)
        {
            return BitConverter.GetBytes(block)[0];
        }

        static public byte GetData(ushort block)
        {
            return BitConverter.GetBytes(block)[1];
        }

        static public byte GetFace(ushort block, Direction direction)
        {
            byte type = GetType(block);

            switch (type)
            {
                case 0: return 253;                                                             // Air
                case 1: return new byte[] { 3, 3, 0, 2, 3, 3 }[(byte)direction];                // Grass
                case 2: return 1;                                                               // Stone
                case 3: return 2;                                                               // Dirt
                case 4: return 205;                                                             // Water
                case 5: return 49;                                                              // Glass
                case 6: return new byte[] { 35, 35, 4, 4, 35, 35 }[(byte)direction];            // Bookshelf
                case 7: return new byte[] { 20, 20, 21, 21, 20, 20 }[(byte)direction];          // Log
                case 8: return 4;                                                               // Wood
                case 9: return new byte[] { 68, 68, 66, 2, 68, 68 }[(byte)direction];           // Snow
                case 10: return new byte[] { 5, 5, 6, 6, 5, 5 }[(byte)direction];               // Halfstep
                case 11: return new byte[] { 60, 60, 43, 43, 59, 59 }[(byte)direction];         // CraftingTable
                case 12: return new byte[] { 45, 45, 6, 6, 44, 45 }[(byte)direction];           // Furnace
                case 13: return 18;                                                             // Sand
                case 14: return 52;                                                             // Leaves
                case 15: return 255;                                                            // Lava
                case 16: return 7;                                                              // Brick
                case 17: return 16;                                                             // Cobblestone
                case 18: return 67;                                                             // Ice
                case 255: return 17;                                                            // Vacuum
                default: return 49;
            }
        }

        static public bool IsTranslucent(ushort block)
        {
            byte type = GetType(block);

            switch (type)
            {
                case 0: return true;    // Air
                case 4: return true;    // Water
                case 5: return true;    // Glass
                case 18: return true;   // Ice
                case 14: return true;   // Leaves
                case 255: return false; // Vacuum
                default: return false;
            }
        }

        static public bool IsVisible(ushort block)
        {
            byte type = GetType(block);

            switch (type)
            {
                case 0: return false;   // Air
                case 255: return true;  // Vacuum
                default: return true;
            }
        }

        static public bool IsSolid(ushort block)
        {
            byte type = GetType(block);

            switch (type)
            {
                case 0: return false;   // Air
                case 4: return false;   // Water
                case 15: return false;  // Lava
                default: return true;
            }
        }

        static public BlockVisibility GetVisibility(ushort block)
        {
            byte type = GetType(block);

            switch (type)
            {
                case 0: return BlockVisibility.Invisible;       // Air
                case 4: return BlockVisibility.Translucent;     // Water
                case 5: return BlockVisibility.Translucent;     // Glass
                case 14: return BlockVisibility.Translucent;    // Leaves
                case 18: return BlockVisibility.Translucent;    // Ice
                case 255: return BlockVisibility.Invisible;     // Vacuum
                default: return BlockVisibility.Opaque;
            }
        }

        static public string GetNameFromBlock(ushort block)
        {
            byte type = GetType(block);

            switch (type)
            {
                case 0: return "Air";
                case 1: return "Grass";
                case 2: return "Stone";
                case 3: return "Dirt";
                case 4: return "Water";
                case 5: return "Glass";
                case 6: return "Bookshelf";
                case 7: return "Log";
                case 8: return "Wood";
                case 9: return "Snow";
                case 10: return "Slab";
                case 11: return "Crafting Table";
                case 12: return "Furnace";
                case 13: return "Sand";
                case 14: return "Leaves";
                case 15: return "Lava";
                case 16: return "Brick";
                case 17: return "Cobblestone";
                case 18: return "Ice";
                case 255: return "Vacuum";
                default: return "UNUSED";
            }
        }

        static public ushort GetBlockFromName(string name)
        {
            switch (name)
            {
                case "air": return 0;
                case "grass": return 1;
                case "stone": return 2;
                case "dirt": return 3;
                case "water": return 4;
                case "glass": return 5;
                case "bookshelf": return 6;
                case "log": return 7;
                case "wood": return 8;
                case "snow": return 9;
                case "slab": return 10;
                case "craftingtable": return 11;
                case "furnace": return 12;
                case "sand": return 13;
                case "leaves": return 14;
                case "lava": return 15;
                case "brick": return 16;
                case "cobblestone": return 17;
                case "ice": return 18;
                case "vacuum": return 255;
                default: return 0;
            }
        }
    }
}
