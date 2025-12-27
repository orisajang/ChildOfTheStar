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
        SceneManager.LoadScene("LobbyTest");
    }
}
