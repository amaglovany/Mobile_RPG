// Swarm - Special renderer that draws a swarm of swirling/crawling lines.
// https://github.com/keijiro/Swarm

using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    // Pre-built template mesh for tube shapes.
    public sealed class TubeTemplate : ScriptableObject
    {
        #region Editor functions

#if UNITY_EDITOR

        public void Rebuild()
        {
            // Vertex array
            var vertices = new List<Vector3>();

            // Head tip
            vertices.Add(new Vector3(0, -1, 0));

            // Tube body vertices
            for (var i = 0; i < _segments + 1; i++)
            for (var j = 0; j < _divisions; j++)
            {
                var phi = Mathf.PI * 2 * j / _divisions;
                vertices.Add(new Vector3(phi, 0, i));
            }

            // Tail tip
            vertices.Add(new Vector3(0, 1, _segments)); // tail

            // Index array
            var indices = new List<int>();

            // Head cap
            for (var i = 0; i < _divisions - 1; i++)
            {
                indices.Add(0);
                indices.Add(i + 2);
                indices.Add(i + 1);
            }

            indices.Add(0);
            indices.Add(1);
            indices.Add(_divisions);

            // Tube body indices
            var refi = 1;

            for (var i = 0; i < _segments; i++)
            {
                for (var j = 0; j < _divisions - 1; j++)
                {
                    indices.Add(refi);
                    indices.Add(refi + 1);
                    indices.Add(refi + _divisions);

                    indices.Add(refi + 1);
                    indices.Add(refi + 1 + _divisions);
                    indices.Add(refi + _divisions);

                    refi++;
                }

                indices.Add(refi);
                indices.Add(refi + 1 - _divisions);
                indices.Add(refi + _divisions);

                indices.Add(refi + 1 - _divisions);
                indices.Add(refi + 1);
                indices.Add(refi + _divisions);

                refi++;
            }

            // Tail cap
            for (var i = 0; i < _divisions - 1; i++)
            {
                indices.Add(refi + i);
                indices.Add(refi + i + 1);
                indices.Add(refi + _divisions);
            }

            indices.Add(refi + _divisions - 1);
            indices.Add(refi);
            indices.Add(refi + _divisions);

            // Mesh rebuilding
            _mesh.Clear();
            _mesh.SetVertices(vertices);
            _mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            _mesh.UploadMeshData(true);
        }

#endif

        #endregion

        #region Exposed properties

        [Tooltip("Number of vertices on a ring.")] [SerializeField]
        private int _divisions = 6;

        public int divisions
        {
            get { return Mathf.Clamp(_divisions, 2, 64); }
        }

        [Tooltip("Number of segments in a tube.")] [SerializeField]
        private int _segments = 256;

        public int segments
        {
            get { return Mathf.Clamp(_segments, 4, 4096); }
        }

        #endregion

        #region Serialized data

        [SerializeField] private Mesh _mesh;

        public Mesh mesh
        {
            get { return _mesh; }
        }

        #endregion

        #region ScriptableObject functions

        private void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "Tube Template";
            }
        }

        private void OnValidate()
        {
            _divisions = Mathf.Clamp(_divisions, 2, 64);
            _segments = Mathf.Clamp(_segments, 4, 4096);
        }

        #endregion
    }
}