using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerator {
    public class MeshData {
        // Vertices are vector 3 positions in space that define the points
        // or corners of every triangle in the mesh.
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        // Every three entries in the triangles list defines one triangle
        // and the entry itself is the index of a vector 3 in the vertices array.
        public List<int> Triangles { get; } = new List<int>();
        // The uv list is a vector 2 list of texture coordinates and there are two entries per triangle,
        // the coordinates of the lower left of the triangle and the upper right of the triangle.
        public List<Vector2> UV { get; } = new List<Vector2>();

        // The collider lists are the same as vertices and triangles,
        // but for use as the collider mesh so that you can pass a different mesh for the rendering and the collider.
        public List<Vector3> ColliderVertices { get; } = new List<Vector3>();
        public List<int> ColliderTriangles { get; } = new List<int>();
        // Determines if all triangles and vertices added to the render mesh get added to the collision mesh as well.
        public bool UseRenderDataForCollider { get; set; }

        public MeshData() { }

        public void AddVertex(Vector3 vertex) {
            Vertices.Add(vertex);

            if (UseRenderDataForCollider) {
                ColliderVertices.Add(vertex);
            }
        }

        public void AddTriangle(int index) {
            Triangles.Add(index);

            if (UseRenderDataForCollider) {
                ColliderTriangles.Add(index - (Vertices.Count - ColliderVertices.Count));
            }
        }

        public void AddQuadTriangles() {
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);

            if (UseRenderDataForCollider) {
                ColliderTriangles.Add(ColliderVertices.Count - 4);
                ColliderTriangles.Add(ColliderVertices.Count - 3);
                ColliderTriangles.Add(ColliderVertices.Count - 2);

                ColliderTriangles.Add(ColliderVertices.Count - 4);
                ColliderTriangles.Add(ColliderVertices.Count - 2);
                ColliderTriangles.Add(ColliderVertices.Count - 1);
            }
        }
    }
}
