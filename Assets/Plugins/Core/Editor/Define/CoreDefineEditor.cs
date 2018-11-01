using UnityEditor;
using UnityEngine;

namespace TDC
{
    public class CoreDefineEditor : EditorWindowWrapper
    {
        #region Data

        private static readonly string nameWindow = "DefineManager";

        #endregion

        #region Unity

        protected void OnEnable()
        {
            CoreDefine.LoadDefine();
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            Header(nameWindow);
            RenderTableHeader();
            ViewListDefine();
            Floor();
        }

        #endregion

        #region Core

        [MenuItem("TDC/Define Manager")]
        public static void InitializationWindow()
        {
            var window = (CoreDefineEditor) GetWindow(typeof(CoreDefineEditor));
            window.titleContent = new GUIContent(nameWindow);
            window.Show();
        }

        private void RenderTableHeader()
        {
            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);

            var headerStyle = new GUIStyle(EditorStyles.largeLabel);
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            GUI.color = backgroundColor;

            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(20));
            {
                GUILayout.Space(15);
                GUI.color = Color.white;
                GUILayout.Label("ID", headerStyle, GUILayout.Height(20));

                GUILayout.Space(15);
                GUILayout.Label("State", headerStyle, GUILayout.Height(20));

                GUILayout.Space(150);
                GUILayout.Label("Define", headerStyle, GUILayout.Width(248), GUILayout.Height(20));

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ViewListDefine()
        {
            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);
            directiveLineStyle.hover.textColor = new Color(1f, 0.5f, 0.2f);

            var textFieldStyle = new GUIStyle(EditorStyles.toolbarTextField);
            textFieldStyle.alignment = TextAnchor.MiddleLeft;
            textFieldStyle.fixedHeight = 0;
            textFieldStyle.normal.textColor = labelColor;
            textFieldStyle.fontStyle = FontStyle.Bold;
            textFieldStyle.padding = new RectOffset(8, 8, 0, 0);
            textFieldStyle.fontSize = 12;

            var textButtondStyle = new GUIStyle(EditorStyles.miniButton);
            textButtondStyle.alignment = TextAnchor.MiddleCenter;
            textButtondStyle.fixedHeight = 0;
            textButtondStyle.normal.textColor = labelColor;
            textButtondStyle.fontStyle = FontStyle.Bold;
            //textButtondStyle.padding = new RectOffset(8, 8, 0, 0);
            textButtondStyle.fontSize = 12;

            var labelStyle = new GUIStyle(EditorStyles.largeLabel);
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = labelColor;
            labelStyle.fixedHeight = 0;
            labelStyle.padding = new RectOffset(8, 8, 0, 0);
            labelStyle.fontSize = 12;

            GUI.color = new Color(1f, 1f, 1f);


            for (var i = 0; i < CoreDefine.listPlatform.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(32));
                {
                    GUILayout.Space(12);

                    GUILayout.Label((i + 1).ToString(), labelStyle, GUILayout.Height(25));

                    GUILayout.Space(8);

                    if (CoreDefine.listPlatform[i].state)
                    {
                        GUI.color = mainColor;
                        if (GUILayout.Button("On", textButtondStyle, GUILayout.Width(50), GUILayout.Height(25)))
                        {
                            CoreDefine.listPlatform[i].state = false;
                            CoreDefine.SaveDefine();
                        }

                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.color = Color.white;
                        if (GUILayout.Button("Off", textButtondStyle, GUILayout.Width(50), GUILayout.Height(25)))
                        {
                            CoreDefine.listPlatform[i].state = true;
                            CoreDefine.SaveDefine();
                        }

                        GUI.color = Color.white;
                    }

                    GUILayout.Space(25);

                    if (CoreDefine.listPlatform[i].state)
                    {
                        GUI.color = mainColor;
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }

                    CoreDefine.listPlatform[i].name = GUILayout.TextField(CoreDefine.listPlatform[i].name,
                        textFieldStyle, GUILayout.Width(300), GUILayout.Height(25));
                    GUI.color = Color.white;

                    GUILayout.Space(3);

                    if (GUILayout.Button(new GUIContent("-", "Delete Define"), textButtondStyle, GUILayout.Width(25),
                        GUILayout.Height(25)))
                    {
                        CoreDefine.RemoveDefine(CoreDefine.listPlatform[i].name);
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(32));
            {
                GUILayout.Space(4);

                GUILayout.Label("Dev", labelStyle, GUILayout.Height(25));


                if (GUILayout.Button(new GUIContent("+", "Add new Define"), textButtondStyle, GUILayout.Width(50),
                    GUILayout.Height(25)))
                {
                    CoreDefine.AddDefine("New Define");
                }

                GUILayout.Space(25);

                if (GUILayout.Button(new GUIContent("Save", "Add new Define"), textButtondStyle, GUILayout.Width(300),
                    GUILayout.Height(25)))
                {
                    CoreDefine.SaveDefine();
                    CoreConfiguration.EditConfiguration("PathDefineManager", CoreConfiguration.pathDefineManager);
                }

                GUILayout.Space(25);

                GUILayout.Label("Path", labelStyle, GUILayout.Height(25));

                GUILayout.Space(5);

                CoreConfiguration.pathDefineManager = GUILayout.TextField(CoreConfiguration.pathDefineManager,
                    textFieldStyle, GUILayout.Width(500), GUILayout.Height(25));

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}