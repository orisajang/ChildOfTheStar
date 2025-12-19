using TMPro;
using UnityEngine;

public class PlayerStatusUI : MonoBehaviour
{
    // 캐릭터 공력력 방어력은 기획서 초반에 사라졌고 HP, 이동력, 쉴드 
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI movePointText;
    [SerializeField] private TextMeshProUGUI shieldText;

    public void UpdateHP(int currentHp, int maxHp)
    {
        hpText.text = $"HP: {currentHp} / {maxHp}";
    }
    public void UpdateMovePoint(int movePoint, int maxMovePoint)
    {
        movePointText.text = $"MovePoint : {movePoint} / {maxMovePoint}";
    }
    public void UpdateShield(int shield)
    {
        shieldText.text = $"Shield : {shield}";
    }
}