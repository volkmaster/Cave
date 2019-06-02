using System;

namespace WorldGenerator {
    [Serializable]
    public class BlockGrass : Block {
        public override string Description { get; } = "Grass";

        public BlockGrass() : base() { }

        protected override Tile TilePosition(Direction direction) {
            switch (direction) {
                case Direction.up:
                    return new Tile { x = 2, y = 0 };
                case Direction.down:
                    return new Tile { x = 1, y = 0 };
                default:
                    return new Tile { x = 3, y = 0 };
            }
        }
    }
}
