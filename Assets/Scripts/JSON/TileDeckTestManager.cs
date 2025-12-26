using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class TileDeckTestManager : Singleton<TileDeckTestManager>
{
    //타일덱의 id별로 종류가 몇개가 있는지 저장하는 딕셔너리
    //Dictionary<int, int> tileDeckIdDic = new Dictionary<int, int>();
    Dictionary<int, TileDeckDataJson> tileDeckIdDic = new Dictionary<int, TileDeckDataJson>();
    List<TileDeckDataJson> tileDeckDataList = new List<TileDeckDataJson>();
    //통합
    PlayerDataJson _playerDataJson;
    string filePath;
    //테스트용 (삭제예정. 버튼클릭하면 저장되게)
    [SerializeField] Button saveButton;

    protected override void Awake()
    {
        isDestroyOnLoad = false;
        base.Awake();
        if (Instance != this) return;
        SetFilePath();
    }
    private void SetFilePath()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        Debug.Log("파일경로: " + filePath);
    }
    public void MakeInitSaveData()
    {
        if (filePath == "" || filePath == null) SetFilePath();
        PlayerDataJson playerDataJson = new PlayerDataJson();
        playerDataJson.currentDengeonNumber = 1;
        string json = JsonUtility.ToJson(playerDataJson, true);
        File.WriteAllText(filePath, json);
        Debug.Log("플레이어 초기 데이터 저장완료");
    }
    private void OnEnable()
    {
        //saveButton.onClick.AddListener(() => SaveMethod());
    }
    private void OnDisable()
    {
       // saveButton.onClick.RemoveAllListeners();
    }
    public void SaveMethod()
    {
        //설정을 하고,
        SetTileDeckInfoForJson();
        //저장
        SaveData();
    }
    /// <summary>
    /// 저장할 데이터를 불러와서 정보 설정
    /// </summary>
    public void SetTileDeckInfoForJson()
    {
        //플레이어가 가지고있는 타일덱을 불러오는 기능(안쓰게 되어서 주석처리), 시련의별 기능때 필요할거같아서 남김
        //SetPlayerDeckDec();
        //_playerDataJson에 정보를 채워준다
        _playerDataJson = new PlayerDataJson();

        //플레이어가 가지고있는ㄷ 타일색상의 데이터를 얻기위해 ColorResourceManager에서 데이터 가져와서 저장형식으로 데이터 가공
        IReadOnlyDictionary<TileColor, int> tileColorDataDic = ColorResourceManager.Instance.ColorResourceDic;
        List<ColorResourceDataJson> colorDataList = new List<ColorResourceDataJson>();
        foreach(var color in tileColorDataDic.Keys)
        {
            //데이터 가공
            ColorResourceDataJson colorResourceDataBuf = new ColorResourceDataJson();
            colorResourceDataBuf.color = color;
            colorResourceDataBuf.amount = tileColorDataDic[color];
            colorDataList.Add(colorResourceDataBuf);
        }
        _playerDataJson.colorResourceDataList = colorDataList;
        _playerDataJson.currentDengeonNumber = DungeonManager.Instance.CurrentDungeonNumber;
        _playerDataJson.currentStageNumber = DungeonManager.Instance.CurrentStageNumber;
        //_playerDataJson.playerTileDeck = new List<TileDeckDataJson>(tileDeckDataList);
    }

    /// <summary>
    /// //플레이어가 가지고있는 타일덱을 불러오는 기능
    /// </summary>
    public void SetPlayerDeckDec()
    {
        
        //플레이어 덱을 불러온다
        IReadOnlyList<TileSO> playerDeckList = PlayerManager.Instance._player.PlayerDeckSO;
        //딕셔너리에 타일ID별로 타일이 몇개있는지 넣기
        for (int index = 0; index < playerDeckList.Count; index++)
        {
            int currentTileId = playerDeckList[index].Id;
            if (!tileDeckIdDic.ContainsKey(currentTileId))
            {
                //처음 설정해주는경우 정보를 다 넣어준다
                TileDeckDataJson databuf = new TileDeckDataJson();
                databuf.tileId = currentTileId;
                databuf.amount = 1;
                databuf.tileColor = playerDeckList[index].Color;
                databuf.rarity = playerDeckList[index].Rarity;
                tileDeckIdDic[currentTileId] = databuf;
            }
            else
            {
                //갯수만 ++;
                tileDeckIdDic[currentTileId].amount += 1;
            }
        }
        //이제 tileDeckDataList를 저장한다 (JSON은 딕셔너리 저장못해서 리스트로 변환)
        tileDeckDataList = tileDeckIdDic.Values.ToList();
    }

    /// <summary>
    /// JSON으로 저장
    /// </summary>
    public void SaveData()
    {
        
        string json = JsonUtility.ToJson(_playerDataJson, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"플레이어 데이터 저장완료: {filePath}");
    }
    public PlayerDataJson LoadData()
    {
        if (filePath == "" || filePath == null) SetFilePath();
        if (!File.Exists(filePath)) MakeInitSaveData();

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerDataJson playerDataJson = JsonUtility.FromJson<PlayerDataJson>(json);
            Debug.Log("로드 완료");
            return playerDataJson;
        }
        else
        {
            return null;
        }
    }
}
