using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerator {
    public class ChunkLoader : MonoBehaviour {
        public World world;
        private readonly List<WorldPosition> buildList = new List<WorldPosition>();
        private readonly List<WorldPosition> updateList = new List<WorldPosition>();
        // Chunk positions radiating from position (0, 0, 0) sorted by closeness to the center
        private static readonly WorldPosition[] ChunkPositions = {
            new WorldPosition(0, 0, 0), new WorldPosition(-1, 0, 0), new WorldPosition(0, 0, -1), new WorldPosition(0, 0, 1), new WorldPosition(1, 0, 0),
            new WorldPosition(-1, 0, -1), new WorldPosition(-1, 0, 1), new WorldPosition(1, 0, -1), new WorldPosition(1, 0, 1), new WorldPosition(-2, 0, 0),
            new WorldPosition(0, 0, -2), new WorldPosition(0, 0, 2), new WorldPosition(2, 0, 0), new WorldPosition(-2, 0, -1), new WorldPosition(-2, 0, 1),
            new WorldPosition(-1, 0, -2), new WorldPosition(-1, 0, 2), new WorldPosition(1, 0, -2), new WorldPosition(1, 0, 2), new WorldPosition(2, 0, -1),
            new WorldPosition(2, 0, 1), new WorldPosition(-2, 0, -2), new WorldPosition(-2, 0, 2), new WorldPosition(2, 0, -2), new WorldPosition(2, 0, 2),
            new WorldPosition(-3, 0, 0), new WorldPosition(0, 0, -3), new WorldPosition(0, 0, 3), new WorldPosition(3, 0, 0), new WorldPosition(-3, 0, -1),
            new WorldPosition(-3, 0, 1), new WorldPosition(-1, 0, -3), new WorldPosition(-1, 0, 3), new WorldPosition(1, 0, -3), new WorldPosition(1, 0, 3),
            new WorldPosition(3, 0, -1), new WorldPosition(3, 0, 1), new WorldPosition(-3, 0, -2), new WorldPosition(-3, 0, 2), new WorldPosition(-2, 0, -3),
            new WorldPosition(-2, 0, 3), new WorldPosition(2, 0, -3), new WorldPosition(2, 0, 3), new WorldPosition(3, 0, -2), new WorldPosition(3, 0, 2),
            new WorldPosition(-4, 0, 0), new WorldPosition(0, 0, -4), new WorldPosition(0, 0, 4), new WorldPosition(4, 0, 0), new WorldPosition(-4, 0, -1),
            new WorldPosition(-4, 0, 1), new WorldPosition(-1, 0, -4), new WorldPosition(-1, 0, 4), new WorldPosition(1, 0, -4), new WorldPosition(1, 0, 4),
            new WorldPosition(4, 0, -1), new WorldPosition(4, 0, 1), new WorldPosition(-3, 0, -3), new WorldPosition(-3, 0, 3), new WorldPosition(3, 0, -3),
            new WorldPosition(3, 0, 3), new WorldPosition(-4, 0, -2), new WorldPosition(-4, 0, 2), new WorldPosition(-2, 0, -4), new WorldPosition(-2, 0, 4),
            new WorldPosition(2, 0, -4), new WorldPosition(2, 0, 4), new WorldPosition(4, 0, -2), new WorldPosition(4, 0, 2), new WorldPosition(-5, 0, 0),
            new WorldPosition(-4, 0, -3), new WorldPosition(-4, 0, 3), new WorldPosition(-3, 0, -4), new WorldPosition(-3, 0, 4), new WorldPosition(0, 0, -5),
            new WorldPosition(0, 0, 5), new WorldPosition(3, 0, -4), new WorldPosition(3, 0, 4), new WorldPosition(4, 0, -3), new WorldPosition(4, 0, 3),
            new WorldPosition(5, 0, 0), new WorldPosition(-5, 0, -1), new WorldPosition(-5, 0, 1), new WorldPosition(-1, 0, -5), new WorldPosition(-1, 0, 5),
            new WorldPosition(1, 0, -5), new WorldPosition(1, 0, 5), new WorldPosition(5, 0, -1), new WorldPosition(5, 0, 1), new WorldPosition(-5, 0, -2),
            new WorldPosition(-5, 0, 2), new WorldPosition(-2, 0, -5), new WorldPosition(-2, 0, 5), new WorldPosition(2, 0, -5), new WorldPosition(2, 0, 5),
            new WorldPosition(5, 0, -2), new WorldPosition(5, 0, 2), new WorldPosition(-4, 0, -4), new WorldPosition(-4, 0, 4), new WorldPosition(4, 0, -4),
            new WorldPosition(4, 0, 4), new WorldPosition(-5, 0, -3), new WorldPosition(-5, 0, 3), new WorldPosition(-3, 0, -5), new WorldPosition(-3, 0, 5),
            new WorldPosition(3, 0, -5), new WorldPosition(3, 0, 5), new WorldPosition(5, 0, -3), new WorldPosition(5, 0, 3), new WorldPosition(-6, 0, 0),
            new WorldPosition(0, 0, -6), new WorldPosition(0, 0, 6), new WorldPosition(6, 0, 0), new WorldPosition(-6, 0, -1), new WorldPosition(-6, 0, 1),
            new WorldPosition(-1, 0, -6), new WorldPosition(-1, 0, 6), new WorldPosition(1, 0, -6), new WorldPosition(1, 0, 6), new WorldPosition(6, 0, -1),
            new WorldPosition(6, 0, 1), new WorldPosition(-6, 0, -2), new WorldPosition(-6, 0, 2), new WorldPosition(-2, 0, -6), new WorldPosition(-2, 0, 6),
            new WorldPosition(2, 0, -6), new WorldPosition(2, 0, 6), new WorldPosition(6, 0, -2), new WorldPosition(6, 0, 2), new WorldPosition(-5, 0, -4),
            new WorldPosition(-5, 0, 4), new WorldPosition(-4, 0, -5), new WorldPosition(-4, 0, 5), new WorldPosition(4, 0, -5), new WorldPosition(4, 0, 5),
            new WorldPosition(5, 0, -4), new WorldPosition(5, 0, 4), new WorldPosition(-6, 0, -3), new WorldPosition(-6, 0, 3), new WorldPosition(-3, 0, -6),
            new WorldPosition(-3, 0, 6), new WorldPosition(3, 0, -6), new WorldPosition(3, 0, 6), new WorldPosition(6, 0, -3), new WorldPosition(6, 0, 3),
            new WorldPosition(-7, 0, 0), new WorldPosition(0, 0, -7), new WorldPosition(0, 0, 7), new WorldPosition(7, 0, 0), new WorldPosition(-7, 0, -1),
            new WorldPosition(-7, 0, 1), new WorldPosition(-5, 0, -5), new WorldPosition(-5, 0, 5), new WorldPosition(-1, 0, -7), new WorldPosition(-1, 0, 7),
            new WorldPosition(1, 0, -7), new WorldPosition(1, 0, 7), new WorldPosition(5, 0, -5), new WorldPosition(5, 0, 5), new WorldPosition(7, 0, -1),
            new WorldPosition(7, 0, 1), new WorldPosition(-6, 0, -4), new WorldPosition(-6, 0, 4), new WorldPosition(-4, 0, -6), new WorldPosition(-4, 0, 6),
            new WorldPosition(4, 0, -6), new WorldPosition(4, 0, 6), new WorldPosition(6, 0, -4), new WorldPosition(6, 0, 4), new WorldPosition(-7, 0, -2),
            new WorldPosition(-7, 0, 2), new WorldPosition(-2, 0, -7), new WorldPosition(-2, 0, 7), new WorldPosition(2, 0, -7), new WorldPosition(2, 0, 7),
            new WorldPosition(7, 0, -2), new WorldPosition(7, 0, 2), new WorldPosition(-7, 0, -3), new WorldPosition(-7, 0, 3), new WorldPosition(-3, 0, -7),
            new WorldPosition(-3, 0, 7), new WorldPosition(3, 0, -7), new WorldPosition(3, 0, 7), new WorldPosition(7, 0, -3), new WorldPosition(7, 0, 3),
            new WorldPosition(-6, 0, -5), new WorldPosition(-6, 0, 5), new WorldPosition(-5, 0, -6), new WorldPosition(-5, 0, 6), new WorldPosition(5, 0, -6),
            new WorldPosition(5, 0, 6), new WorldPosition(6, 0, -5), new WorldPosition(6, 0, 5), new WorldPosition(-7, 0, -4), new WorldPosition(-7, 0, 4),
            new WorldPosition(-4, 0, -7), new WorldPosition(-4, 0, 7), new WorldPosition(4, 0, -7), new WorldPosition(4, 0, 7), new WorldPosition(7, 0, -4),
            new WorldPosition(7, 0, 4), new WorldPosition(-6, 0, -6), new WorldPosition(-6, 0, 6), new WorldPosition(6, 0, -6), new WorldPosition(6, 0, 6),
            new WorldPosition(-7, 0, -5), new WorldPosition(-7, 0, 5), new WorldPosition(-5, 0, -7), new WorldPosition(-5, 0, 7), new WorldPosition(5, 0, -7),
            new WorldPosition(5, 0, 7), new WorldPosition(7, 0, -5), new WorldPosition(7, 0, 5), new WorldPosition(-7, 0, -6), new WorldPosition(-7, 0, 6),
            new WorldPosition(-6, 0, -7), new WorldPosition(-6, 0, 7), new WorldPosition(6, 0, -7), new WorldPosition(6, 0, 7), new WorldPosition(7, 0, -6),
            new WorldPosition(7, 0, 6), new WorldPosition(-7, 0, -7), new WorldPosition(-7, 0, 7), new WorldPosition(7, 0, -7), new WorldPosition(7, 0, 7)
        };
        private const int DELETE_FREQUENCY = 10;
        private const float DELETE_DISTANCE = 256f;
        private int deleteCounter = 0;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            DeleteChunks();
            FindChunksToLoad();
            LoadAndRenderChunks();
        }

        private void DeleteChunks() {
            if (deleteCounter == DELETE_FREQUENCY) {
                List<WorldPosition> chunksToDelete = new List<WorldPosition>();
                foreach (var chunk in world.Chunks) {
                    float distance = Vector3.Distance(
                        new Vector3(chunk.Value.Position.X, 0, chunk.Value.Position.Z),
                        new Vector3(transform.position.x, 0, transform.position.z)
                    );

                    if (distance > DELETE_DISTANCE) {
                        chunksToDelete.Add(chunk.Key);
                    }
                }

                foreach (var chunk in chunksToDelete) {
                    world.DestroyChunk(chunk.X, chunk.Y, chunk.Z);
                }
                chunksToDelete.Clear();

                deleteCounter = 0;
            }

            deleteCounter++;
        }

        private void FindChunksToLoad() {
            // Get the position of this game object to generate around it
            WorldPosition playerPosition = new WorldPosition(
                Mathf.FloorToInt(transform.position.x / Chunk.SIZE) * Chunk.SIZE,
                Mathf.FloorToInt(transform.position.y / Chunk.SIZE) * Chunk.SIZE,
                Mathf.FloorToInt(transform.position.z / Chunk.SIZE) * Chunk.SIZE
            );

            // If no chunks to generate exist yet
            if (buildList.Count == 0) {
                // Cycle through the array of positions
                foreach (var chunkPosition in ChunkPositions) {
                    // Translate the player position and array position into chunk position
                    WorldPosition newChunkPosition = new WorldPosition(
                        chunkPosition.X * Chunk.SIZE + playerPosition.X,
                        0,
                        chunkPosition.Z * Chunk.SIZE + playerPosition.Z
                    );

                    // Get the chunk in the defined position
                    Chunk newChunk = world.GetChunk(newChunkPosition.X, newChunkPosition.Y, newChunkPosition.Z);

                    // If the chunk already exists and it's already rendered or in queue to be rendered, continue
                    if (newChunk != null && (newChunk.Rendered || updateList.Contains(chunkPosition))) {
                        continue;
                    }

                    // Load a column of chunks in this position
                    for (int y = -Chunk.SIZE / 4; y < Chunk.SIZE / 4; y++) {
                        buildList.Add(new WorldPosition(newChunkPosition.X, y * Chunk.SIZE, newChunkPosition.Z));
                    }

                    // Return, so that only one column is added to the list
                    return;
                }
            }
        }

        private void LoadAndRenderChunks() {
            // Loops through up to 4 chunks to be built and builds them.
            // We can limit how intensive this is per frame by changing the for loop around the build list
            // and the call to BuildChunk to iterate more or fewer times per frame.
            for (int i = 0; i < 4; i++) {
                if (buildList.Count != 0) {
                    BuildChunk(buildList[0]);
                    buildList.RemoveAt(0);
                }
            }

            // Loops through all the chunks to update and updates them.
            // There's no reason to limit how many of these to do per frame, because there will only ever be
            // one to update per chunk built so we can limit the chunks built instead.
            for (int i = 0; i < updateList.Count; i++) {
                Chunk chunk = world.GetChunk(updateList[0].X, updateList[0].Y, updateList[0].Z);
                if (chunk != null) {
                    chunk.DoUpdate = true;
                }
                updateList.RemoveAt(0);
            }
        }

        // When rendering a chunk we need to have all the neighbouring chunks loaded, so that the blocks on the edge of the chunk
        // being rendered can get block information on the blocks next to them in the neighbouring chunk.
        // If we don't load the neighbouring chunks the blocks have to guess if the neighbour is solid or not:
        // - if solid we will make a lot of errors where the rendering assumes the face can't be seen
        // - if not solid we will render a lot of block faces that are actually hidden, especially underground
        // Having the neighbours loaded but not rendered solves this problem.
        private void BuildChunk(WorldPosition position) {
            for (int y = position.Y - Chunk.SIZE; y <= position.Y + Chunk.SIZE; y += Chunk.SIZE) {
                if (y < -Chunk.SIZE * 4 || y > Chunk.SIZE * 4) {
                    continue;
                }

                for (int x = position.X - Chunk.SIZE; x <= position.X + Chunk.SIZE; x += Chunk.SIZE) {
                    for (int z = position.Z - Chunk.SIZE; z <= position.Z + Chunk.SIZE; z += Chunk.SIZE) {
                        if (world.GetChunk(x, y, z) == null) {
                            world.CreateChunk(x, y, z);
                        }
                    }
                }
            }

            updateList.Add(position);
        }
    }
}