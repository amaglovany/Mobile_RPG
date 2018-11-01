//
// Custom editor for GrassScroller
//

using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GrassScroller))]
    public class GrassScrollerEditor : Editor
    {
        private SerializedProperty _speed;
        private SerializedProperty _yawAngle;

        private void OnEnable()
        {
            _yawAngle = serializedObject.FindProperty("_yawAngle");
            _speed = serializedObject.FindProperty("_speed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_yawAngle);
            EditorGUILayout.PropertyField(_speed);
            serializedObject.ApplyModifiedProperties();
        }
    }
}