using UnityEngine;

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
}
