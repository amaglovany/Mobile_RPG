using UnityEditor;
using UnityEngine;

namespace TDC
{
    public class EditorWindowWrapper : EditorWindow
    {
        protected Color backgroundColor;
        protected Color disableColor;
        protected Color enableColor;
        protected Color labelColor;
        protected Color mainColor;

        protected void Initialization()
        {
        }

        #region Wrapper

        protected virtual void OnGUI()
        {
            mainColor = new Color(0f, 0.7f, 0.8f);
            backgroundColor = new Color(0f, 0.5f, 0.6f);
            labelColor = new Color(0f, 0.3f, 0.4f);
            enableColor = new Color(0.65f, 0.9f, 0.5f);
            disableColor = new Color(0.9f, 0.3f, 0.3f);
        }

        protected virtual void Header(string nameWindow)
        {
            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);

            var headerStyle = new GUIStyle(EditorStyles.largeLabel);
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            GUI.color = new Color(0f, 0.7f, 0.8f);

            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(20));
            {
                GUI.color = Color.white;
                EditorGUILayout.LabelField("", GUILayout.Width(5));
                EditorGUILayout.LabelField(nameWindow, headerStyle, GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void Floor()
        {
            Color mainColor = GUI.color;
            Color backgroundColor = GUI.backgroundColor;

            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);

            var headerStyle = new GUIStyle(EditorStyles.largeLabel);
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            GUI.color = new Color(0f, 0.4f, 0.5f);

            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(20));
            {
                GUI.color = mainColor;
                EditorGUILayout.LabelField("", GUILayout.Width(5));
                EditorGUILayout.LabelField("Themes Daly Core", headerStyle, GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}