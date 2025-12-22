using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBarUI : MonoBehaviour
{
    [SerializeField] private Image hpFill;

    private Transform _target;
    private int _monstermaxHp;

    /// <summary>
    /// 몬스터 정보 세팅 후 호출
    /// </summary>
    public void Init(Transform target, int monstermaxHp)
    {
        _target = target;
        _monstermaxHp = monstermaxHp;

        UpdateHP(monstermaxHp);
    }

    /// <summary>
    /// 현재 체력 기준으로 HPBar 갱신
    /// </summary>
    public void UpdateHP(int currentHp)
    {
        float ratio = (float)currentHp / _monstermaxHp;
        hpFill.fillAmount = ratio;
    }
}