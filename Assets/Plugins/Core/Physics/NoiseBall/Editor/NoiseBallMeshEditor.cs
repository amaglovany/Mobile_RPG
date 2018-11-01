using System.IO;
using UnityEditor;
using UnityEngine;

namespace NoiseBall
{
    [CustomEditor(typeof(NoiseBallMesh))]
    public class NoiseBallMeshEditor : Editor
    {
        private SerializedProperty _subdivisionLevel;

        private void OnEnable()
        {
            _subdivisionLevel = serializedObject.FindProperty("_subdivisionLevel");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_subdivisionLevel);
            var rebuild = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            if (rebuild)
            {
                foreach (Object t in targets)
                    ((NoiseBallMesh) t).RebuildMesh();
            }
        }

        [MenuItem("Assets/Create/Emgen/NoiseBallMesh")]
        public static void CreateNoiseBallMeshAsset()
        {
            // Make a proper path from the current selection.
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/NoiseBallMesh.asset");

            // Create an NoiseBballMesh asset.
            var asset = CreateInstance<NoiseBallMesh>();
            AssetDatabase.CreateAsset(asset, assetPathName);
            AssetDatabase.AddObjectToAsset(asset.sharedMesh, asset);

            // Build an initial mesh for the asset.
            asset.RebuildMesh();

            // Save the generated mesh asset.
            AssetDatabase.SaveAssets();

            // Tweak the selection.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}