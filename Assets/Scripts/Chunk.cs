using UnityEngine;

namespace WorldGenerator {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour {
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        public Block[,,] Blocks { get; set; } = new Block[SIZE, SIZE, SIZE];
        public bool DoUpdate { get; set; } = true;
        public World World { get; set; }
        public WorldPosition Position { get; set; }
        public const int SIZE = 16;

        // Start is called before the first frame update
        void Start() {
            meshFilter = gameObject.GetComponent<MeshFilter>();
            meshCollider = gameObject.GetComponent<MeshCollider>();
        }

        // Update is called once per frame
        void Update() {
            if (DoUpdate) {
                DoUpdate = false;
                UpdateChunk();
            }
        }

        public Block GetBlock(int x, int y, int z) {
            if (InRange(x) && InRange(y) && InRange(z)) {
                return Blocks[x, y, z];
            } else {
                return World.GetBlock(Position.X + x, Position.Y + y, Position.Z + z);
            }
        }

        public void SetBlock(int x, int y, int z, Block block) {
            if (InRange(x) && InRange(y) && InRange(z)) {
                Blocks[x, y, z] = block;
            } else {
                World.SetBlock(Position.X + x, Position.Y + y, Position.Z + z, block);
            }
        }

        public void SetBlocksUnmodified() {
            foreach (Block block in Blocks) {
                block.Changed = false;
            }
        }

        // Updates the chunk based on its contents
        private void UpdateChunk() {
            MeshData meshData = new MeshData();
            for (int x = 0; x < SIZE; x++) {
                for (int y = 0; y < SIZE; y++) {
                    for (int z = 0; z < SIZE; z++) {
                        meshData = Blocks[x, y, z].BlockData(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }

        // Sends the calculated mesh information to the mesh and collision components
        private void RenderMesh(MeshData meshData) {
            meshFilter.mesh.Clear();
            meshFilter.mesh.vertices = meshData.Vertices.ToArray();
            meshFilter.mesh.triangles = meshData.Triangles.ToArray();
            meshFilter.mesh.uv = meshData.UV.ToArray();
            meshFilter.mesh.RecalculateNormals();

            meshCollider.sharedMesh = null;
            Mesh mesh = new Mesh {
                vertices = meshData.ColliderVertices.ToArray(),
                triangles = meshData.ColliderTriangles.ToArray()
            };
            mesh.RecalculateNormals();
            meshCollider.sharedMesh = mesh;
        }

        public static bool InRange(int index) =>
            index >= 0 && index < SIZE;
    }
}
