using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDamangeFrenzy", menuName = "Scriptable Objects/StatusSkill/EnemyDamangeFrenzy")]
public class EnemyDamangeFrenzy : TileSkillBase
{
    protected override void Execute(Tile[,] board, Tile casterTile)
    {

        SkillManager.Instance.notSelfDamagedFrenzy = true;

    }
}