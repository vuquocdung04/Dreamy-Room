using Spine.Unity;
using UnityEngine;

    public class Level_8_Phase2 : LevelBase
    {
        public SkeletonAnimation shibaSkeleton;
        public override void Init()
        {
            base.Init();
            var currentLevel = (Level_8)GamePlayController.levelController.currentLevel;
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

        public void HandleShibaAnimation()
        {
            shibaSkeleton.gameObject.SetActive(true);
            shibaSkeleton.AnimationState.SetAnimation(0, "idle", true);
        }
    }