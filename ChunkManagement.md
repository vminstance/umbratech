# Introduction #

Chunks are managed by `Implementations/ChunkManager.cs`

---


# Details #

Chunks can be in one of the following states:
  * Non-existing - The chunk has never been generated.
  * Unloaded - The chunk is stored on the disc, nothing is in memory.
  * To setup - The chunk is loaded to memory, but dont have a vertexbuffer.
  * Loaded - The chunk is ready with data, octree and a vertexbuffer.

---