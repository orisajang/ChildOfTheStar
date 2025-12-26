using UnityEngine;

[CreateAssetMenu(fileName = "ConvertStatusToGrowthAndDamage", menuName = "Scriptable Objects/StatusSkill/ConvertStatusToGrowthAndDamage")]
public class ConvertStatusToGrowthAndDamage : TileSkillBase
{
    [SerializeField] private int _damage = 1;

    [SerializeField] private TileStatusBase _tileStatus;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
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

        if (MonsterManager.Instance != null && MonsterManager.Instance._targetMonster != null)
        {
            Debug.Log($"[Skill] 성장 변환 후 총 {finalDamage} 피해를 입힙니다.");
            MonsterManager.Instance._targetMonster.TakeDamage(finalDamage);
        }
    }
}