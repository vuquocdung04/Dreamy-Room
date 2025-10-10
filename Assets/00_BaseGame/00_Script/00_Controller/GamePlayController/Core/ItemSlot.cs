using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public bool isFullSlot;
    public bool isReadyShow = true;
    public List<ItemSlot> conditionSlots;

    public void Init()
    {
        isReadyShow = conditionSlots.Count == 0;
    }

    public void ValidateReadyState()
    {
        bool allConditionsMet = conditionSlots.All(slot => slot.isFullSlot);
        if (allConditionsMet)
        {
            isReadyShow = true;
        }
    }
    
    public void Active()
    {
        this.gameObject.SetActive(true);
    }

    public void DeActive()
    {
        this.gameObject.SetActive(false);
    }
    
    
}