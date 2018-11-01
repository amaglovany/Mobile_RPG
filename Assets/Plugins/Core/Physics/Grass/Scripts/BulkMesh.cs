//
// Bulk mesh container
//

using System;
using UnityEngine;

namespace Kvant
{
    public partial class Grass
    {
        [Serializable]
        private class BulkMesh
        {
            #region Public Properties

            public Mesh mesh { get; private set; }

            #endregion

            #region Public Methods

            public BulkMesh(Mesh[] shapes, int copyCount)
            {
                CombineMeshes(shapes, copyCount);
            }

            public void Rebuild(Mesh[] shapes, int copyCount)
            {
                Release();
                CombineMeshes(shapes, copyCount);
            }

            public void Release()
            {
                if (mesh)
                {
                    DestroyImmediate(mesh);
                }
            }

            #endregion

            #region Private Methods

            // Cache structure which stores shape information
            private struct ShapeCacheData
            {
                private readonly Vector3[] vertices;
                private readonly Vector3[] normals;
                private readonly Vector4[] tangents;
                private readonly Vector2[] uv;
                private readonly int[] indices;

                public ShapeCacheData(Mesh mesh)
                {
                    if (mesh)
                    {
                        vertices = mesh.vertices;
                        normals = mesh.normals;
                        tangents = mesh.tangents;
                        uv = mesh.uv;
                        indices = mesh.GetIndices(0);
                    }
                    else
                    {
                        // An empty mesh was given; replaces with a two-sided quad.
                        vertices = new[]
                        {
                            new Vector3(-1, +1, 0), new Vector3(+1, +1, 0),
                            new Vector3(-1, -1, 0), new Vector3(+1, -1, 0),
                            new Vector3(+1, +1, 0), new Vector3(-1, +1, 0),
                            new Vector3(+1, -1, 0), new Vector3(-1, -1, 0)
                        };
                        normals = new[]
                        {
                            Vector3.forward, Vector3.forward,
                            Vector3.forward, Vector3.forward,
                            -Vector3.forward, -Vector3.forward,
                            -Vector3.forward, -Vector3.forward
                        };
                        tangents = new[]
                        {
                            new Vector4(1, 0, 0, 1), new Vector4(1, 0, 0, 1),
                            new Vector4(1, 0, 0, 1), new Vector4(1, 0, 0, 1),
                            new Vector4(-1, 0, 0, 1), new Vector4(-1, 0, 0, 1),
                            new Vector4(-1, 0, 0, 1), new Vector4(-1, 0, 0, 1)
                        };
                        uv = new[]
                        {
                            new Vector2(0, 1), new Vector2(1, 1),
                            new Vector2(0, 0), new Vector2(1, 0),
                            new Vector2(1, 1), new Vector2(0, 1),
                            new Vector2(1, 0), new Vector2(0, 0)
                        };
                        indices = new[] {0, 1, 2, 3, 2, 1, 4, 5, 6, 7, 6, 5};
                    }
                }

                public int VertexCount
                {
                    get { return vertices.Length; }
                }

                public int IndexCount
                {
                    get { return indices.Length; }
                }

                public void CopyVerticesTo(Vector3[] destination, int position)
                {
                    Array.Copy(vertices, 0, destination, position, vertices.Length);
                }

                public void CopyNormalsTo(Vector3[] destination, int position)
                {
                    Array.Copy(normals, 0, destination, position, normals.Length);
                }

                public void CopyTangentsTo(Vector4[] destination, int position)
                {
                    Array.Copy(tangents, 0, destination, position, tangents.Length);
                }

                public void CopyUVTo(Vector2[] destination, int position)
                {
                    Array.Copy(uv, 0, destination, position, uv.Length);
                }

                public void CopyIndicesTo(int[] destination, int position, int offset)
                {
                    for (var i = 0; i < indices.Length; i++)
                        destination[position + i] = offset + indices[i];
                }
            }

            // Mesh combiner function
            private void CombineMeshes(Mesh[] shapes, int copyCount)
            {
                ShapeCacheData[] cache;

                if (shapes == null || shapes.Length == 0)
                {
                    // An empty shape was given; use the default shape.
                    cache = new ShapeCacheData[1];
                    cache[0] = new ShapeCacheData(null);
                }
                else
                {
                    // Copy given meshes into a shape cache.
                    cache = new ShapeCacheData[shapes.Length];
                    for (var i = 0; i < shapes.Length; i++)
                        cache[i] = new ShapeCacheData(shapes[i]);
                }

                // Count the number of vertices and indices in the shape cache.
                var vc_shapes = 0;
                var ic_shapes = 0;
                foreach (ShapeCacheData s in cache)
                {
                    vc_shapes += s.VertexCount;
                    ic_shapes += s.IndexCount;
                }

                // If there is nothing, break.
                if (vc_shapes == 0)
                {
                    return;
                }

                // Determine the number of vertices/indicies.
                var vc = 0;
                var ic = 0;
                for (var i = 0; i < copyCount; i++)
                {
                    ShapeCacheData s = cache[i % cache.Length];
                    if (vc + s.VertexCount > 65535)
                    {
                        break;
                    }

                    vc += s.VertexCount;
                    ic += s.IndexCount;
                }

                // Create vertex arrays.
                var vertices = new Vector3[vc];
                var normals = new Vector3[vc];
                var tangents = new Vector4[vc];
                var uv = new Vector2[vc];
                var uv2 = new Vector2[vc];
                var indicies = new int[ic];

                for (int v_i = 0, i_i = 0, e_i = 0; v_i < vc; e_i++)
                {
                    ShapeCacheData s = cache[e_i % cache.Length];

                    s.CopyVerticesTo(vertices, v_i);
                    s.CopyNormalsTo(normals, v_i);
                    s.CopyTangentsTo(tangents, v_i);
                    s.CopyUVTo(uv, v_i);

                    s.CopyIndicesTo(indicies, i_i, v_i);

                    var coord = new Vector2((float) e_i / copyCount, 0);
                    for (var i = 0; i < s.VertexCount; i++) uv2[v_i + i] = coord;

                    v_i += s.VertexCount;
                    i_i += s.IndexCount;
                }

                // Create a mesh object.
                mesh = new Mesh();

                mesh.vertices = vertices;
                mesh.normals = normals;
                mesh.tangents = tangents;
                mesh.uv = uv;
                mesh.uv2 = uv2;

                mesh.SetIndices(indicies, MeshTopology.Triangles, 0);
                ;

                // This only for temporary use. Don't save.
                mesh.hideFlags = HideFlags.DontSave;

                // Avoid being culled.
                mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);
            }

            #endregion
        }
    }
}