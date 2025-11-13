using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectGameModeBox : BoxSingleton<SelectGameModeBox>
{
    public static SelectGameModeBox Setup()
    {
        return Path(PathPrefabs.SELECT_GAME_MODE_BOX);
    }
    public Canvas canvas;
    public TextMeshProUGUI txtTitle;
    public Button btnPlay;
    public Button btnPlayWithAds;
    public Button btnRelax;
    public Button btnClose;
    public Button btnCloseWithPanel;
    public List<SGM_Item> lsItems;
    [Header("Localization")]
    public LocalizedText lcTitle;
    public List<LocalizedText> lsLcs;
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
        foreach (var item in lsItems)
        {
            item.Init();
        }
        AddOnClickListener(btnClose, delegate
        {
            Close();
            if(GameController.Instance.IsSceneGamePlay())
                GameController.Instance.ChangeScene2(SceneName.HOME_SCENE);
        });
        AddOnClickListener(btnCloseWithPanel, delegate
        {
            Close();
            if(GameController.Instance.IsSceneGamePlay())
                GameController.Instance.ChangeScene2(SceneName.HOME_SCENE);
        });
        AddOnClickListener(btnPlay, delegate
        {
            GameController.Instance.curGameModeName = GameMode.NORMAL;
            GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
        });
        AddOnClickListener(btnRelax, delegate
        {
            GameController.Instance.curGameModeName = GameMode.RELAX;
            GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
        });
        AddOnClickListener(btnPlayWithAds, delegate
        {
            GameController.Instance.curGameModeName = GameMode.NORMAL;
            GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
        });
        
        InitLocalization();
    }
    
    protected override void InitState()
    {
        txtTitle.text = $"Level {UseProfile.CurrentLevel}";
        UpdateUI();
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer,InitLocalization);
    }

    private void InitLocalization()
    {
        lcTitle.SetText(" " + UseProfile.CurrentLevel);
        lcTitle.Init();
        foreach(var t in lsLcs) t.Init();
    }

    private void AddOnClickListener(Button btn, System.Action callback =null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(); });
    }

    private void UpdateUI()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        var currentLevel = UseProfile.CurrentLevel;
        bool isRelax = currentLevel < maxLevel;
        btnRelax.gameObject.SetActive(isRelax);
        btnPlayWithAds.gameObject.SetActive(!isRelax);
    }
}