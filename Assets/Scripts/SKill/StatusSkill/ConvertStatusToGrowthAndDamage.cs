using UnityEngine;

[CreateAssetMenu(fileName = "ConvertStatusToGrowthAndDamage", menuName = "Scriptable Objects/StatusSkill/ConvertStatusToGrowthAndDamage")]
public class ConvertStatusToGrowthAndDamage : TileSkillBase
{
    [SerializeField] private int _damage = 1;

    [SerializeField] private TileStatusBase _tileStatus;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {

        TileColor casterColor = TileColor.White;
        if (casterTile != null)
            casterColor = casterTile.Color;

        int statusCount = 0;

        if (casterTile.StatusDictionarty != null)
        {
            foreach (var statusList in casterTile.StatusDictionarty.Values)
            {
                statusCount += statusList.Count;
            }
        }

        if (statusCount > 0)
        {
            casterTile.ClearStatus();

            for (int i = 0; i < statusCount; i++)
            {
                casterTile.AddStatus(TileStatus.Growth, _tileStatus);
            }

        }

        int finalDamage = casterTile.GetApplyGrowth(_damage);


        var targetMonster = MonsterManager.Instance._targetMonster;

        if (targetMonster == null)
        {
            var monsters = MonsterManager.Instance.SpawnedMonster;
            if (monsters == null || monsters.Count <= 0)
            {
                return;
            }
            int randTarget = Random.Range(0, monsters.Count);
            targetMonster = monsters[randTarget];
        }

        if (targetMonster != null)
        {
            targetMonster.TakeDamage(finalDamage,casterColor);
        }
    }
}