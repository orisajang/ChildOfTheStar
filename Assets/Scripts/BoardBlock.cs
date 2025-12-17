using UnityEngine;

public class BoardBlock : MonoBehaviour
{
    [SerializeField] private GameObject boardBlocker;
    [SerializeField] private GameObject board;
    /// <summary>
    /// 보드 비/활성화
    /// </summary>
    /// <param name="active">true 또는 false를 넣어서 활성화 또는 비활성화</param>
    public void SetBoardActive(bool active)
    {
        if (active)
        {
            boardBlocker.SetActive(true);
            Debug.Log("보드 활성화");
        }
        else
        {
            boardBlocker.SetActive(false);
            Debug.Log("보드 비활성화");
        }
    }
}
