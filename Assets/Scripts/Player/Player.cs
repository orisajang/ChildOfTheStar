using UnityEngine;

public class Player : MonoBehaviour
{
    int _characterId;
    string _characterName;
    //시작 타일덱
    int _startDectId;
    //최대 hp, 최대 이동력
    int _characterHpMax;
    int _MovementPointMax;

    string _sprite;
    string _animation;
    string _sound;
    string _effect;

    //현재 hp, 현재 이동력
    int _characterHpCurrent;
    int _MovementPointCurrent;


}
