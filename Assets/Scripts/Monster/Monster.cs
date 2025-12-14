using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.U2D;

public enum eMonsterType
{
    Normal = 1 ,Boss
}
public enum eMonsterSize
{
    Small = 1, Medium, Large
}

public class Monster : MonoBehaviour
{
    //몬스터의 필드 (기획서 테이블에 나와있는 순서대로)
    int _monsterId;
    string _monsterName;
    eMonsterType _monsterType;
    eMonsterSize _monsterSize;
    int _monsterHp;
    int _monsterAttackPower;
    int _monsterMaxEnergy;
    int _monsterCycleId;
    //몬스터가 어떤 이미지,애니메이션,음성을 가지고있는지
    string _monsterSprite;
    string _monsterAnimation;
    string _monsterSound;

    //행동정보. 상태패턴으로 해야할듯?
    int _idle;
    int _attackReady;
    int _attack;
    int _attackSpecial;


    int _monsterCurrentEnergy;


    //제약조건
    //1. 스테이지당 최대 소환 객체수 (몬스터매니저)
    //2. 몬스터 턴에서만 행동가능(턴매니저)
    //3. HP 0되면 사망


    //몬스터의 행동 (상태패턴)
    //몬스터는 4가지 행동을 가지고있으며 (Idle, ready,attack,special)
    //턴이 남아있으면 몬스터의 행동 양식에 따라 계속 반복하면서 공격을 한다.
    //몬스터의 행동양식은 반복되어야하므로 List로 저장하고, 그것을 %나머지연산을 통해 계속 어떤 행동해야하는지 찾는다
    //CSV로 읽어야하는 종류가 3종류라 잠시 스탑.. (몬스터정보, 몬스터 행동, 몬스터 행동 정보가.. csv로 되어있음.)


    public void SetMonsterInfo(MonsterCSVData data)
    {
        _monsterId = data.monsterId;
        _monsterName = data.monsterName;
        _monsterType = data.monsterType;
        _monsterSize = data.monsterSize;
        _monsterHp = data.monsterHp;
        _monsterAttackPower = data.monsterAttackPower;
        _monsterMaxEnergy = data.monsterMaxEnergy;
        _monsterCycleId = data.monsterCycleId;
        _monsterSprite = data.monsterSprite;
        _monsterAnimation = data.monsterAnimation;
        _monsterSound = data.monsterSound;
    }

}
