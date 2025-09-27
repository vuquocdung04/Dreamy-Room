using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BoosterBase : MonoBehaviour
{
    [SerializeField] protected Image boosterIcon;
    [SerializeField] protected TextMeshProUGUI txtBoosterAmount;
    [SerializeField] protected RectTransform transLockedState;
    [SerializeField] protected RectTransform transUnlockedActive;
    [SerializeField] protected RectTransform transUnlockedEmpty;
}