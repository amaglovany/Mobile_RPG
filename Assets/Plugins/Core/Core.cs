using System.Collections.Generic;
using TDC.Audio;
using TDC.LoadScreen;
using TDC.SaveAndLoad;
using TDC.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//using UnityEditor;

#if GoogleMobileAds
using GoogleMobileAds.Api;
#endif

namespace TDC
{
    [AddComponentMenu("TDC/Core")]
    public class Core : MonoBehaviourSingleton<Core>
    {
        #region Data

#if GoogleMobileAds
        [SerializeField] private CoreAdMob coreAdMob;
        public CoreAdMob CoreAdMob { get { return coreAdMob; } }
        #endif

        [SerializeField] private DataDebug dataDebug;

        public DataDebug DataDebug
        {
            get { return dataDebug; }
        }

        [SerializeField] private DialogApp dialogApp;

        public DialogApp DialogApp
        {
            get { return dialogApp; }
        }

        [SerializeField] private CoreAudio coreAudio;

        public CoreAudio CoreAudio
        {
            get { return coreAudio; }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Initialization();
        }

        private void Update()
        {
            CoreUpdate();
        }

        #endregion

        #region Core

        public void Initialization()
        {
            Globals.Instance.Initialization();

#if GoogleMobileAds
            coreAdMob.Initialization();
            #endif

            DontDestroyOnLoad(gameObject);


            var initScene = new List<int>();

            initScene.Add(2);

            LoadManager.Instance.LoadScene(initScene, LoadSceneMode.Additive);
        }

        public void CoreUpdate()
        {
            Globals.Instance.CoreUpdate();
        }

        #endregion

        #region Debug

        public string GetStringColor(string text, DDebug.TColor type = DDebug.TColor.Default)
        {
            foreach (DDebug Item in DataDebug.listDebug)
                if (Item.TypeColor == type)
                {
                    var StrColor = Item.Color.r.ToString("X2") + Item.Color.g.ToString("X2") +
                                   Item.Color.b.ToString("X2");
                    return "<color=#" + StrColor + ">" + text + "</color>";
                }

            return text;
        }

        public string DebugLog(string text, DDebug.TColor type = DDebug.TColor.Default)
        {
            return GetStringColor("[System] ", DDebug.TColor.System) + GetStringColor(text, type);
        }

        #endregion
    }

    //[InitializeOnLoad]
    public class CoreConfiguration
    {
        private const string globalPathXml = "Assets\\Core\\Data\\";
        private const string globalNameXml = "globalConfiguration";
        public static string pathDefineManager;

        public static List<CoreXml.DataXml> globalConfiguration = new List<CoreXml.DataXml>();

        static CoreConfiguration()
        {
            globalConfiguration = new List<CoreXml.DataXml>();
            globalConfiguration = CoreXml.Load(globalPathXml, globalNameXml);

            if (globalConfiguration == null)
            {
                var gConfExample = new CoreXml.DataXml();
                gConfExample.Initialization("PathDefineManager", "Assets\\Core\\Data\\");

                var listExample = new List<CoreXml.DataXml>();
                listExample.Add(gConfExample);

                CoreXml.Save(globalPathXml, globalNameXml, listExample);
                return;
            }

            Debug.Log("Unity Core Initialization [" + globalConfiguration.Count + "]");

            for (var i = 0; i < globalConfiguration.Count; i++)
            {
                switch (globalConfiguration[i].name)
                {
                    case "PathDefineManager":
                        pathDefineManager = globalConfiguration[i].text;
                        break;
                }

                Debug.Log("Load path: " + globalConfiguration[i].text);
            }
        }

        public static void EditConfiguration(string name, string value)
        {
            if (globalConfiguration.Count == 0)
            {
                return;
            }

            for (var i = 0; i < globalConfiguration.Count; i++)
                if (globalConfiguration[i].name == name)
                {
                    globalConfiguration[i].SetText(value);
                }

            CoreSave();
        }

        public static void CoreSave()
        {
            CoreXml.Save(globalPathXml, globalNameXml, globalConfiguration);
            Debug.Log("Unity Core Save");
        }
    }
}