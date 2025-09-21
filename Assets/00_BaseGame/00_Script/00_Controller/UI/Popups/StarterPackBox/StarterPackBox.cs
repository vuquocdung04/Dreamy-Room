using UnityEngine;

public class StarterPackBox : BoxSingleton<StarterPackBox>
{
    public static StarterPackBox Setup()
    {
        return Path(PathPrefabs.STARTER_PACK_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}