// Kino/Streak - Anamorphic lens flare effect for Unity
// https://github.com/keijiro/KinoStreak

using UnityEditor;

namespace TDC.CameraEngine.Effect
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Streak))]
    public class StreakEditor : Editor
    {
        private SerializedProperty _intensity;
        private SerializedProperty _stretch;
        private SerializedProperty _threshold;
        private SerializedProperty _tint;

        private void OnEnable()
        {
            _threshold = serializedObject.FindProperty("_threshold");
            _stretch = serializedObject.FindProperty("_stretch");
            _intensity = serializedObject.FindProperty("_intensity");
            _tint = serializedObject.FindProperty("_tint");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_threshold);
            EditorGUILayout.PropertyField(_stretch);
            EditorGUILayout.PropertyField(_intensity);
            EditorGUILayout.PropertyField(_tint);

            serializedObject.ApplyModifiedProperties();
        }
    }
}