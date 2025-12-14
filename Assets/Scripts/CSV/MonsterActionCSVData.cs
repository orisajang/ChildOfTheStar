using UnityEngine;

public class MonsterActionCSVData
{
    //CSV파일에 저장되어있는 속성들
    public int id;
    public string actionName;
    public eMonsterAction actionType;
    public int actionMonsterId;
    public int actionTarget;
    public int actionCost;
    public bool isAttack;
    public bool isRestore;
    public bool isSpecial;
    public float specialValue;
    public string sprite;
    public string animation;
    public string effect;
    public string sound;
}
