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
    public int Gold;
    public int ExpPoint;
    public int Level;

    //색상 타일 자원 보유량
    public List<ColorResourceDataJson> colorResourceDataList;
    //일반 던전 덱?
    //??
    //시련의별 던전 덱?
    //??
    //던전 진행도 (몇번째 던전 진행중인지
    public int currentDengeonNumber;
    //스테이지 진행도 (현재 몇던전까지 클리어했었는지, 5던전까지 클리어했는지 판단할때도 사용-> 시련의별을 위해)
    public int currentStageNumber; 
    

}
