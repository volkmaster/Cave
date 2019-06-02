using System;

namespace WorldGenerator {
    [Serializable]
    public class BlockStone : Block {
        public override string Description { get; } = "Stone";

        public BlockStone() : base() { }

        protected override Tile TilePosition(Direction direction) {
            return new Tile { x = 0, y = 0 };
        }
    }
}
