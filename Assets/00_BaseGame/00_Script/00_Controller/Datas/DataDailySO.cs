using UnityEngine;

[CreateAssetMenu(fileName = "DataDailySO", menuName = "DATA/DataDaily", order = 0)]
public class DataDailySO : ScriptableObject
{
    public int currentSystemDay = 0;
    public int playerClaimedDay = 0;
    public int totalDaysInCycle = 7;


    public void ResetProgress()
    {
        currentSystemDay = 0;
        playerClaimedDay = 0;
    }

}