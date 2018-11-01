using System;
using System.Collections.Generic;
using UnityEngine;

#region ClassData

[Serializable]
public class DDebug
{
    public enum TColor
    {
        Error,
        Warning,
        Apply,
        Default,
        System
    }

    public Color32 Color;

    public TColor TypeColor;
}

#endregion

[Serializable]
public class DataDebug
{
    public List<DDebug> listDebug;

    private void Awake()
    {
        listDebug = new List<DDebug>();
    }
}