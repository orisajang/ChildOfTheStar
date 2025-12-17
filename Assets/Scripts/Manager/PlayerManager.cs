using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Player _player { get; private set; }
    //플레이어 정보를 CSV에서 읽어오기 위해 추가
    private PlayerCSVLoader _playerCSVLoader = new PlayerCSVLoader();
    private Dictionary<int, PlayerCSVData > _playerCSVDataDic = new Dictionary<int, PlayerCSVData>();
    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();
    }
    private void Start()
    {
        //데이터를 받아옴
        SetPlayerData();
    }

    private void OnEnable()
    {
        _player.OnPlayerDead += PlayerDeadMethod;
    }
    private void OnDisable()
    {
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
    

}
