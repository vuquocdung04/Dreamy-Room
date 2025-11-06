
using UnityEngine.UI;

    public class LocalizationBox : BoxSingleton<LocalizationBox>
    {
        public static LocalizationBox Setup()
        {
            return Path(PathPrefabs.LOCALIZATION_BOX);
        }

        public Button btnClose;
        public Button btnCloseByPanel;

        public Button btnEn;
        public Button btnVi;
        protected override void Init()
        {
            btnClose.onClick.AddListener(Close);
            btnCloseByPanel.onClick.AddListener(Close);
            
            btnEn.onClick.AddListener(delegate
            {
                Close();
                GameController.Instance.localizationController.ChangeLanguage(Language.En);
            });
            btnVi.onClick.AddListener(delegate
            {
                Close();
                GameController.Instance.localizationController.ChangeLanguage(Language.Vi);
            });
        }

        protected override void InitState()
        {
            
        }
    }