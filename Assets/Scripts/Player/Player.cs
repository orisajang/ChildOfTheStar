using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _characterId;
    string _characterName;
    //시작 타일덱
    int _startDeckId;
    //최대 hp, 최대 이동력
    int _characterHpMax;
    int _movementPointMax;

    string _sprite;
    string _animation;
    string _sound;
    string _effect;

    //현재 hp, 현재 이동력
    int _characterHpCurrent;
    int _MovementPointCurrent;

    //플레이어 사망 이벤트
    public event Action OnPlayerDead;

    private void Awake()
    {
        _characterHpCurrent = _characterHpMax;
    }

    /// <summary>
    /// CSV데이터로 읽어온 데이터를 현재 Player에 적용
    /// </summary>
    public void SetPlayerDataByCSVData(int id, string name, int deckID,int maxHP, int movePoint, string sprite, string animation, string sound)
    {
        _characterId = id;
        _characterName = name;
        _startDeckId = deckID;
        _characterHpMax = maxHP;
        _movementPointMax = movePoint;
        _sprite = sprite;
        _animation = animation;
        _sound = sound;
    }

    public void TakeDamage(int damage)
    {
        _characterHpCurrent -= damage;
        if(_characterHpCurrent < 0)
        {
            OnPlayerDead?.Invoke();
        }

    }

}
