using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int _characterId { get; private set; }
    public string _characterName { get; private set; }
    //시작 타일덱
    public int _startDeckId { get; private set; }
    //최대 hp, 최대 이동력
    public int _characterHpMax { get; private set; }
    public int _movementPointMax { get; private set; }

    public string _sprite { get; private set; }
    public string _animation { get; private set; }
    public string _sound { get; private set; }
    public string _effect { get; private set; }

    //현재 hp, 현재 이동력
    public int _characterHpCurrent { get; private set; }
    public int _MovementPointCurrent { get; private set; }

    //플레이어 사망 이벤트
    public event Action OnPlayerDead;

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

        //현재 체력, 현재 이동력 초기값 설정
        _characterHpCurrent = _characterHpMax;
        SetMovementPointInit();
    }
    /// <summary>
    /// 턴이 시작될때마다 이동력 초기화
    /// </summary>
    public void SetMovementPointInit()
    {
        _MovementPointCurrent = _movementPointMax;
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
