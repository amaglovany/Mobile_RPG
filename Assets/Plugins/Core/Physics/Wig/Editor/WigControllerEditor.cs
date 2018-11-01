using UnityEditor;
using UnityEngine;

namespace Kvant
{
    [CustomEditor(typeof(WigController))]
    public class WigControllerEditor : Editor
    {
        #region Editor functions

        private SerializedProperty _target;
        private SerializedProperty _template;

        private SerializedProperty _maxTimeStep;
        private SerializedProperty _randomSeed;

        private SerializedProperty _length;
        private SerializedProperty _lengthRandomness;

        private SerializedProperty _spring;
        private SerializedProperty _damping;
        private SerializedProperty _gravity;

        private SerializedProperty _noiseAmplitude;
        private SerializedProperty _noiseFrequency;
        private SerializedProperty _noiseSpeed;

        private static readonly GUIContent _textAmplitude = new GUIContent("Amplitude");
        private static readonly GUIContent _textFrequency = new GUIContent("Frequency");
        private static readonly GUIContent _textRandomness = new GUIContent("Randomness");
        private static readonly GUIContent _textSpeed = new GUIContent("Speed");

        private void OnEnable()
        {
            _target = serializedObject.FindProperty("_target");
            _template = serializedObject.FindProperty("_template");

            _maxTimeStep = serializedObject.FindProperty("_maxTimeStep");
            _randomSeed = serializedObject.FindProperty("_randomSeed");

            _length = serializedObject.FindProperty("_length");
            _lengthRandomness = serializedObject.FindProperty("_lengthRandomness");

            _spring = serializedObject.FindProperty("_spring");
            _damping = serializedObject.FindProperty("_damping");
            _gravity = serializedObject.FindProperty("_gravity");

            _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseSpeed = serializedObject.FindProperty("_noiseSpeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var needsReset = false;
            var reconfigured = false;

            // VVV Check changes from here (needsReset; editor only) VVV
            if (!Application.isPlaying)
            {
                EditorGUI.BeginChangeCheck();
            }

            EditorGUILayout.PropertyField(_target);

            // VVV Check changes from here (reconfigured) VVV
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_template);

            reconfigured = EditorGUI.EndChangeCheck();
            // ^^^ Check changes to here (reconfigured) ^^^

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_maxTimeStep);

            // VVV Check changes from here (needsReset) VVV
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_randomSeed);

            needsReset |= EditorGUI.EndChangeCheck();
            // ^^^ Check changes to here (needsReset) ^^^

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Filament", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_length);
            EditorGUILayout.PropertyField(_lengthRandomness, _textRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Dynamics", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spring);
            EditorGUILayout.PropertyField(_damping);
            EditorGUILayout.PropertyField(_gravity);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Noise Field Force", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_noiseAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_noiseFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_noiseSpeed, _textSpeed);

            if (!Application.isPlaying)
            {
                needsReset |= EditorGUI.EndChangeCheck();
            }
            // ^^^ Check changes to here (needsReset; editor only) ^^^

            serializedObject.ApplyModifiedProperties();

            // Set reset flags if there are any changes.
            if (needsReset || reconfigured)
            {
                foreach (Object t in targets)
                {
                    var wig = (WigController) t;
                    if (needsReset)
                    {
                        wig.ResetSimulation();
                    }

                    if (reconfigured)
                    {
                        wig.RequestReconfigurationFromEditor();
                    }
                }
            }
        }

        #endregion
    }
}