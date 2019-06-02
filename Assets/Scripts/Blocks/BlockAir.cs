using System;

namespace WorldGenerator {
    [Serializable]
    public class BlockAir : Block {
        public override string Description { get; } = "Air";

        public BlockAir() : base() { }

        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.UseRenderDataForCollider = false;

            return meshData;
        }

        protected override bool IsSolid(Direction direction) {
            return false;
        }
    }
}