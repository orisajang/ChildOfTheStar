using UnityEngine;

[CreateAssetMenu(fileName = "NormalShieldSkill", menuName = "Scriptable Objects/Skill/NormalShieldSkill")]
public class NormalShieldSkill : TileSkillBase
{
    [SerializeField]private int _shieldAmout;
    public int ShieldAmout => _shieldAmout;

    public override void Execute()
    {
        Debug.Log($"플레이어 {_shieldAmout} 실드 회복");
    }
}
