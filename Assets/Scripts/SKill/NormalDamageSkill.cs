using UnityEngine;

[CreateAssetMenu(fileName = "NormalDamageSkill", menuName = "Scriptable Objects/Skill/NormalDamageSkill")]
public class NormalDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        Debug.Log($"적에게 {_damage} 피해");
    }
}
