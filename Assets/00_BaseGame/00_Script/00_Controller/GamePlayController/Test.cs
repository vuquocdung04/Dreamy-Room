using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(delegate
        {
            GameController.Instance.effectChangeScene.FadeToScene(SceneName.GAME_PLAY);
        });
    }
}
