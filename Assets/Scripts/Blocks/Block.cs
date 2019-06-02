using System;
using UnityEngine;

namespace WorldGenerator {
    [Serializable]
    public class Block {
        public virtual string Description { get; } = "Any";

        protected enum Direction { north, east, south, west, up, down };
        protected struct Tile { public int x; public int y; }
        private const float TILE_SIZE = 0.25f;

        public bool Changed { get; set; } = true;

        public Block() { }

        public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.UseRenderDataForCollider = true;

            if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down)) {
                meshData = FaceDataUp(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up)) {
                meshData = FaceDataDown(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south)) {
                meshData = FaceDataNorth(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north)) {
                meshData = FaceDataSouth(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west)) {
                meshData = FaceDataEast(chunk, x, y, z, meshData);
            }

            if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east)) {
                meshData = FaceDataWest(chunk, x, y, z, meshData);
            }

            return meshData;
        }

        protected virtual bool IsSolid(Direction direction) {
            switch (direction) {
                case Direction.north:
                    return true;
                case Direction.east:
                    return true;
                case Direction.south:
                    return true;
                case Direction.west:
                    return true;
                case Direction.up:
                    return true;
                case Direction.down:
                    return true;
            }
            return false;
        }

        protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddQuadTriangles();

            meshData.UV.AddRange(FaceUVs(Direction.up));

            return meshData;
        }

        protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddQuadTriangles();

            meshData.UV.AddRange(FaceUVs(Direction.down));

            return meshData;
        }

        protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddQuadTriangles();

            meshData.UV.AddRange(FaceUVs(Direction.north));

            return meshData;
        }

        protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddQuadTriangles();

            meshData.UV.AddRange(FaceUVs(Direction.east));

            return meshData;
        }

        protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddQuadTriangles();

            meshData.UV.AddRange(FaceUVs(Direction.south));

            return meshData;
        }

        protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData) {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddQuadTriangles();

            meshData.UV.AddRange(FaceUVs(Direction.west));

            return meshData;
        }

        protected virtual Vector2[] FaceUVs(Direction direction) {
            Tile tilePosition = TilePosition(direction);
            return new[] {
            new Vector2(TILE_SIZE * tilePosition.x + TILE_SIZE, TILE_SIZE * tilePosition.y),
            new Vector2(TILE_SIZE * tilePosition.x + TILE_SIZE, TILE_SIZE * tilePosition.y + TILE_SIZE),
            new Vector2(TILE_SIZE * tilePosition.x, TILE_SIZE * tilePosition.y + TILE_SIZE),
            new Vector2(TILE_SIZE * tilePosition.x, TILE_SIZE * tilePosition.y)
        };
        }

        protected virtual Tile TilePosition(Direction direction) =>
            new Tile { x = 0, y = 0 };
    }
}