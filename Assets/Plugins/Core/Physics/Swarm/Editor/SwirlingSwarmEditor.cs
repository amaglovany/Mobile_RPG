// Swarm - Special renderer that draws a swarm of swirling/crawling lines.
// https://github.com/keijiro/Swarm

using UnityEditor;
using UnityEngine;

namespace Swarm
{
    // Custom inspector for SwirlingSwarm
    [CustomEditor(typeof(SwirlingSwarm))]
    [CanEditMultipleObjects]
    public class SwirlingSwarmEditor : Editor
    {
        private SerializedProperty _gradient;
        private SerializedProperty _instanceCount;
        private SerializedProperty _length;

        private SerializedProperty _material;
        private SerializedProperty _noiseFrequency;
        private SerializedProperty _noiseMotion;
        private SerializedProperty _radius;

        private SerializedProperty _randomSeed;

        private SerializedProperty _spread;
        private SerializedProperty _template;

        private void OnEnable()
        {
            _instanceCount = serializedObject.FindProperty("_instanceCount");
            _template = serializedObject.FindProperty("_template");
            _radius = serializedObject.FindProperty("_radius");
            _length = serializedObject.FindProperty("_length");

            _spread = serializedObject.FindProperty("_spread");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion = serializedObject.FindProperty("_noiseMotion");

            _material = serializedObject.FindProperty("_material");
            _gradient = serializedObject.FindProperty("_gradient");

            _randomSeed = serializedObject.FindProperty("_randomSeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_instanceCount);
            EditorGUILayout.PropertyField(_template);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_radius);
            EditorGUILayout.PropertyField(_length);
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(_spread);
            EditorGUILayout.LabelField("Noise Field");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_noiseFrequency, Labels.frequency);
            EditorGUILayout.PropertyField(_noiseMotion, Labels.motion);
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_gradient);

            EditorGUILayout.PropertyField(_randomSeed);

            serializedObject.ApplyModifiedProperties();
        }

        private static class Labels
        {
            public static readonly GUIContent frequency = new GUIContent("Frequency");
            public static readonly GUIContent motion = new GUIContent("Motion");
        }
    }
}