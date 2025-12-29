using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    /// <summary>
    /// Bgm 여러개 넣어두면 그중에서 랜덤 재생
    /// </summary>
    [SerializeField] private string[] bgmList;

    void Start()
    {
        if (bgmList.Length == 0)
        {
            return;
        }

        string selectedBGM = bgmList[Random.Range(0, bgmList.Length)];

        SoundManager.Instance.PlayBGM(selectedBGM);
    }
}