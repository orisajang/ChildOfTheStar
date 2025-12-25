using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Player _player { get; private set; }
    //플레이어 정보를 CSV에서 읽어오기 위해 추가
    private PlayerCSVLoader _playerCSVLoader = new PlayerCSVLoader();
    private Dictionary<int, PlayerCSVData > _playerCSVDataDic = new Dictionary<int, PlayerCSVData>();
    //플레이어의 현재 행동력을 TurnManager에 보내서 TurnManager에서 턴종료를 판단하도록
    public event Action<int> SendPlayerMovePoint;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return; //이거도 추가
        _player = GetComponent<Player>();
        //데이터를 받아옴
        SetPlayerData();
        //이벤트 구독 (싱글톤이라서 Enable말고 Awake
        _player.OnPlayerDead += PlayerDeadMethod;
    }
    private void OnDestroy()
    {
        if (Instance != this) return;
        _player.OnPlayerDead -= PlayerDeadMethod;
    }
    private void SetPlayerData()
    {
        //정보를 불러옴
        _playerCSVDataDic = _playerCSVLoader.LoadData("CharacterData");
        //CSV로 읽어온 데이터를 플레이어에 넣어줌 (지금은 플레이어 1개이므로 101로 지정)
        PlayerCSVData playerData = _playerCSVDataDic[101];
        _player.SetPlayerDataByCSVData(playerData.id, playerData.name, playerData.startDeckId, playerData.charHP,
            playerData.movePoint, playerData.sprite, playerData.animation, playerData.sound);
    }

    /// <summary>
    /// 플레이어 사망 처리
    /// </summary>
    public void PlayerDeadMethod()
    {
        //게임매니저에 보내거나.. 그런 처리 진행
        Debug.Log("플레이어 사망");
    }
    /// <summary>
    /// 플레이어 행동력 1 감소하는 메서드
    /// </summary>
    public void OnPlayerMovePointDecrease()
    {
        int currentMovePoint = _player.PlayerActDo();
        Debug.Log($"현재 행동력 {currentMovePoint}");
        //플레이어의 현재 행동력값 보내기
        SendPlayerMovePoint?.Invoke(currentMovePoint);
    }



}
