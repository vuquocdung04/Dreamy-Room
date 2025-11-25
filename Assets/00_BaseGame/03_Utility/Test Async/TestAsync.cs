

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TestAsync : MonoBehaviour
{
    public int currentLevel;
    
    [Header("Data Menu - May tinh")]
    public HomeMenuData homeMenuData;
    
    [Header("Danh sach hoc sinh")]
    public List<ItemHomeMenu> items;
    private void Start()
    {
        bool isUnlock;
        
        for (int i = 0; i < items.Count; i++)
        {
            isUnlock = items[i].level > currentLevel;
            
            items[i].Init(homeMenuData.lsData[i]);
        }
    }
}

public class ItemHomeMenu : MonoBehaviour
{
    [Header("Setup")]
    public int level;
    public Image ImgIcon;
    
    [Header("Events")]
    public Button btnChoose;

    private void Start()
    {
        btnChoose.onClick.AddListener(delegate
        {
            Debug.Log("Play level " + level);
        });
    }

    public void Init(DanhSachData data)
    {
        level = data.idLevel;
        ImgIcon.sprite = data.iconLevel;
    }
}
[CreateAssetMenu(fileName = "HomeMenuData", menuName = "DATA/HomeMenuData")]
public class HomeMenuData : ScriptableObject
{
    public List<DanhSachData> lsData;
}

[System.Serializable]
public class DanhSachData
{
    public int idLevel;
    public GameObject levelPrefab;
    public Sprite iconLevel;
}


