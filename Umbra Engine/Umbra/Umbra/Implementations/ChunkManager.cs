using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    static public class ChunkManager
    {
        static Thread SetupThread;
        static public SetupThread Setup;

        static public void InitializeThreads()
        {
            Setup = new SetupThread();
            SetupThread = new Thread(new ThreadStart(Setup.Run));
            SetupThread.Start();
        }

        static public void AbortThreads()
        {
            SetupThread.Abort();
        }

        static string GetChunkPath()
        {
            return Variables.World.Current.Path + Constants.Content.Data.ChunkFilePath;
        }

        static string GetChunkPath(ChunkIndex index)
        {
            return Variables.World.Current.Path + Constants.Content.Data.ChunkFilePath + @"\" + index.ToString() + Constants.Content.Data.ChunkFileExtension;
        }

        static bool IsChunkOnDisk(ChunkIndex index)
        {
            return File.Exists(GetChunkPath(index));
        }

        static public Chunk ObtainChunk(ChunkIndex index)
        {
            Chunk chunk = new Chunk(index);

            if (IsChunkOnDisk(index))
            {
                Setup.AddToLoad(chunk);
            }
            else
            {
                Setup.AddToGeneration(chunk);
            }

            return chunk;
        }

        static public void LoadChunkImmediate(Chunk chunk)
        {
            if (!IsChunkOnDisk(chunk.Index))
            {
                return;
            }

            FileStream stream = File.Open(GetChunkPath(chunk.Index), FileMode.Open);

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.World.ChunkSize; y++)
                {
                    for (int z = 0; z < Constants.World.ChunkSize; z++)
                    {
                        chunk[x, y, z] = new Block((byte)stream.ReadByte(), (byte)stream.ReadByte());
                    }
                }
            }

            stream.Close();
        }

        static public void UnloadChunk(Chunk chunk)
        {
            if (Constants.World.SaveDynamicWorld)
            {
                Setup.AddToUnload(chunk);
            }
        }


        static public void StoreChunkImmediate(Chunk chunk)
        {
            if (!Directory.Exists(GetChunkPath()))
            {
                Directory.CreateDirectory(GetChunkPath());
            }

            FileStream stream = File.Open(GetChunkPath(chunk.Index), FileMode.Create);

            chunk.DisposeBuffers();

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.World.ChunkSize; y++)
                {
                    for (int z = 0; z < Constants.World.ChunkSize; z++)
                    {
                        stream.Write(chunk[x, y, z].Bytes, 0, 2);
                    }
                }
            }

            stream.Close();
        }
    }
}
