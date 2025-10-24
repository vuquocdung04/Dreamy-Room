using System;
using UnityEngine;

public class L10_WardoreDoorClose : MonoBehaviour
{
    [SerializeField] private L10_WardoreDoorOpen doorOpen;
    [SerializeField] private Collider2D coll;

    public void Init()
    {
        gameObject.SetActive(true);
        coll.enabled = false;
    }
    
    public void EnableColl() => coll.enabled = true;
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        doorOpen.gameObject.SetActive(true);
        doorOpen.SetOpenedItems(true);
    }

    public void HandleDoneState()
    {
        gameObject.SetActive(true);
        coll.enabled = false;
    }
}