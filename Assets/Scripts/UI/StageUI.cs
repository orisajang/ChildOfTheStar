using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;

    /// <summary>
    /// 임시로 만듬
    /// </summary>
    /// <param name="text"></param>
    public void testStageText(string text)
    {
        stageText.text = text;
    }
}
