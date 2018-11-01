// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Cloner
{
    [CustomEditor(typeof(PointCloud))]
    public sealed class PointCloudEditor : Editor
    {
        #region Custom inspector

        public override void OnInspectorGUI()
        {
            var pointCount = ((PointCloud) target).pointCount;
            EditorGUILayout.LabelField("Point Count", pointCount.ToString());
        }

        #endregion

        #region Menu item functions

        private static Object[] SelectedMeshes
        {
            get { return Selection.GetFiltered(typeof(Mesh), SelectionMode.Deep); }
        }

        [MenuItem("Assets/Cloner/Convert To Point Cloud", true)]
        private static bool ValidateConvertToPointCloud()
        {
            return SelectedMeshes.Length > 0;
        }

        [MenuItem("Assets/Cloner/Convert To Point Cloud")]
        private static void ConvertToTemplate()
        {
            var templates = new List<Object>();

            foreach (Mesh mesh in SelectedMeshes)
            {
                // Destination file path.
                var dirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(mesh));
                var filename = (string.IsNullOrEmpty(mesh.name) ? "Points" : mesh.name + " Points") + ".asset";
                var assetPath = AssetDatabase.GenerateUniqueAssetPath(dirPath + "/" + filename);

                // Create a point buffer asset.
                var asset = CreateInstance<PointCloud>();
                asset.InitWithMesh(mesh);
                AssetDatabase.CreateAsset(asset, assetPath);

                templates.Add(asset);
            }

            // Save the generated assets.
            AssetDatabase.SaveAssets();

            // Select the generated assets.
            EditorUtility.FocusProjectWindow();
            Selection.objects = templates.ToArray();
        }

        #endregion
    }
}