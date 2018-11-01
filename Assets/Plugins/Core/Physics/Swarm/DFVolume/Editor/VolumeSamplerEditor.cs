// DFVolume - Distance field volume generator for Unity
// https://github.com/keijiro/DFVolume

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DFVolume
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VolumeSampler))]
    internal class VolumeSamplerEditor : Editor
    {
        private SerializedProperty _extent;
        private SerializedProperty _resolution;

        private void OnEnable()
        {
            _resolution = serializedObject.FindProperty("_resolution");
            _extent = serializedObject.FindProperty("_extent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_resolution);
            EditorGUILayout.PropertyField(_extent);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Create Volume Data"))
            {
                CreateVolumeData();
            }

            CheckSkewedTransform();
        }

        private void CreateVolumeData()
        {
            var output = new List<Object>();

            foreach (VolumeSampler sampler in targets)
            {
                var path = "Assets/New Volume Data.asset";
                path = AssetDatabase.GenerateUniqueAssetPath(path);

                var asset = CreateInstance<VolumeData>();
                asset.Initialize(sampler);

                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.AddObjectToAsset(asset.texture, asset);
            }

            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.objects = output.ToArray();
        }

        private void CheckSkewedTransform()
        {
            if (targets.Any(o => ((Component) o).transform.lossyScale != Vector3.one))
            {
                EditorGUILayout.HelpBox(
                    "Using scale in transform may introduce error in output volumes.",
                    MessageType.Warning
                );
            }
        }
    }
}