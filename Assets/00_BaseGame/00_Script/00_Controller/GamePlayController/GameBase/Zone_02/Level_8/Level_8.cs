using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

    public class Level_8 : LevelBase
    {
        public Level_8_Phase2 level8Phase2;
        public override void Init()
        {
            base.Init();
            level8Phase2.gameObject.SetActive(false);
        }

        public int GetTotalItemsRequired() => totalItemsRequired;
        protected override ItemSlot CreateItemSlotInstance(GameObject go)
        {
            return go.AddComponent<Slot_8>();
        }

        protected override async UniTask OnBeforeWinCompleted()
        {
            GamePlayController.Instance.PauseGame();
            var cameraMain = GamePlayController.playerContains.mainCamera;
            await cameraMain.transform.DOMoveX(0f,0.5f).SetEase(Ease.Linear);
            await  cameraMain.transform.DOMoveX(3.35f,1f).SetEase(Ease.Linear);
        }
        protected override async UniTask OnLevelFinished()
        {
            await UniTask.Yield();
            level8Phase2.gameObject.SetActive(true);
            level8Phase2.Init();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            GamePlayController.Instance.ResumeGame();
        }

    }