using System.Collections.Generic;
using UnityEngine.UI;

public class RateBox : BoxSingleton<RateBox>
{
    public static RateBox Setup()
    {
        return Path(PathPrefabs.RATE_BOX);
    }
    public Button  btnClose;
    public Button  btnRate;
    public List<Button> lsBtnStars;
    public List<Image> lsImgStars;
    private int currentStar;
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
        btnRate.onClick.AddListener(HandleRate);
        
        for (int i = 0; i < lsBtnStars.Count; i++)
        {
            int starIndex = i; 
            lsBtnStars[i].onClick.AddListener(() =>
            {
                HandleStarClick(starIndex);
            });
        }
    }

    protected override void InitState()
    {
        UpdateStarVisuals(0);
    }
    

    private void HandleStarClick(int index)
    {
        currentStar = index + 1;
        if(currentStar > 0) btnRate.interactable = true;
        UpdateStarVisuals(currentStar);
    }

    private void UpdateStarVisuals(int starCount)
    {
        for (int i = 0; i < lsImgStars.Count; i++)
        {
            if(i < starCount) lsImgStars[i].gameObject.SetActive(true);
            else  lsImgStars[i].gameObject.SetActive(false);
        }
    }

    private void HandleRate()
    {
        //NOTE: Them logic rate 
        Close();
    }
}