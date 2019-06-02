using UnityEngine;

namespace WorldGenerator {
    public static class Terrain {
        public static Block GetBlock(RaycastHit hit, bool adjacent = false) {
            Chunk chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null) {
                return null;
            }

            WorldPosition worldPosition = GetBlockPosition(hit, adjacent);
            Block block = chunk.World.GetBlock(worldPosition.X, worldPosition.Y, worldPosition.Z);
            return block;
        }

        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false) {
            Chunk chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null) {
                return false;
            }

            WorldPosition worldPosition = GetBlockPosition(hit, adjacent);
            chunk.World.SetBlock(worldPosition.X, worldPosition.Y, worldPosition.Z, block);
            return true;
        }

        // RaycastHit properties:
        // - point: The impact point in world space where the ray hit the collider.
        // - normal: The normal of the surface (triangle) the ray hit. The normal is the direction pointing away from the collider it hits.
        public static WorldPosition GetBlockPosition(RaycastHit hit, bool adjacent = false) =>
            GetBlockPosition(
                new Vector3(
                    MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
                    MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
                    MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
                )
            );

        public static WorldPosition GetBlockPosition(Vector3 position) =>
            new WorldPosition(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y),
                Mathf.RoundToInt(position.z)
            );

        // When we raycast onto a cube block the axis of the face the raycast hits will be 0.5, exactly half way between two blocks.
        // We can use the normal to move the point outwards or inwards
        // (Only add half because the whole thing could equal up to 1 which would push the position too far back):
        // - if we're getting the adjacent block add half the normal to the position pushing it outwards
        // - if we're not looking for the adjacent block subtract the same amount moving it further into the block we're pointing at
        private static float MoveWithinBlock(float position, float normal, bool adjacent = false) {
            if (position - (int)position == 0.5f || position - (int)position == -0.5f) {
                if (adjacent) {
                    position += normal / 2;
                } else {
                    position -= normal / 2;
                }
            }
            return position;
        }
    }
}
