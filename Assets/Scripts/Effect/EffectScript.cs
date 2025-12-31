using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum eEffectType
{
    Animator, Particle
}

public class EffectScript : MonoBehaviour
{
    //이펙트가 애니메이터로 되어있으므로 애니메이터를 가져와야함
    Animator _animator;
    ParticleSystem _particle;
    public string effectName;

    public eEffectType effectType { get; private set; } //현재 이펙트의 타입이 애니메이션 형식인지? 이펙트 형식인지 체크

    public event Action<string,EffectScript> onEnd;
    Coroutine _coroutine;

    private void Awake()
    {
        CheckEffectType();
    }

    /// <summary>
    /// 이펙트가 애니메이터, 혹은 파티클 둘중에 무엇을 가지고있는지 확인
    /// </summary>
    private void CheckEffectType()
    {
        //만약에 Animator컴포넌트가 있다면
        if(transform.TryGetComponent<Animator>(out _animator))
        {
            effectType = eEffectType.Animator;
        }
        else if(transform.TryGetComponent<ParticleSystem>(out _particle))
        {
            effectType = eEffectType.Particle;
        }
        else
        {
            Debug.LogError("이펙트 타입이 없습니다. 뭔가 잘못됨 ");
        }
    }


    private void OnEnable()
    {
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(WaitAnimationEnd());
        }
    }
    private void OnDisable()
    {
        _coroutine = null;
    }

    /// <summary>
    /// 코루틴으로 애니메이션이 끝났는지 체크하는 부분
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAnimationEnd()
    {
        yield return null;
        while(true)
        {
            if (_animator == null) break;
            //애니메이션 1회 실행한 상태인지 체크
            if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime>= 1f)
            {
                //종료 이벤트 발송
                onEnd?.Invoke(effectName, this);
                //코루틴 완전히 종료
                yield break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 유니티에서 제공하는 파티클 끝나면 호출되는 메서드
    /// </summary>
    void OnParticleSystemStopped()
    {
        onEnd?.Invoke(effectName, this);
    }
}
