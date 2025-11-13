using System.Collections.Generic;
using EventDispatcher;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EditProfileBox : BoxSingleton<EditProfileBox>
{
    public static EditProfileBox Setup()
    {
        return Path(PathPrefabs.EDIT_PROFILE_BOX);
    }

    public Button btnClose;
    public Button btnTabFrame;
    public Button btnTabAvatar;
    public Button btnSave;
    public Image imgFrameDisplay;
    public Image imgAvatarDisplay;

    public Transform tabFrame;
    public Transform tabAvatar;
    public InputField txtName;

    public List<ProfileItem> lstProfileFrames;
    public List<ProfileItem> lstProfileAvatars;

    [Header("Localization")]
    public LocalizedText lcTitle;

    public LocalizedText lcFrame;
    public LocalizedText lcFrame1;
    public LocalizedText lcAvatar;
    public LocalizedText lcSave;
    
    
    private ProfileItem currentSelectedAvatar;
    private ProfileItem currentSelectedFrame;


    private int initialAvatarId;
    private int initialFrameId;
    private string initialName;


    protected override void Init()
    {
        InitializeAllSpritesSetup();
        btnClose.onClick.AddListener(Close);
        btnTabAvatar.onClick.AddListener(() => SwitchTab(true));
        btnTabFrame.onClick.AddListener(() => SwitchTab(false));
        btnSave.onClick.AddListener(OnClickBtnSave);
        // Gán sự kiện OnClick bằng cách gọi hàm xử lý chung
        OnClick(lstProfileAvatars, (item) => HandleSelection(item, ref currentSelectedAvatar, () =>
        {
            UpdateAvatarDisplay(item.GetId());
            UpdateSaveButtonState();
        }));

        OnClick(lstProfileFrames, (item) => HandleSelection(item, ref currentSelectedFrame, () =>
        {
            UpdateFrameDisplay(item.GetId());
            UpdateSaveButtonState();
        }));

        InitLocalization();
    }

    protected override void InitState()
    {
        InitializeAllSelections();
        UpdateSaveButtonState();
        initialAvatarId = UseProfile.ProfileAvatarDetail;
        initialFrameId = UseProfile.ProfileFrameDetail;
        initialName = UseProfile.UserName;
        
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer,  InitLocalization);
    }

    private void InitLocalization()
    {
        lcTitle.Init();
        lcFrame.Init();
        lcAvatar.Init();
        lcSave.Init();
        lcFrame1.Init();
    }
    

    private void UpdateSaveButtonState()
    {
        bool hasAvatarChanged = currentSelectedAvatar != null && currentSelectedAvatar.GetId() != initialAvatarId;
        bool hasFrameChanged = currentSelectedFrame != null && currentSelectedFrame.GetId() != initialFrameId;
        bool hasNameChanged = txtName.text != initialName;
        
        btnSave.interactable = hasAvatarChanged || hasFrameChanged || hasNameChanged;
    }

    private void OnClickBtnSave()
    {
        UseProfile.ProfileAvatarDetail = currentSelectedAvatar.GetId();
        UseProfile.ProfileFrameDetail = currentSelectedFrame.GetId();
        UseProfile.UserName = txtName.text;
        
        initialAvatarId = UseProfile.ProfileAvatarDetail;
        initialFrameId = UseProfile.ProfileFrameDetail;
        initialName = UseProfile.UserName;
        
        this.PostEvent(EventID.CHANGE_AVATAR);
        
        UpdateSaveButtonState();
    }
    // HÀM MỚI: Khởi tạo checkmark cho tất cả các list
    private void InitializeAllSelections()
    {
        InitializeListSelection(lstProfileAvatars, UseProfile.ProfileAvatarDetail, out currentSelectedAvatar);
        InitializeListSelection(lstProfileFrames, UseProfile.ProfileFrameDetail, out currentSelectedFrame);

        UpdateAvatarDisplay(UseProfile.ProfileAvatarDetail);
        UpdateFrameDisplay(UseProfile.ProfileFrameDetail);
        UpdateNameUser();
    }
    private void InitializeAllSpritesSetup()
    {
        var dataProfile = GameController.Instance.dataContains.dataProfile;
        foreach (var avatarItem in lstProfileAvatars)
        {
            int itemId = avatarItem.GetId();
            Sprite avatarSprite = dataProfile.GetSpriteAvatarById(itemId);
            Sprite frameSprite = dataProfile.GetSpriteFrameById(itemId);
            avatarItem.InitSpriteAvatar(avatarSprite);
            avatarItem.InitSpriteFrame(frameSprite);
        }

        foreach (var frameItem in lstProfileFrames)
        {
            int itemId = frameItem.GetId();
            Sprite itemSprite = dataProfile.GetSpriteFrameById(itemId);
            frameItem.InitSpriteFrame(itemSprite);
        }
    }
    // HÀM MỚI: Xử lý logic chọn item chung
    private void HandleSelection(ProfileItem clickedItem, ref ProfileItem currentItem, System.Action onSelectCallback)
    {
        // Nếu bấm lại item đang chọn thì không làm gì
        if (currentItem == clickedItem) return;

        // Tắt checkmark của item cũ (nếu có)
        if (currentItem != null)
        {
            currentItem.HandleCheckMark(false);
        }

        // Cập nhật item hiện tại và bật checkmark
        currentItem = clickedItem;
        currentItem.HandleCheckMark(true);

        // Thực hiện hành động riêng cho Avatar hoặc Frame
        onSelectCallback?.Invoke();
    }



    // HÀM MỚI: Xử lý logic khởi tạo cho một list cụ thể
    private void InitializeListSelection(List<ProfileItem> items, int selectedId, out ProfileItem currentItem)
    {
        currentItem = null;
        foreach (var item in items)
        {
            bool isSelected = item.GetId() == selectedId;
            item.HandleCheckMark(isSelected);
            if (isSelected)
            {
                currentItem = item;
            }
        }
    }

    private void SwitchTab(bool isAvatarTab)
    {
        tabAvatar.gameObject.SetActive(isAvatarTab);
        tabFrame.gameObject.SetActive(!isAvatarTab);
    }

    private void OnClick(List<ProfileItem> lstProfiles, System.Action<ProfileItem> callback = null)
    {
        foreach (var item in lstProfiles) item.Init(callback);
    }

    private void UpdateFrameDisplay(int id)
    {
        var dataProfile = GameController.Instance.dataContains.dataProfile;
        imgFrameDisplay.sprite = dataProfile.GetSpriteFrameById(id);
    }

    private void UpdateAvatarDisplay(int id)
    {
        var dataProfile = GameController.Instance.dataContains.dataProfile;
        imgAvatarDisplay.sprite = dataProfile.GetSpriteAvatarById(id);
    }


    private void UpdateNameUser()
    {
        txtName.text = UseProfile.UserName;
    }

    [Button("Setup Item")]
    void SetupItem()
    {
        for (int i = 0; i < lstProfileAvatars.Count; i++)
        {
            lstProfileAvatars[i].SetId(i);
            lstProfileAvatars[i].SetupOdin();
        }

        for (int i = 0; i < lstProfileFrames.Count; i++)
        {
            lstProfileFrames[i].SetId(i);
            lstProfileFrames[i].SetupOdin();
        }
    }
}