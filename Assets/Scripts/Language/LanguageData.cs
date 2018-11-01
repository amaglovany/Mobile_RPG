using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataWord
{
    public string text;
    public string tag;
}

[CreateAssetMenu(fileName = "LanguageData", menuName = "LanguageData")]
public class LanguageData : ScriptableObject
{
    public LanguageSystem.TypeLanguage type;
    public List<DataWord> listWords = new List<DataWord>();
}
