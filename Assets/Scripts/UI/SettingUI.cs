using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private GameObject setting;

    public void OpenSetting()
    {
        setting.SetActive(true);
    }

    public void CloseSetting()
    {
        setting.SetActive(false);
    }

    public void SeceneChange()
    {
        GameManager.Instance.GoToLobbyScene();
    }

    public void GoToStageSelectScene()
    {
        DungeonManager.Instance.OnStageInfoInit();
        GameManager.Instance.GoToStageScene();
        ShopManager.Instance.Init();
    }
    public void GameExit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
