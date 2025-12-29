using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;

    /// <summary>
    /// 임시로 만듬
    /// </summary>
    /// <param name="text"></param>
    public void testStageText()
    {
        //stageText.text = text;
        switch(StageManager.Instance.CurrentStageNumber)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                stageText.text = $"스테이지 {StageManager.Instance.CurrentStageNumber}";
                break;
            case 5:
                stageText.text = $"보스 스테이지";
                break;
        }
    }
}
