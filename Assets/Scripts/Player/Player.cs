using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int CharacterId { get; private set; }
    public string CharacterName { get; private set; }
    //시작 타일덱
    public int StartDeckId { get; private set; }
    //최대 hp, 최대 이동력
    public int CharacterHpMax { get; private set; }
    public int MovementPointMax { get; private set; }

    public string Sprite { get; private set; }
    public string Animation { get; private set; }
    public string Sound { get; private set; }
    public string Effect { get; private set; }

    //현재 hp, 현재 이동력
    public int CharacterHpCurrent { get; private set; }
    public int MovementPointCurrent { get; private set; }
    public int Shield { get; private set; }
    //플레이어 사망 이벤트
    public event Action OnPlayerDead;

    private List<TileSO> _playerDeckSO;
    public IReadOnlyList<TileSO> PlayerDeckSO => _playerDeckSO;

    /// <summary>
    /// CSV데이터로 읽어온 데이터를 현재 Player에 적용
    /// </summary>
    public void SetPlayerDataByCSVData(int id, string name, int deckID, int maxHP, int movePoint, string sprite, string animation, string sound)
    {
        CharacterId = id;
        CharacterName = name;
        StartDeckId = deckID;
        CharacterHpMax = maxHP;
        MovementPointMax = movePoint;
        Sprite = sprite;
        Animation = animation;
        Sound = sound;

        //현재 체력, 현재 이동력 초기값 설정
        CharacterHpCurrent = CharacterHpMax;
        PlayerTurnInit();

        //UI 초기화
        UIManager.Instance.PlayerStatusUI.UpdateHP(CharacterHpCurrent, CharacterHpMax);
        UIManager.Instance.PlayerStatusUI.UpdateShield(Shield);
        UIManager.Instance.PlayerStatusUI.UpdateMovePoint(MovementPointCurrent, MovementPointMax);
    }
    /// <summary>
    /// 플레이어 턴이 시작될때 초기화해야할 항목 저장
    /// </summary>
    public void PlayerTurnInit()
    {
        MovementPointCurrent = MovementPointMax;
        Shield = 0;
        UIManager.Instance.PlayerStatusUI.UpdateShield(Shield);
        UIManager.Instance.PlayerStatusUI.UpdateMovePoint(MovementPointCurrent, MovementPointMax);
    }
    /// <summary>
    /// 사망후 플레이어 상태 초기화
    /// </summary>
    public void PlayerStateInit()
    {
        CharacterHpCurrent = CharacterHpMax;
        MovementPointCurrent = MovementPointMax;
        Shield = 0;
    }

    public int TakeHeal(int heal)
    {
        int overHeal = 0;
        CharacterHpCurrent += heal;
        if (CharacterHpCurrent > CharacterHpMax)
        {
            overHeal = CharacterHpCurrent - CharacterHpMax;
            CharacterHpCurrent = CharacterHpMax;
        }

        Debug.Log($"현재 플레이어 체력:{CharacterHpCurrent}");

        UIManager.Instance.PlayerStatusUI.UpdateHP(CharacterHpCurrent, CharacterHpMax);

        return overHeal;
    }
    public void TakeDamage(int damage)
    {
        //플레이어의 쉴드가 있다면 쉴드부터 차감
        if (Shield > damage)
        {
            Shield -= damage;
            damage = 0;
        }
        else
        {
            damage -= Shield;
            Shield = 0;
        }
        //실제 데미지 기반으로 HP차감
        CharacterHpCurrent -= damage;
        Debug.Log($"현재 플레이어 체력:{CharacterHpCurrent}");

        UIManager.Instance.PlayerStatusUI.UpdateHP(CharacterHpCurrent, CharacterHpMax);
        UIManager.Instance.PlayerStatusUI.UpdateShield(Shield);

        if (CharacterHpCurrent < 0)
        {
            OnPlayerDead?.Invoke();
        }
    }
    /// <summary>
    /// 타일 부셔질때 쉴드값을 생성
    /// </summary>
    /// <param name="value"></param>
    public void AddShieldValue(int value)
    {
        Shield += value;
        Debug.Log($"현재 플레이어 쉴드값: {Shield}");

        UIManager.Instance.PlayerStatusUI.UpdateShield(Shield);
    }
    /// <summary>
    /// 플레이어 이동력 1 감소
    /// </summary>
    /// <returns></returns>
    public int PlayerActDo()
    {
        MovementPointCurrent -= 1;

        UIManager.Instance.PlayerStatusUI.UpdateMovePoint(MovementPointCurrent, MovementPointMax);

        return MovementPointCurrent;
    }


    public void PlayerDeckSet()
    {
        if(_playerDeckSO == null)
        {
            //기본 베이스 덱을 쓴다
        }
        else
        {

        }
    }
    public void CheckAndSetPlayerDeck(List<TileSO> initDeckData)
    {
        //플레이어 초기 덱이 비어있으면 기본 덱을 넣는다
        if(_playerDeckSO == null)
        {
            //깊은복사로 넣음
            _playerDeckSO = new List<TileSO>(initDeckData);
        }
    }
}