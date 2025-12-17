using UnityEngine;

[CreateAssetMenu(fileName = "NormalDamageSkill", menuName = "Scriptable Objects/Skill/NormalDamageSkill")]
public class NormalDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;

    public override void Execute()
    {
        Debug.Log($"적에게 {_damage} 피해");
    }
}
