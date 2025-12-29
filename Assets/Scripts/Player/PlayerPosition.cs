using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    //인게임 전투에 들어가면 플레이어의 위치를 지정해주는 스크립트
    private void Start()
    {
        PlayerManager.Instance.SetPlayerPosition(transform.position);
    }
}
