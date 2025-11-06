using System;
using EventDispatcher;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string localizeKey;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        this.RegisterListener(EventID.CHANGE_LOCALIZATION, OnLanguageChanged);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.CHANGE_LOCALIZATION, OnLanguageChanged);
    }

    private void OnLanguageChanged(object obj = null)
    {
        Refresh();
    }
    
    private void Refresh()
    {
        text.SetText(GameController.Instance.localizationController.GetString(localizeKey));
    }
    
}