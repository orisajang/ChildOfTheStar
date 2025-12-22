using UnityEngine;

public class EffectUI : MonoBehaviour
{
    // 임시로 해둔거라 껏다 켰다만 해두고 나중에 UIManager랑 연동 예정
    [SerializeField] private GameObject panel;

    private bool isOpen = false;

    public void Open()
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);
    }

    public void Close()
    {
        isOpen = false;
        panel.SetActive(false);
    }
}
