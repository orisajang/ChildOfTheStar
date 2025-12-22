using System;
using UnityEngine;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    //오브젝트풀 프리팹,사이즈 , 선언
    [SerializeField] Monster _monsterPrefab;
    [SerializeField] int poolSize = 10;
    ObjPool<Monster> _monsterPool;

    protected override void Awake()
    {
        isDestroyOnLoad = false;
        base.Awake();
        _monsterPool = new ObjPool<Monster>(_monsterPrefab, poolSize, gameObject.transform);
    }
    //생성 코드
    public Monster GetMonsterByPool(Transform trf)
    {
        Monster monsterBuf = _monsterPool.GetObject();
        monsterBuf.transform.position = trf.position;
        monsterBuf.OnMonsterDead += ReturnMonsterToPool;
        return monsterBuf;
    }
    //오브젝트 반환
    private void ReturnMonsterToPool(Monster expPoint)
    {
        expPoint.OnMonsterDead -= ReturnMonsterToPool;
        _monsterPool.ReturnObject(expPoint);
    }
}
