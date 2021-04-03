using UnityEngine;

namespace UMT
{
    public static class MeshTransformer
    {
        public static Mesh GenerateMesh(Mesh original, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            var newMesh = Object.Instantiate(original);
            var vertices = newMesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                vertex.x *= scale.x;
                vertex.y *= scale.y;
                vertex.z *= scale.z;
                vertex = Quaternion.Euler(rotation) * vertex;
                vertex += position;
                vertices[i] = vertex;
            }

            newMesh.SetVertices(vertices);
            return newMesh;
        }
    }
}
