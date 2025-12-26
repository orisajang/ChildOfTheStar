using UnityEngine;
using UnityEngine.UI;

public class LuminosityUI : MonoBehaviour
{
    [SerializeField] private Slider luminositySlider;
    [SerializeField] private Image luminosityImg;

    void Start()
    {
        luminosityImg.gameObject.SetActive(true);

        luminositySlider.value = 1f;
        UpdateLuminosity();
    }

    public void UpdateLuminosity()
    {
        float value = luminositySlider.value;

        float alpha = Mathf.Lerp(0f, 0.9f, 1f - value);

        Color c = luminosityImg.color;
        c.a = alpha;
        luminosityImg.color = c;
    }
}