using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ColorResourceDataJson
{
    //타일색과 가지고있는 갯수
    public TileColor color;
    public int amount; 
}


[System.Serializable]
public class PlayerDataJson
{
    //색상 타일 자원 보유량
    public List<ColorResourceDataJson> colorResourceDataList;
    //던전 진행도 (현재 몇던전까지 클리어했었는지, 5던전까지 클리어했는지 판단할때도 사용-> 시련의별을 위해)
    public int currentDengeonNumber;
    //스테이지 진행도 
    public int currentStageNumber;
    //일반 던전 덱? (저장 안하게되어서 주석. 시련의별 덱이 필요하다면 이런형식으로 쓸수도있음)
    //public List<TileDeckDataJson> playerTileDeck;
    //시련의별 던전 덱?
    //??


}
