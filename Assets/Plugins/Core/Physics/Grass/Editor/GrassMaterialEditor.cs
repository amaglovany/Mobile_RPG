//
// Custom material editor for Grass surface shader
//

using UnityEditor;
using UnityEngine;

namespace Kvant
{
    public class GrassMaterialEditor : ShaderGUI
    {
        private static readonly GUIContent _albedoText = new GUIContent("Albedo");
        private static readonly GUIContent _normalMapText = new GUIContent("Normal Map");
        private MaterialProperty _albedoMap;
        private MaterialProperty _color;
        private MaterialProperty _color2;
        private MaterialProperty _colorMode;

        private bool _initial = true;
        private MaterialProperty _metallic;
        private MaterialProperty _normalMap;
        private MaterialProperty _normalScale;
        private MaterialProperty _occExp;
        private MaterialProperty _occHeight;
        private MaterialProperty _occToColor;
        private MaterialProperty _smoothness;

        private void FindProperties(MaterialProperty[] props)
        {
            _colorMode = FindProperty("_ColorMode", props);
            _color = FindProperty("_Color", props);
            _color2 = FindProperty("_Color2", props);
            _metallic = FindProperty("_Metallic", props);
            _smoothness = FindProperty("_Smoothness", props);
            _albedoMap = FindProperty("_MainTex", props);
            _normalMap = FindProperty("_NormalMap", props);
            _normalScale = FindProperty("_NormalScale", props);
            _occHeight = FindProperty("_OccHeight", props);
            _occExp = FindProperty("_OccExp", props);
            _occToColor = FindProperty("_OccToColor", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            FindProperties(properties);

            if (ShaderPropertiesGUI(materialEditor) || _initial)
            {
                foreach (Material m in materialEditor.targets)
                    CompleteMaterialChanges(m);
            }

            _initial = false;
        }

        private bool ShaderPropertiesGUI(MaterialEditor materialEditor)
        {
            EditorGUI.BeginChangeCheck();

            materialEditor.ShaderProperty(_colorMode, "Color Mode");

            if (_colorMode.floatValue > 0)
            {
                Rect rect = EditorGUILayout.GetControlRect();
                rect.x += EditorGUIUtility.labelWidth;
                rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2 - 2;
                materialEditor.ShaderProperty(rect, _color, "");
                rect.x += rect.width + 4;
                materialEditor.ShaderProperty(rect, _color2, "");
            }
            else
            {
                materialEditor.ShaderProperty(_color, " ");
            }

            EditorGUILayout.Space();

            materialEditor.ShaderProperty(_metallic, "Metallic");
            materialEditor.ShaderProperty(_smoothness, "Smoothness");

            EditorGUILayout.Space();

            materialEditor.TexturePropertySingleLine(_albedoText, _albedoMap, null);
            materialEditor.TexturePropertySingleLine(_normalMapText, _normalMap,
                _normalMap.textureValue ? _normalScale : null);
            materialEditor.TextureScaleOffsetProperty(_albedoMap);

            EditorGUILayout.Space();

            materialEditor.ShaderProperty(_occHeight, "Occlusion Height");
            materialEditor.ShaderProperty(_occExp, "Occlusion Exponent");
            materialEditor.ShaderProperty(_occToColor, "Occlusion To Color");

            return EditorGUI.EndChangeCheck();
        }

        private static void CompleteMaterialChanges(Material material)
        {
            var occh = Mathf.Max(material.GetFloat("_OccHeight"), 0.01f);
            material.SetFloat("_HeightToOcc", 1.0f / occh);

            SetKeyword(material, "_ALBEDOMAP", material.GetTexture("_MainTex"));
            SetKeyword(material, "_NORMALMAP", material.GetTexture("_NormalMap"));
        }

        private static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
            {
                m.EnableKeyword(keyword);
            }
            else
            {
                m.DisableKeyword(keyword);
            }
        }
    }
}