using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDC;

public class LanguageSystem : MonoBehaviourSingleton<LanguageSystem>
{
    public enum TypeLanguage
    {
        English,
        Russian
    }

    public TypeLanguage typeLanguage = TypeLanguage.English;
    public List<LanguageData> listLanguage = new List<LanguageData>();

    public string GetText(string tag)
    {
        for (int i = 0; i < listLanguage.Count; i++)
        {
            if(listLanguage[i].type == typeLanguage)
            {
                for (int w=0; w<listLanguage[i].listWords.Count; w++)
                {
                    if(listLanguage[i].listWords[w].tag == tag)
                    {
                        return listLanguage[i].listWords[w].text;
                    }
                }
            }
        }

        return null;
    }
}
