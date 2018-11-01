using System;
using System.Collections.Generic;
using TDC.SaveAndLoad;
using UnityEditor;

namespace TDC
{
    public static class CoreDefine
    {
        private const char delimiterDefine = ';';

        public static List<DefineData> listPlatform = new List<DefineData>(); //{ get; private set; }

        private static readonly List<BuildTargetGroup> listCorePlatform =
            new List<BuildTargetGroup> {BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS};

        public static void GetNativeDefine()
        {
            if (listPlatform.Count > 0)
            {
                listPlatform.Clear();
            }

            foreach (BuildTargetGroup platform in listCorePlatform)
            {
                var getDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);

                if (getDefine != "" || getDefine != null)
                {
                    var splitDefine = getDefine.Split(delimiterDefine);

                    for (var i = 0; i < splitDefine.Length; i++)
                    {
                        var allowed = true;

                        for (var check = 0; check < listPlatform.Count; check++)
                            if (listPlatform[check].name == splitDefine[i])
                            {
                                allowed = false;
                                break;
                            }

                        if (allowed)
                        {
                            var newData = new DefineData();
                            newData.Initialization(splitDefine[i], true);

                            listPlatform.Add(newData);
                        }
                    }
                }
            }
        }

        private static void InitDefine()
        {
            foreach (BuildTargetGroup platform in listCorePlatform)
            {
                var setDefine = "";

                for (var i = 0; i < listPlatform.Count; i++)
                    if (listPlatform[i].state)
                    {
                        setDefine += listPlatform[i].name;

                        if (i < listPlatform.Count - 1)
                        {
                            setDefine += delimiterDefine;
                        }
                    }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, setDefine);
                AssetDatabase.Refresh();
            }
        }

        public static void LoadDefine()
        {
            if (listPlatform.Count > 0)
            {
                listPlatform = new List<DefineData>();
            }

            var getList = CoreXml.Load(CoreConfiguration.pathDefineManager, "defineXml");

            if (getList == null || getList.Count == 0)
            {
                var gConfExample = new CoreXml.DataXml();
                gConfExample.Initialization("Define", "ExampleDefineName");

                var gConfAttribute = new CoreXml.DataXmlAttribute();
                gConfAttribute.Initialization("Atr", "0");
                gConfExample.listAttribute.Add(gConfAttribute);

                var listExample = new List<CoreXml.DataXml>();
                listExample.Add(gConfExample);

                CoreXml.Save(CoreConfiguration.pathDefineManager, "defineXml", listExample);

                return;
            }

            for (var i = 0; i < getList.Count; i++)
            {
                var newData = new DefineData();

                var state = false;

                if (int.Parse(getList[i].listAttribute[0].value) == 1)
                {
                    state = true;
                }

                newData.Initialization(getList[i].text, state);

                listPlatform.Add(newData);
            }
        }

        public static void AddDefine(string nameDefine)
        {
            var newData = new DefineData();

            newData.Initialization(nameDefine, false);

            listPlatform.Add(newData);
            SaveDefine();
        }

        public static void RemoveDefine(string name)
        {
            for (var i = 0; i < listPlatform.Count; i++)
                if (listPlatform[i].name == name)
                {
                    listPlatform.RemoveAt(i);
                    SaveDefine();
                    return;
                }
        }

        public static void SaveDefine()
        {
            var xmlListPlatform = new List<CoreXml.DataXml>();

            for (var i = 0; i < listPlatform.Count; i++)
            {
                var newData = new CoreXml.DataXml();
                newData.Initialization("Define", listPlatform[i].name);

                var newAtrribute = new CoreXml.DataXmlAttribute();

                var state = 0;

                if (listPlatform[i].state)
                {
                    state = 1;
                }

                newAtrribute.Initialization("state", state.ToString());

                newData.listAttribute.Add(newAtrribute);
                xmlListPlatform.Add(newData);
            }

            CoreXml.Save(CoreConfiguration.pathDefineManager, "defineXml", xmlListPlatform);
            InitDefine();
        }

        [Serializable]
        public class DefineData
        {
            public string name;
            public bool state;

            public void Initialization(string _nameDefine, bool _state)
            {
                name = _nameDefine;
                state = _state;
            }

            public void Switch(bool newState)
            {
                state = newState;
            }
        }
    }
}