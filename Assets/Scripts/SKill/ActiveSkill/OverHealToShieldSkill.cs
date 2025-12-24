using UnityEngine;

[CreateAssetMenu(fileName = "OverHealToShieldSkill", menuName = "Scriptable Objects/Skill/OverHealToShieldSkill")]
public class OverHealToShieldSkill : TileSkillBase
{
    [SerializeField]private int _heal;
    public int Heal=> _heal;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        PlayerManager.Instance._player.OverHealToshield = true;
    }
}
