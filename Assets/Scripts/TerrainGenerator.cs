using UnityEngine;

namespace WorldGenerator {
    public class TerrainGenerator {
        public int Seed { get; set; } = 0;

        private const float stoneBaseHeight = -24; // height that we'll start from
        private const float stoneBaseNoise = 0.05f; // makes peaks around 25 blocks apart
        private const float stoneBaseNoiseHeight = 4; // max difference between peak and valley is 4

        private const float stoneMountainHeight = 48; // greater height
        private const float stoneMountainFrequency = 0.008f; // much smaller frequency value
        private const float stoneMinHeight = -12; // the lowest stone is allowed to go

        private const float dirtBaseHeight = 1; // minimum depth on top of the rock 
        private const float dirtNoise = 0.04f;
        private const float dirtNoiseHeight = 3;

        public void GenerateChunk(ref Chunk chunk) {
            Noise.Seed = Seed;

            for (int x = chunk.Position.X; x < chunk.Position.X + Chunk.SIZE; x++) {
                for (int z = chunk.Position.Z; z < chunk.Position.Z + Chunk.SIZE; z++) {
                    GenerateChunkColumn(ref chunk, x, z);
                }
            }
        }

        private void GenerateChunkColumn(ref Chunk chunk, int x, int z) {
            // We create a stone height variable set to the base height and add the mountain noise
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
            stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

            // raise everything under the minimum to the minimum
            if (stoneHeight < stoneMinHeight) {
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);
            }

            // apply the base noise
            stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

            // we make dirt height variable equal to the stone height plus the base dirt height and add the dirt noise on top
            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
            dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

            // we cycle through every chunk in the column adding the block that matches
            for (int y = chunk.Position.Y; y < chunk.Position.Y + Chunk.SIZE; y++) {
                if (y <= stoneHeight) {
                    chunk.SetBlock(x - chunk.Position.X, y - chunk.Position.Y, z - chunk.Position.Z, new BlockStone());
                } else if (y <= dirtHeight) {
                    chunk.SetBlock(x - chunk.Position.X, y - chunk.Position.Y, z - chunk.Position.Z, new BlockGrass());
                } else {
                    chunk.SetBlock(x - chunk.Position.X, y - chunk.Position.Y, z - chunk.Position.Z, new BlockAir());
                }
            }
        }

        public static int GetNoise(int x, int y, int z, float scale, int max) {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
