using System;

namespace WorldGenerator {
    [Serializable]
    public class BlockCoal : Block {
        public override string Description { get; } = "Coal";

        public BlockCoal() : base() { }

        protected override Tile TilePosition(Direction direction) {
            return new Tile { x = 0, y = 3 };
        }
    }
}
