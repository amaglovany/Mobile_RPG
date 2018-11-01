// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

using UnityEditor;
using UnityEngine;

namespace Cloner
{
    public class ClonerMaterialEditor : ShaderGUI
    {
        public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
        {
            EditorGUI.BeginChangeCheck();

            // Albedo map
            MaterialProperty texture = FindProperty("_MainTex", props);
            MaterialProperty option = FindProperty("_Color", props);
            editor.TexturePropertySingleLine(Labels.albedoMap, texture, option);

            // Metallic/Smoothness
            editor.RangeProperty(FindProperty("_Metallic", props), "Metallic");
            editor.RangeProperty(FindProperty("_Smoothness", props), "Smoothness");

            // Normal map
            texture = FindProperty("_NormalMap", props);
            option = FindProperty("_NormalScale", props);
            editor.TexturePropertySingleLine(
                Labels.normalMap, texture,
                texture.textureValue != null ? option : null
            );

            // Scale/Tiling
            texture = FindProperty("_MainTex", props);
            editor.TextureScaleOffsetProperty(texture);
        }

        private static class Labels
        {
            public static readonly GUIContent albedoMap = new GUIContent("Albedo Map");
            public static readonly GUIContent normalMap = new GUIContent("Normal Map");
        }
    }
}