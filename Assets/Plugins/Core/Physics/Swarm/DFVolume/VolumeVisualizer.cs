// DFVolume - Distance field volume generator for Unity
// https://github.com/keijiro/DFVolume

using UnityEngine;

namespace DFVolume
{
    [ExecuteInEditMode]
    public class VolumeVisualizer : MonoBehaviour
    {
        [SerializeField] private VolumeData _data;
        [SerializeField] [Range(0, 1)] private float _depth = 0.5f;

        private Material _material;
        [SerializeField] private Mode _mode;

        [SerializeField] [HideInInspector] private Mesh _quadMesh;
        [SerializeField] [HideInInspector] private Shader _shader;

        private void OnDestroy()
        {
            if (_material != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(_material);
                }
                else
                {
                    DestroyImmediate(_material);
                }
            }
        }

        private void Update()
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _material.SetTexture("_MainTex", _data.texture);
            _material.SetFloat("_Depth", _depth);
            _material.SetFloat("_Mode", (int) _mode);

            Graphics.DrawMesh(
                _quadMesh, transform.localToWorldMatrix,
                _material, gameObject.layer
            );
        }

        private enum Mode
        {
            Distance,
            Gradient
        }
    }
}