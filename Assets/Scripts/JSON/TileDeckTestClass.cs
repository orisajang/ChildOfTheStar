using System.Collections.Generic;
using UnityEngine;

public class TileDeckTestClass : Singleton<TileDeckTestClass>
{
    //타일덱의 id별로 종류가 몇개가 있는지 저장하는 딕셔너리
    Dictionary<int, int> tileDeckIdDic = new Dictionary<int, int>();
    List<TileDeckDataJson> tileDeckDataList = new List<TileDeckDataJson>();

    public void SetTileDeckInfoForJson()
    {
        //플레이어의 덱을 불러온다
        IReadOnlyList<TileSO> playerDeckList = PlayerManager.Instance._player.PlayerDeckSO;
        //딕셔너리에 타일ID별로 타일이 몇개있는지 넣기
        for (int index = 0; index < playerDeckList.Count; index++)
        {
            int currentTileId = playerDeckList[index].Id;
            if (!tileDeckIdDic.ContainsKey(currentTileId))
            {
                tileDeckIdDic[currentTileId] = 1;
            }
            else
            {
                tileDeckIdDic[currentTileId]++;
            }
        }
        // 딕셔너리를 클래스로 만든다 (리스트)
        foreach (int currentTileId in tileDeckIdDic.Keys)
        {
            TileDeckDataJson dataBuf = new TileDeckDataJson();
            dataBuf.tileId = currentTileId;
            dataBuf.amount = tileDeckIdDic[currentTileId];
            tileDeckDataList.Add(dataBuf);
        }
        //이제 tileDeckDataList를 저장한다
    }
}
