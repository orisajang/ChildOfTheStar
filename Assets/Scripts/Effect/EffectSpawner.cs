using System.Collections.Generic;
using UnityEngine;
using static TurnManager;
using static UnityEngine.GraphicsBuffer;

public enum EffectOwner
{
    Monster, Player,None
}

public class EffectSpawner : Singleton<EffectSpawner>
{
    ObjPool<EffectScript> objPool;
    Dictionary<string, ObjPool<EffectScript>> objPoolByEffectNameDic = new Dictionary<string, ObjPool<EffectScript>>();
    [SerializeField] int effectPoolSize = 3;
    //리소스를 미리 어떤거 쓸건지 저장해둔다
    Dictionary<string, EffectScript> effectPrefabDic = new Dictionary<string, EffectScript>();


    protected override void Awake()
    {
        isDestroyOnLoad = false;
        base.Awake();
    }
    private void SetPrefabDictionary(EffectOwner owner, string effectName)
    {
        EffectScript effectResource = null;
        if(owner == EffectOwner.Monster)
        {
            effectResource = Resources.Load<EffectScript>("Effect/Monster/" + effectName);
        }
        else if(owner == EffectOwner.Player)
        {
            effectResource = Resources.Load<EffectScript>("Effect/Player/" + effectName);
        }
        else
        {
            effectResource = Resources.Load<EffectScript>("Effect/" + effectName);
        }
        effectPrefabDic[effectName] = effectResource;
    }

    public void SetEffectPoolData(EffectOwner owner, List<string> effectList)
    {
        foreach(string effectName in effectList)
        {
            //없으면 채워줌
            if(!effectPrefabDic.ContainsKey(effectName))
            {
                SetPrefabDictionary(owner, effectName);
            }
            EffectScript prefab = effectPrefabDic[effectName];
            objPool = new ObjPool<EffectScript>(prefab, effectPoolSize, gameObject.transform);
            objPoolByEffectNameDic[effectName] = objPool;
        }
    }
    //생성 코드
    public EffectScript GetEffectScript(string effectName, Vector3 pos)
    {
        EffectScript effectBuf = objPoolByEffectNameDic[effectName].GetObject();
        effectBuf.effectName = effectName;
        effectBuf.transform.position = pos;
        effectBuf.onEnd += ReturnEffectToPool;
        return effectBuf;
    }
    //오브젝트 반환 (여기는 이벤트형식으로 변경)
    public void ReturnEffectToPool(string effectName, EffectScript effectScript)
    {
        objPoolByEffectNameDic[effectName].ReturnObject(effectScript);
    }
}
