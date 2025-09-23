using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProfileItem : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Transform checkMark;
    [SerializeField] private Button btn;
    [SerializeField] private bool isAvatar = false;
    [ShowIf("isAvatar")]
    [SerializeField] private Image imgAvatar;

    public void SetId(int idParam) => this.id = idParam;
    public int GetId() => id;

    public void Init(System.Action<ProfileItem> callback = null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(this); });
    }

    public void HandleCheckMark(bool isChecked)
    {
        checkMark.gameObject.SetActive(isChecked);
    }

    public void InitSpriteFrame(Sprite icon)
    {
        imgIcon.sprite = icon;
    }

    public void InitSpriteAvatar(Sprite avatar)
    {
        imgAvatar.sprite = avatar;
    }

    public void SetupOdin()
    {
        imgIcon = transform.Find("icon").GetComponent<Image>();
        checkMark = transform.Find("tick");
        btn = GetComponent<Button>();
        if(isAvatar) imgAvatar = GetComponent<Image>();
    }
}