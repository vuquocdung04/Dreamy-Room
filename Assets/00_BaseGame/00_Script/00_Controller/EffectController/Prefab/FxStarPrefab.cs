using UnityEngine;

public class FxStarPrefab : MonoBehaviour
{
    public SpriteRenderer spr;

    public void Init(float randScale)
    {
        transform.localScale = Vector3.one * randScale;
        spr.color = Color.white;
    }
}