using UnityEngine;

public class PigBankBox :BoxSingleton<PigBankBox>
{
    public static PigBankBox Setup()
    {
        return Path(PathPrefabs.PIG_BANK_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}