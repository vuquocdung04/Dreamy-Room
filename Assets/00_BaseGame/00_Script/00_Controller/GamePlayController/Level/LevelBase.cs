using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    
    [SerializeField] private float snapThreshold;
    [SerializeField] private List<ItemSlot> lsItemSlots;
    [SerializeField] private List<ItemBase> lsItems;
    [SerializeField] private List<ItemBase> lsCompletedItems;
}