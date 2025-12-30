using UnityEngine;

[CreateAssetMenu(fileName = "SelfDamageSkill", menuName = "Scriptable Objects/Skill/SelfDamageSkill")]
public class SelfDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int growthValue = casterTile.GetApplyGrowth(_damage);
        PlayerManager.Instance._player.TakeDamage(growthValue);

    }
}
