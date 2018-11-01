using TDC.LoadScreen;
using TDC.UI;
using UnityEditor;
using UnityEngine;

namespace TDC
{
    public class EditorAddObject : Editor
    {
        #region Data

        #endregion

        #region Unity

        private void Start()
        {
        }

        private void Update()
        {
        }

        #endregion

        #region Core

        public void Initialization()
        {
        }

        public void CoreUpdate()
        {
        }

        #region UI

        [MenuItem("CoreApp", menuItem = "GameObject/TDCore/Core", priority = -10000)]
        public static void AddCore()
        {
            var gameObject = new GameObject("TDC|Core");
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.AddComponent<Core>();
            gameObject.AddComponent<LoadManager>();
            gameObject.AddComponent<PoolManager.PoolManager>();

            if (Selection.objects.Length > 0)
            {
                gameObject.transform.SetParent(Selection.transforms[0]);
                gameObject.transform.localPosition = Vector3.zero;
            }

            Selection.objects = new[] {gameObject};
            EditorApplication.ExecuteMenuItem("GameObject/Move To View");
        }

        [MenuItem("UIWindowManager", menuItem = "GameObject/TDCore/UI/WindowManager", priority = -10001)]
        public static void AddUIWindowManager()
        {
            var gameObject = new GameObject("TDC|WindowManager");
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.AddComponent<WindowManager>();

            if (Selection.objects.Length > 0)
            {
                gameObject.transform.SetParent(Selection.transforms[0]);
                gameObject.transform.localPosition = Vector3.zero;
            }

            Selection.objects = new[] {gameObject};
            EditorApplication.ExecuteMenuItem("GameObject/Move To View");
        }

        [MenuItem("UIWindowControl", menuItem = "GameObject/TDCore/UI/WindowControl", priority = -9999)]
        public static void AddUIWindowControl()
        {
            var gameObject = new GameObject("TDC|WindowControl");
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.AddComponent<WindowControl>();

            if (Selection.objects.Length > 0)
            {
                gameObject.transform.SetParent(Selection.transforms[0]);
                gameObject.transform.localPosition = Vector3.zero;

                if (Selection.transforms[0].GetComponent<WindowManager>())
                {
                    Selection.transforms[0].GetComponent<WindowManager>().ListWindow
                        .Add(gameObject.GetComponent<WindowControl>());
                }
            }
        }

        #region Elements

        [MenuItem("UIButton", menuItem = "GameObject/TDCore/UI/Elements/Button", priority = -11001)]
        public static void AddUIButton()
        {
            var gameObject = new GameObject("TDC|Button");
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.AddComponent<ButtonUI>();

            if (Selection.objects.Length > 0)
            {
                gameObject.transform.SetParent(Selection.transforms[0]);
                gameObject.transform.localPosition = Vector3.zero;
            }

            Selection.objects = new[] {gameObject};
            EditorApplication.ExecuteMenuItem("GameObject/Move To View");
        }

        #endregion

        #endregion

        #endregion
    }
}