using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public static class MonsterActionClipName
{
    public static string monsterIdleClip = "Dummy_Idle";
    public static string monsterAttackReadyClip = "Dummy_AttackReady";
    public static string monsterAttackClip = "Dummy_Attack";
}
public class MonsterAnimatorFactory
{
    private AnimatorController _baseController;

    public AnimatorOverrideController CreateOverrideController(Dictionary<eMonsterAction,string> actionNameDic)
    {
        if (_baseController == null) _baseController = Resources.Load<AnimatorController>("Monster/MonsterAnimation_base");
        AnimatorOverrideController overrideController = new AnimatorOverrideController(_baseController);

        //기존 Clip리스트 가져오기
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);

        foreach(eMonsterAction actionType in actionNameDic.Keys)
        {
            string newClipName =  actionNameDic[actionType];

            string baseClipName = GetBaseClipName(actionType);
            AnimationClip baseClip = GetBaseClip(baseClipName);
            AnimationClip newClip = LoadClip(newClipName);

            if(baseClip != null && newClip != null)
            {
                for(int i=0; i< overrides.Count; i++)
                {
                    if (overrides[i].Key == baseClip)
                    {
                        overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(baseClip, newClip);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Clip override failed: Base={baseClipName}, New={newClipName}");
            }
        }
        overrideController.ApplyOverrides(overrides);
        return overrideController;
    }

    private string GetBaseClipName(eMonsterAction action)
    {
        switch (action)
        {
            case eMonsterAction.Idle:
                return MonsterActionClipName.monsterIdleClip;
                break;
            case eMonsterAction.AttackReady:
                return MonsterActionClipName.monsterAttackReadyClip;
                break;
            case eMonsterAction.Attack:
                return MonsterActionClipName.monsterAttackClip;
                break;
        }
        return "";
    }

    private AnimationClip GetBaseClip(string name)
    {
        foreach (var clip in _baseController.animationClips)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }
    private AnimationClip LoadClip(string clipName)
    {
        return Resources.Load<AnimationClip>("Monster/" + clipName);
    }
}
