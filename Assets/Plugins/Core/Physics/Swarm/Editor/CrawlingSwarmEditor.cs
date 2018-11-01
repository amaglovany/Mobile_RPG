// Swarm - Special renderer that draws a swarm of swirling/crawling lines.
// https://github.com/keijiro/Swarm

using UnityEditor;
using UnityEngine;

namespace Swarm
{
    // Custom inspector for CrawlingSwarm
    [CustomEditor(typeof(CrawlingSwarm))]
    [CanEditMultipleObjects]
    public class CrawlingSwarmEditor : Editor
    {
        private SerializedProperty _gradient;
        private SerializedProperty _initialSpread;
        private SerializedProperty _instanceCount;

        private SerializedProperty _material;
        private SerializedProperty _noiseFrequency;
        private SerializedProperty _noiseMotion;
        private SerializedProperty _noiseSpread;
        private SerializedProperty _radius;

        private SerializedProperty _randomSeed;

        private SerializedProperty _speed;
        private SerializedProperty _template;
        private SerializedProperty _trim;
        private SerializedProperty _volume;

        private void OnEnable()
        {
            _instanceCount = serializedObject.FindProperty("_instanceCount");
            _template = serializedObject.FindProperty("_template");
            _radius = serializedObject.FindProperty("_radius");
            _trim = serializedObject.FindProperty("_trim");

            _speed = serializedObject.FindProperty("_speed");
            _volume = serializedObject.FindProperty("_volume");
            _initialSpread = serializedObject.FindProperty("_initialSpread");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseSpread = serializedObject.FindProperty("_noiseSpread");
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
            EditorGUILayout.PropertyField(_trim);
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(_speed);
            EditorGUILayout.PropertyField(_volume);
            EditorGUILayout.PropertyField(_initialSpread);
            EditorGUILayout.LabelField("Noise Field");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_noiseFrequency, Labels.frequency);
            EditorGUILayout.PropertyField(_noiseSpread, Labels.spread);
            EditorGUILayout.PropertyField(_noiseMotion, Labels.motion);
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_gradient);

            EditorGUILayout.PropertyField(_randomSeed);

            serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying && GUILayout.Button("Reset"))
            {
                foreach (CrawlingSwarm cs in targets) cs.ResetPositions();
            }
        }

        private static class Labels
        {
            public static readonly GUIContent frequency = new GUIContent("Frequency");
            public static readonly GUIContent spread = new GUIContent("Spread");
            public static readonly GUIContent motion = new GUIContent("Motion");
        }
    }
}