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

    public struct Block
    {
        public byte Type { get; private set; }
        public byte Data { get; private set; }

        public Block(byte type, byte data) : this()
        {
            Type = type;
            Data = data;
        }

        static public Block Air { get { return new Block((byte)BlockType.Air, 0); } }
        static public Block Grass { get { return new Block((byte)BlockType.Grass, 0); } }
        static public Block Stone { get { return new Block((byte)BlockType.Stone, 0); } }
        static public Block Dirt { get { return new Block((byte)BlockType.Dirt, 0); } }
        static public Block Water { get { return new Block((byte)BlockType.Water, 0); } }
        static public Block Glass { get { return new Block((byte)BlockType.Glass, 0); } }
        static public Block Bookshelf { get { return new Block((byte)BlockType.Bookshelf, 0); } }
        static public Block Log { get { return new Block((byte)BlockType.Log, 0); } }
        static public Block Wood { get { return new Block((byte)BlockType.Wood, 0); } }
        static public Block Snow { get { return new Block((byte)BlockType.Snow, 0); } }
        static public Block Slab { get { return new Block((byte)BlockType.Slab, 0); } }
        static public Block CraftingTable { get { return new Block((byte)BlockType.CraftingTable, 0); } }
        static public Block Furnace { get { return new Block((byte)BlockType.Furnace, 0); } }
        static public Block Sand { get { return new Block((byte)BlockType.Sand, 0); } }
        static public Block Leaves { get { return new Block((byte)BlockType.Leaves, 0); } }
        static public Block Lava { get { return new Block((byte)BlockType.Lava, 0); } }
        static public Block Brick { get { return new Block((byte)BlockType.Brick, 0); } }
        static public Block Cobblestone { get { return new Block((byte)BlockType.Cobblestone, 0); } }
        static public Block Ice { get { return new Block((byte)BlockType.Ice, 0); } }
        static public Block Vacuum { get { return new Block((byte)BlockType.Vacuum, 0); } }

        public byte[] Bytes { get{ return new byte[] { Type, Data }; } }

        public byte GetFace(Direction direction)
        {
            switch (Type)
            {
                case (byte)BlockType.Air: return 253;                                                            
                case (byte)BlockType.Grass: return new byte[] { 3, 3, 0, 2, 3, 3 }[(byte)direction];  
                //case (byte)BlockType.Grass: return 0;
                case (byte)BlockType.Stone: return 1;                                                              
                case (byte)BlockType.Dirt: return 2;                                                               
                case (byte)BlockType.Water: return 205;                                                            
                case (byte)BlockType.Glass: return 49;                                                              
                case (byte)BlockType.Bookshelf: return new byte[] { 35, 35, 4, 4, 35, 35 }[(byte)direction];            
                case (byte)BlockType.Log: return new byte[] { 20, 20, 21, 21, 20, 20 }[(byte)direction];         
                case (byte)BlockType.Wood: return 4;                                                            
                case (byte)BlockType.Snow: return new byte[] { 68, 68, 66, 2, 68, 68 }[(byte)direction];         
                case (byte)BlockType.Slab: return new byte[] { 5, 5, 6, 6, 5, 5 }[(byte)direction];             
                case (byte)BlockType.CraftingTable: return new byte[] { 60, 60, 43, 43, 59, 59 }[(byte)direction];       
                case (byte)BlockType.Furnace: return new byte[] { 45, 45, 6, 6, 44, 45 }[(byte)direction];          
                case (byte)BlockType.Sand: return 18;                                                            
                case (byte)BlockType.Leaves: return 52;                                                          
                case (byte)BlockType.Lava: return 255;                                                            
                case (byte)BlockType.Brick: return 7;                                                            
                case (byte)BlockType.Cobblestone: return 16; 
                case (byte)BlockType.Ice: return 67;
                case (byte)BlockType.Vacuum: return 17;
                default: return 49;
            }
        }

        public bool Translucency
        {
            get
            {
                switch (Type)
                {
                    case (byte)BlockType.Air: return true;
                    case (byte)BlockType.Water: return true;
                    case (byte)BlockType.Glass: return true;
                    case (byte)BlockType.Leaves: return true;
                    case (byte)BlockType.Ice: return true;
                    case (byte)BlockType.Vacuum: return false;
                    default: return false;
                }
            }
        }

        public bool Solidity
        {
            get
            {
                switch (Type)
                {
                    case (byte)BlockType.Air: return false;
                    case (byte)BlockType.Water: return false;
                    case (byte)BlockType.Lava: return false;
                    default: return true;
                }
            }
        }
        
        public BlockVisibility Visibility
        {
            get
            {
                switch (Type)
                {
                    case (byte)BlockType.Air: return BlockVisibility.Invisible;      
                    case (byte)BlockType.Water: return BlockVisibility.Translucent;    
                    case (byte)BlockType.Glass: return BlockVisibility.Translucent;    
                    case (byte)BlockType.Leaves: return BlockVisibility.Translucent;   
                    case (byte)BlockType.Ice: return BlockVisibility.Translucent;  
                    case (byte)BlockType.Vacuum: return BlockVisibility.Invisible; 
                    default: return BlockVisibility.Opaque;
                }
            }
        }


        public float Viscosity
        {
            get
            {
                switch (Type)
                {
                case (byte)BlockType.Air: return 1.2F;
                case (byte)BlockType.Water: return 1000.0F;
                case (byte)BlockType.Lava: return 2600.0F;
                case (byte)BlockType.Vacuum: return 0.0F;
                default: return 0.0F;
                }
            }
        }

        public float Density
        {
            get
            {
                switch (Type)
                {
                case (byte)BlockType.Air: return 1.225F;
                case (byte)BlockType.Grass: return 1920.0F;
                case (byte)BlockType.Stone: return 2700.0F;
                case (byte)BlockType.Dirt: return 1922.0F;
                case (byte)BlockType.Water: return 1000.0F;
                case (byte)BlockType.Glass: return 2600.0F;
                case (byte)BlockType.Bookshelf: return 500.0F;
                case (byte)BlockType.Log: return 700.0F;
                case (byte)BlockType.Wood: return 20.0F;
                case (byte)BlockType.Snow: return 200.0F;
                case (byte)BlockType.Slab: return 2400.0F;
                case (byte)BlockType.CraftingTable: return 500.0F;
                case (byte)BlockType.Furnace: return 1400.0F;
                case (byte)BlockType.Sand: return 1602.0F;
                case (byte)BlockType.Leaves: return 8.5F;
                case (byte)BlockType.Lava: return 2600.0F;
                case (byte)BlockType.Brick: return 1922.0F;
                case (byte)BlockType.Cobblestone: return 2800.0F;
                case (byte)BlockType.Ice: return 917.0F;
                case (byte)BlockType.Vacuum: return 0.0F;
                default: return float.NaN;
                }
            }
        }

        public float KineticFrictionCoefficient
        {
            get
            {
                switch (Type)
                {
                case (byte)BlockType.Air: return 0.0F;
                case (byte)BlockType.Grass: return 1.0F;
                case (byte)BlockType.Stone: return 1.0F;
                case (byte)BlockType.Dirt: return 1.0F;
                case (byte)BlockType.Water: return 0.0F;
                case (byte)BlockType.Glass: return 0.94F;
                case (byte)BlockType.Bookshelf: return 1.0F;
                case (byte)BlockType.Log: return 1.0F;
                case (byte)BlockType.Wood: return 1.0F;
                case (byte)BlockType.Snow: return 0.8F;
                case (byte)BlockType.Slab: return 1.0F;
                case (byte)BlockType.CraftingTable: return 1.0F;
                case (byte)BlockType.Furnace: return 1.0F;
                case (byte)BlockType.Sand: return 1.3F;
                case (byte)BlockType.Leaves: return 1.2F;
                case (byte)BlockType.Lava: return 0.0F;
                case (byte)BlockType.Brick: return 1.0F;
                case (byte)BlockType.Cobblestone: return 1.0F;
                case (byte)BlockType.Ice: return 0.15F;
                case (byte)BlockType.Vacuum: return 0.0F;
                default: return 0.0F;
                }
            }
        }

        public float GripCoefficient
        {
            get
            {
                switch (Type)
                {
                    case (byte)BlockType.Air: return 0.01F;
                    case (byte)BlockType.Grass: return 1.0F;
                    case (byte)BlockType.Stone: return 1.0F;
                    case (byte)BlockType.Dirt: return 1.0F;
                    case (byte)BlockType.Water: return 0.4F;
                    case (byte)BlockType.Glass: return 0.94F;
                    case (byte)BlockType.Bookshelf: return 1.0F;
                    case (byte)BlockType.Log: return 1.0F;
                    case (byte)BlockType.Wood: return 1.0F;
                    case (byte)BlockType.Snow: return 0.8F;
                    case (byte)BlockType.Slab: return 1.0F;
                    case (byte)BlockType.CraftingTable: return 1.0F;
                    case (byte)BlockType.Furnace: return 1.0F;
                    case (byte)BlockType.Sand: return 0.7F;
                    case (byte)BlockType.Leaves: return 0.8F;
                    case (byte)BlockType.Lava: return 0.1F;
                    case (byte)BlockType.Brick: return 1.0F;
                    case (byte)BlockType.Cobblestone: return 1.0F;
                    case (byte)BlockType.Ice: return 0.05F;
                    case (byte)BlockType.Vacuum: return 0.0F;
                    default: return 0.0F;
                }
            }
        }

        public string Name
        {
            get
            {
                switch (Type)
                {
                case (byte)BlockType.Air: return "Air";
                case (byte)BlockType.Grass: return "Grass";
                case (byte)BlockType.Stone: return "Stone";
                case (byte)BlockType.Dirt: return "Dirt";
                case (byte)BlockType.Water: return "Water";
                case (byte)BlockType.Glass: return "Glass";
                case (byte)BlockType.Bookshelf: return "Bookshelf";
                case (byte)BlockType.Log: return "Log";
                case (byte)BlockType.Wood: return "Wood";
                case (byte)BlockType.Snow: return "Snow";
                case (byte)BlockType.Slab: return "Slab";
                case (byte)BlockType.CraftingTable: return "CraftingTable";
                case (byte)BlockType.Furnace: return "Furnace";
                case (byte)BlockType.Sand: return "Sand";
                case (byte)BlockType.Leaves: return "Leaves";
                case (byte)BlockType.Lava: return "Lava";
                case (byte)BlockType.Brick: return "Brick";
                case (byte)BlockType.Cobblestone: return "Cobblestone";
                case (byte)BlockType.Ice: return "Ice";
                case (byte)BlockType.Vacuum: return "Vacuum";
                default: return "UNUSED";
                }
            }
        }

        static public Block GetFromName(string name)
        {
            switch (name)
            {
                case "air": return Air;
                case "grass": return Grass;
                case "stone": return Stone;
                case "dirt": return Dirt;
                case "water": return Water;
                case "glass": return Glass;
                case "bookshelf": return Bookshelf;
                case "log": return Log;
                case "wood": return Wood;
                case "snow": return Snow;
                case "slab": return Slab;
                case "craftingtable": return CraftingTable;
                case "furnace": return Furnace;
                case "sand": return Sand;
                case "leaves": return Leaves;
                case "lava": return Lava;
                case "brick": return Brick;
                case "cobblestone": return Cobblestone;
                case "ice": return Ice;
                case "vacuum": return Vacuum;
                default: return Air;
            }
        }
    }

    public enum BlockType : byte
    {
        Air = 0,
        Grass = 1,
        Stone = 2,
        Dirt = 3,
        Water = 4,
        Glass = 5,
        Bookshelf = 6,
        Log = 7,
        Wood = 8,
        Snow = 9,
        Slab = 10,
        CraftingTable = 11,
        Furnace = 12,
        Sand = 13,
        Leaves = 14,
        Lava = 15,
        Brick = 16,
        Cobblestone = 17,
        Ice = 18,
        Vacuum = 255
    }
}
