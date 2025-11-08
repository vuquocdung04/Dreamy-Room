
using EventDispatcher;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string localizeKey;
    private TextMeshProUGUI text;
    public void Init(string key = null)
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
        
        if(key != null)
            SetLocalizeKey(key);
        
        this.RegisterListener(EventID.CHANGE_LOCALIZATION, OnLanguageChanged);
        Refresh();
    }
    
    private void OnDisable()
    {
        this.RemoveListener(EventID.CHANGE_LOCALIZATION, OnLanguageChanged);
    }

    private void SetLocalizeKey(string key)
    {
        localizeKey = key;
    }
    
    private void OnLanguageChanged(object obj = null)
    {
        Refresh();
    }
    
    private void Refresh()
    {
        text.SetText(GameController.Instance.localizationController.GetString(localizeKey));
    }
    
    public void SetKeyOnEdittor(string key) => localizeKey = key;
    
    
}