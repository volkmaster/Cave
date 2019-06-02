﻿using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerator {
    public class World : MonoBehaviour {
        public string Name { get; } = "World";

        public GameObject chunkPrefab;
        private Dictionary<WorldPosition, Chunk> chunks = new Dictionary<WorldPosition, Chunk>();

        public bool generateChunk = false;
        public int newChunkX;
        public int newChunkY;
        public int newChunkZ;

        // Start is called before the first frame update
        void Start() {
            for (int x = -2; x < 2; x++) {
                for (int y = -1; y < 1; y++) {
                    for (int z = -1; z < 1; z++) {
                        CreateChunk(x * Chunk.SIZE, y * Chunk.SIZE, z * Chunk.SIZE);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update() {
            if (generateChunk) {
                generateChunk = false;

                WorldPosition position = new WorldPosition(newChunkX, newChunkY, newChunkZ);
                if (chunks.TryGetValue(position, out Chunk chunk)) {
                    DestroyChunk(position.X, position.Y, position.Z);
                } else {
                    CreateChunk(position.X, position.Y, position.Z);
                }
            }
        }

        public void CreateChunk(int x, int y, int z) {
            WorldPosition position = new WorldPosition(x, y, z);

            // Instantiate the chunk at the coordinates using the chunk prefab
            GameObject chunkObject = Instantiate(
                chunkPrefab,
                new Vector3(x, y, z),
                Quaternion.Euler(Vector3.zero)
            ) as GameObject;

            Chunk chunk = chunkObject.GetComponent<Chunk>();
            chunk.World = this;
            chunk.Position = position;

            // Add it to the chunks dictionary with the position as the key
            chunks.Add(position, chunk);

            for (int xi = 0; xi < Chunk.SIZE; xi++) {
                for (int yi = 0; yi < Chunk.SIZE; yi++) {
                    for (int zi = 0; zi < Chunk.SIZE; zi++) {
                        if (yi < Chunk.SIZE / 2) {
                            SetBlock(x + xi, y + yi, z + zi, new BlockGrass());
                        } else {
                            SetBlock(x + xi, y + yi, z + zi, new BlockAir());
                        }
                    }
                }
            }

            chunk.SetBlocksUnmodified();
            Serialization.LoadChunk(chunk);
        }

        public Chunk GetChunk(int x, int y, int z) {
            WorldPosition position = new WorldPosition(
                Mathf.FloorToInt(x / (float)Chunk.SIZE) * Chunk.SIZE,
                Mathf.FloorToInt(y / (float)Chunk.SIZE) * Chunk.SIZE,
                Mathf.FloorToInt(z / (float)Chunk.SIZE) * Chunk.SIZE
            );
            chunks.TryGetValue(position, out Chunk chunk);
            return chunk;
        }

        public void DestroyChunk(int x, int y, int z) {
            WorldPosition position = new WorldPosition(x, y, z);
            if (chunks.TryGetValue(position, out Chunk chunk)) {
                Serialization.SaveChunk(chunk);
                Destroy(chunk.gameObject);
                chunks.Remove(position);
            }
        }

        public Block GetBlock(int x, int y, int z) {
            Chunk chunk = GetChunk(x, y, z);
            return chunk != null
                ? chunk.GetBlock(x - chunk.Position.X, y - chunk.Position.Y, z - chunk.Position.Z)
                : new BlockAir();
        }

        public void SetBlock(int x, int y, int z, Block block) {
            Chunk chunk = GetChunk(x, y, z);
            if (chunk != null) {
                chunk.SetBlock(x - chunk.Position.X, y - chunk.Position.Y, z - chunk.Position.Z, block);
                chunk.DoUpdate = true;

                // Update bordering chunks if necessary.
                UpdateIfEqual(x - chunk.Position.X, 0,
                    new WorldPosition(x - 1, y, z));
                UpdateIfEqual(x - chunk.Position.X, Chunk.SIZE - 1,
                    new WorldPosition(x + 1, y, z));
                UpdateIfEqual(y - chunk.Position.Y, 0,
                    new WorldPosition(x, y - 1, z));
                UpdateIfEqual(y - chunk.Position.Y, Chunk.SIZE - 1,
                    new WorldPosition(x, y + 1, z));
                UpdateIfEqual(z - chunk.Position.Z, 0,
                    new WorldPosition(x, y, z - 1));
                UpdateIfEqual(z - chunk.Position.Z, Chunk.SIZE - 1,
                    new WorldPosition(x, y, z + 1));
            }
        }

        private void UpdateIfEqual(int value1, int value2, WorldPosition position) {
            if (value1 == value2) {
                Chunk chunk = GetChunk(position.X, position.Y, position.Z);
                if (chunk != null) {
                    chunk.DoUpdate = true;
                }
            }
        }
    }
}