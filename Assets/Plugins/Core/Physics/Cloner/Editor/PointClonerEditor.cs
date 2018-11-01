// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

using UnityEditor;
using UnityEngine;

namespace Cloner
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PointCloner))]
    public sealed class PointClonerEditor : Editor
    {
        private SerializedProperty _bounds;

        private SerializedProperty _displacementByNoise;
        private SerializedProperty _gradient;

        private SerializedProperty _material;

        private SerializedProperty _noiseFrequency;
        private SerializedProperty _noiseSpeed;
        private SerializedProperty _pointSource;
        private SerializedProperty _pulseProbability;
        private SerializedProperty _pulseSpeed;
        private SerializedProperty _randomSeed;
        private SerializedProperty _rotationByNoise;
        private SerializedProperty _scaleByNoise;
        private SerializedProperty _scaleByPulse;
        private SerializedProperty _template;
        private SerializedProperty _templateScale;

        private void OnEnable()
        {
            _pointSource = serializedObject.FindProperty("_pointSource");
            _template = serializedObject.FindProperty("_template");
            _templateScale = serializedObject.FindProperty("_templateScale");

            _displacementByNoise = serializedObject.FindProperty("_displacementByNoise");
            _rotationByNoise = serializedObject.FindProperty("_rotationByNoise");
            _scaleByNoise = serializedObject.FindProperty("_scaleByNoise");
            _scaleByPulse = serializedObject.FindProperty("_scaleByPulse");

            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseSpeed = serializedObject.FindProperty("_noiseSpeed");
            _pulseProbability = serializedObject.FindProperty("_pulseProbability");
            _pulseSpeed = serializedObject.FindProperty("_pulseSpeed");

            _material = serializedObject.FindProperty("_material");
            _gradient = serializedObject.FindProperty("_gradient");
            _bounds = serializedObject.FindProperty("_bounds");
            _randomSeed = serializedObject.FindProperty("_randomSeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_pointSource);
            EditorGUILayout.PropertyField(_template);
            var reallocate = EditorGUI.EndChangeCheck();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_templateScale, Labels.scale);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Modifier");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_displacementByNoise, Labels.position);
            EditorGUILayout.PropertyField(_rotationByNoise, Labels.orientation);
            EditorGUILayout.PropertyField(_scaleByNoise, Labels.scale);
            EditorGUILayout.PropertyField(_scaleByPulse, Labels.scalePulse);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Noise Field");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_noiseFrequency, Labels.frequency);
            EditorGUILayout.PropertyField(_noiseSpeed, Labels.speed);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Pulse Noise");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_pulseProbability, Labels.probability);
            EditorGUILayout.PropertyField(_pulseSpeed, Labels.speed);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_gradient);
            EditorGUILayout.PropertyField(_bounds);
            EditorGUILayout.PropertyField(_randomSeed);

            serializedObject.ApplyModifiedProperties();

            if (reallocate)
            {
                foreach (MonoBehaviour r in targets)
                    r.SendMessage("ReallocateBuffer");
            }
        }

        private static class Labels
        {
            public static readonly GUIContent frequency = new GUIContent("Frequency");
            public static readonly GUIContent orientation = new GUIContent("Orientation");
            public static readonly GUIContent position = new GUIContent("Position");
            public static readonly GUIContent probability = new GUIContent("Probability");
            public static readonly GUIContent scale = new GUIContent("Scale");
            public static readonly GUIContent scalePulse = new GUIContent("Scale (pulse)");
            public static readonly GUIContent speed = new GUIContent("Speed");
        }
    }
}