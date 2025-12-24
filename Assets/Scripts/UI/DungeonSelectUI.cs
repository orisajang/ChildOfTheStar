using UnityEngine.UI;
using UnityEngine;

public class DungeonSelectUI : MonoBehaviour
{
    [SerializeField] private Button[] dungeonButtons;
    [SerializeField] private Image[] dungeonImages;

    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Sprite unlockSprite;

    // 테스트용 코드
    int currentDungeon = 2;

    public void UpdatecurrentDengeon(int currentDengeonNum)
    {
        currentDungeon = currentDengeonNum;
    }

    private void Start()
    {
        DengeonSelect();
    }
    public void DengeonSelect()
    {
        for(int i = 0; i < dungeonButtons.Length; i++)
        {
            bool isUnlocked = i < currentDungeon;

            dungeonButtons[i].interactable = isUnlocked;

            if (isUnlocked)
            {
                dungeonImages[i].sprite = unlockSprite;
            }
            else
            {
                dungeonImages[i].sprite = lockSprite;
            }
        }
    }
}
