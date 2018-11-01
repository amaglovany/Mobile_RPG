using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public string tag = "{NAME_TAG}";

    private void Awake()
    {
        Text get = GetComponent<Text>();

        if (get)
        {
            get.text = LanguageSystem.Instance.GetText(tag);
        }
    }
}
