using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
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

        static string GetChunkPath(ChunkIndex index)
        {
            return Constants.CurrentWorld.Path + Constants.ChunkFilePath + index.ToString() + Constants.ChunkFileExtension;
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

            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.ChunkSize; y++)
                {
                    for (int z = 0; z < Constants.ChunkSize; z++)
                    {
                        chunk[x, y, z] = Block.Create((byte)stream.ReadByte(), (byte)stream.ReadByte());
                    }
                }
            }

            stream.Close();

            chunk.HasData = true;
        }

        static public void UnloadChunk(Chunk chunk)
        {
            if (Constants.SaveChunksDynamic)
            {
                Setup.AddToUnload(chunk);
            }
        }


        static public void StoreChunkImmediate(Chunk chunk)
        {
            if(!IsChunkOnDisk(chunk.Index))
            {
                File.Create(GetChunkPath(chunk.Index));
            }

            FileStream stream = File.Open(GetChunkPath(chunk.Index), FileMode.Truncate);

            chunk.DisposeBuffers();

            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.ChunkSize; y++)
                {
                    for (int z = 0; z < Constants.ChunkSize; z++)
                    {
                        stream.Write(Block.GetBytes(chunk[x, y, z]), 0, 2);
                    }
                }
            }

            stream.Close();
        }
    }
}
