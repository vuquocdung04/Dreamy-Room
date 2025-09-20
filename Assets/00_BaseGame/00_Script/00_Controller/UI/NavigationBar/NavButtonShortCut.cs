
using UnityEngine;
using UnityEngine.UI;

public class NavButtonShortCut : MonoBehaviour
    {
        public ENavType navType;
        public Button button;
        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            HomeController.Instance.navController.GoToScreen(navType);
        }
    }