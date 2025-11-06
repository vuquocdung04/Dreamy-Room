using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TranslationEntry
{
    public string key;
    public string EN; 
    public string VI;
}


[CreateAssetMenu(fileName = "LocalizationData", menuName = "CSV/Localization", order = 0)]
public class LocalizationData : ScriptableObject
{
    public List<TranslationEntry> entries = new List<TranslationEntry>();
}