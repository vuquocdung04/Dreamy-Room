using UnityEngine;

    public class Level_14_Phase2 : LevelBase
    {
        protected override ItemSlot CreateItemSlotInstance(GameObject go)
        {
            return go.AddComponent<Slot_14>();
        }

        public int GetTotalItemsRequired() => allItems.Count;
        public override void Init()
        {
            base.Init();
            var level14 = (Level_14)GamePlayController.levelController.currentLevel;
            itemsPlacedCorrectly = level14.GetTotalItemRequired();
            totalItemsRequired = level14.totalProgressAllPhase;
            SetActiveBox();
            SetColorBox(Color.yellow);
        }
    }