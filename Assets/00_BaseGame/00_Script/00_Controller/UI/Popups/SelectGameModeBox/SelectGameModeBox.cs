using UnityEngine;
using UnityEngine.UI;

public class SelectGameModeBox : BoxSingleton<SelectGameModeBox>
{
    public static SelectGameModeBox Setup()
    {
        return Path(PathPrefabs.SELECT_GAME_MODE_BOX);
    }

    public Button btnPlay;
    public Button btnRelax;
    public Button btnClose;
    public Button btnCloseWithPanel;
    
    protected override void Init()
    {
        
    }

    protected override void InitState()
    {
        
    }
}