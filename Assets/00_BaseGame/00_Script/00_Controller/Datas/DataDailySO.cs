using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDailySO", menuName = "DATA/DataDaily", order = 0)]
public class DataDailySO : ScriptableObject
{
    [SerializeField] private int playerClaimedDay = 0;
    [SerializeField] private int totalDaysInCycle = 6;
    [SerializeField] private bool hasClaimedDay;

    public int GetClaimedDay() => playerClaimedDay;
    public bool HasClaimedDay() => hasClaimedDay;
    public void HandleClaimed()
    {
        playerClaimedDay++;
        hasClaimedDay = true;
    }

    public void ResetProgress()
    {
        if (playerClaimedDay == totalDaysInCycle)
        {
            playerClaimedDay = 0;
        }
        hasClaimedDay = false;
    }


    [Button("ReDay", ButtonSizes.Large)]
    void ReDay()
    {
        UseProfile.FirstTimeOpenGame = TimeManager.GetCurrentTime().AddDays(-1);
    }

}