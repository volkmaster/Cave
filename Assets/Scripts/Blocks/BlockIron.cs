using System;

namespace WorldGenerator {
    [Serializable]
    public class BlockIron : Block {
        public override string Description { get; } = "Iron";

        public BlockIron() : base() { }

        protected override Tile TilePosition(Direction direction) {
            return new Tile { x = 0, y = 2 };
        }
    }
}
