using UnityEngine;

public class MonsterAction
{
    //몬스터의 행동(액션) 에 대해 정의하기 위해 사용
    //아래 필드들은 전투테이블 csv파일의 필드를 정의한것
    int _id;
    string _actionName;
    eMonsterAction _actionType;
    int _actionMonsterId;
    int _actionTarget;
    int _actionCost;
    bool _isAttack;
    bool _isRestore;
    bool _isSpecial;
    float _specialValue;
    string _animation;
    string _effect;
    string _sound;
}
