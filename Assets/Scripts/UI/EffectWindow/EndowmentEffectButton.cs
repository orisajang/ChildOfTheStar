using UnityEngine;

public class EndowmentEffectButton : MonoBehaviour
{
    [SerializeField] GameObject _EffectWindow;

    public void OpenWindow()
    {
        _EffectWindow.SetActive(true);
    }
    public void CloseWindow()
    {
        _EffectWindow.SetActive(false);
    }
}
