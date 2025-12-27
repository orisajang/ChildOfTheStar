using System;
using UnityEngine;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    //오브젝트풀 프리팹,사이즈 , 선언
    [SerializeField] MonsterRoot _monsterPrefab;
    [SerializeField] int poolSize = 10;
    ObjPool<MonsterRoot> _monsterRootPool; //기존 오브젝트풀과 다르게 부모를 하나 추가하기위해 MonsterRoot를 넣어서 사용

    protected override void Awake()
    {
        isDestroyOnLoad = false;
        base.Awake();
        _monsterRootPool = new ObjPool<MonsterRoot>(_monsterPrefab, poolSize, gameObject.transform);
    }
    //생성 코드
    public Monster GetMonsterByPool(Transform trf)
    {
        MonsterRoot monParent = _monsterRootPool.GetObject();
        monParent.transform.position = trf.position;
        Monster monsterBuf = monParent.Monster;
        //monsterBuf.transform.position = trf.position;
        monsterBuf.OnMonsterDead += ReturnMonsterToPool;
        return monsterBuf;
    }
    //오브젝트 반환
    private void ReturnMonsterToPool(Monster monster)
    {
        monster.OnMonsterDead -= ReturnMonsterToPool;
        //부모를 가져온다
        MonsterRoot monParent = monster.GetComponentInParent<MonsterRoot>();
        _monsterRootPool.ReturnObject(monParent);
    }
}

