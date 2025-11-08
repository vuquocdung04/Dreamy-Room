using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private CollectionType type;
    [SerializeField] private Button btn;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI txtProgress;
    [SerializeField] private LocalizedText localizedText;
    private int curAmountProgress;


    public CollectionType GetCollectionType() => type;
    public void AddClickListener(System.Action<CollectionItem> callback = null )
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(this); });
    }
    
    public void Init()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;
        var dataCollectionConfict = dataCollection.GetCollectionByType(type);
        var lsCollection = new List<int>(dataCollectionConfict.lsIdCards);
        curAmountProgress = 0;
        
        foreach (var t in lsCollection)
        {
            if (t > UseProfile.MaxUnlockedLevel) continue;
            curAmountProgress++;
        }
        txtProgress.text = curAmountProgress.ToString();
        fill.fillAmount = (float)curAmountProgress / dataCollectionConfict.totalAmount;
        localizedText.Init();
    }

    public void HandleInteractableBtn(bool isState)
    {
        btn.interactable = isState;
    }
    
    public LocalizedText GetLocalizedText() => localizedText;
    public void SetupOdin(int id)
    {
        btn =  GetComponent<Button>();
        fill = transform.Find("progressBar").Find("fill").GetComponent<Image>();
        txtProgress = transform.Find("progressBar").Find("txtCurrent").GetComponent<TextMeshProUGUI>();
        type = (CollectionType)id;
        var txtObj = transform.Find("Title/txt");
        if (txtObj != null)
        {
            localizedText = txtObj.GetComponent<LocalizedText>();
            if (localizedText == null)
                localizedText = txtObj.gameObject.AddComponent<LocalizedText>();
        }
        else
            Debug.LogError("Không tìm thấy Text object 'Title/txt' trong CollectionItem!");
    }
}