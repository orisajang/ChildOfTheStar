using UnityEngine;

public class BoardBlock : MonoBehaviour
{
    [SerializeField] private GameObject boardBlocker;

    public bool _IsBlocked => boardBlocker != null && boardBlocker.activeSelf;
    /// <summary>
    /// 보드 비/활성화
    /// </summary>
    /// <param name="active">true 또는 false를 넣어서 활성화 또는 비활성화</param>
    public void SetBoardActive(bool Active)
    {
        if(Active)
        {
            boardBlocker.SetActive(Active);
            Debug.Log("보드 비활성화");
        }

        else if(Active == false)
        {
            boardBlocker.SetActive(Active);
            Debug.Log("보드 활성화");
        }
    }
}
