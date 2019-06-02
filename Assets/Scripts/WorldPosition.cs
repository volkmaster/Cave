using System;

namespace WorldGenerator {
    [Serializable]
    public struct WorldPosition {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public WorldPosition(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj) {
            if (!(obj is WorldPosition)) {
                return false;
            }

            WorldPosition worldPosition = (WorldPosition)obj;
            return worldPosition.X == X & worldPosition.Y == Y && worldPosition.Z == Z;
        }

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
    }
}
