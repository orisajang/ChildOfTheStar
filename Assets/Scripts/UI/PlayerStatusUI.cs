using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    // 캐릭터 공력력 방어력은 기획서 초반에 사라졌고 HP, 이동력, 쉴드 
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI movePointText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private Image hpImage;

    public void UpdateHP(int currentHp, int maxHp)
    {
        float ratio = (float)currentHp / maxHp;
        hpImage.fillAmount = ratio;
        hpText.text = $"{currentHp} / {maxHp}";
    }
    public void UpdateMovePoint(int movePoint, int maxMovePoint)
    {
        movePointText.text = $"MovePoint: {movePoint} / {maxMovePoint}";
    }
    public void UpdateShield(int shield)
    {
        shieldText.text = $"Shield: {shield}";
    }
}