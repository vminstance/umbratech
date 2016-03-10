# Introduction #

The rendering is done in several steps:
  * Load or generate chunk data.
  * Build an octree.
  * Use the octree to generate faces.
  * Loop through each chunk and render it's vertexbuffer.
  * Draw overlays.

---


# Details #

The chunk data is stored in .cnk files. Octrees are made recursivly from the bottom.

---