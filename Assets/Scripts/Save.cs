using System;
using System.Collections.Generic;

namespace WorldGenerator {
    [Serializable]
    public class Save {
        public Dictionary<WorldPosition, Block> Blocks { get; } = new Dictionary<WorldPosition, Block>();

        public Save(Chunk chunk) {
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int y = 0; y < Chunk.SIZE; y++) {
                    for (int z = 0; z < Chunk.SIZE; z++) {
                        if (!chunk.Blocks[x, y, z].Changed) {
                            continue;
                        }

                        WorldPosition position = new WorldPosition(x, y, z);
                        Blocks.Add(position, chunk.Blocks[x, y, z]);
                    }
                }
            }
        }
    }
}
