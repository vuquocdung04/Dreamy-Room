using UnityEngine;

    public class Level_8_Phase2 : LevelBase
    {
        public override void Init()
        {
            base.Init();
            var currentLevel = (Level_8)gamePlayController.levelController.currentLevel;
            itemsPlacedCorrectly = currentLevel.GetTotalItemsRequired();
            totalItemsRequired += currentLevel.GetTotalItemsRequired();
            HandleFillProgress();
            SetActiveBox();
            SetColorBox(Color.yellow);
        }
        protected override ItemSlot CreateItemSlotInstance(GameObject go)
        {
            return go.AddComponent<Slot_8>();
        }
    }