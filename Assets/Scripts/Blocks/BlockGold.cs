using System;

namespace WorldGenerator {
    [Serializable]
    public class BlockGold : Block {
        public override string Description { get; } = "Gold";

        public BlockGold() : base() { }

        protected override Tile TilePosition(Direction direction) {
            return new Tile { x = 3, y = 3 };
        }
    }
}
