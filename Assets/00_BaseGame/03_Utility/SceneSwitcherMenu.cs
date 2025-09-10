using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwitcherMenu
{
    // Đường dẫn tới thư mục chứa các scene của bạn.
    private const string SCENE_FOLDER_PATH = "Assets/Scenes/";

    [MenuItem("Open Scene/Loading Scene", priority = 1)]
    static void OpenLoadingScene()
    {
        OpenScene(SCENE_FOLDER_PATH + "LoadingScene.unity");
    }

    [MenuItem("Open Scene/Home Scene", priority = 2)]
    static void OpenHomeScene()
    {
        OpenScene(SCENE_FOLDER_PATH + "HomeScene.unity");
    }

    [MenuItem("Open Scene/Game Play", priority = 3)]
    static void OpenGameScene()
    {
        OpenScene(SCENE_FOLDER_PATH + "GamePlayScene.unity");
    }

    // Hàm trợ giúp để mở scene và xử lý việc lưu các thay đổi
    static void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }
    }
}
#endif