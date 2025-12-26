using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBasedOnWillDestroy", menuName = "Scriptable Objects/StatusSkill/DamageBasedOnWillDestroy")]
public class DamageBasedOnWillDestroy : TileSkillBase
{
    [Tooltip("계수")]
    [SerializeField] private int _damage = 1;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int row = board.GetLength(0);
        int col = board.GetLength(1);
        int destroyCount = 0;

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target != null && target.WillDestroy)
                {
                    destroyCount++;
                }
            }
        }

        if (destroyCount > 0)
        {
            int totalDamage = destroyCount * _damage;
            var monsters = MonsterManager.Instance.SpawnedMonster;
            if (monsters == null|| monsters.Count <= 0)
            {
                return;
            }

            int growthValue = casterTile.GetApplyGrowth(totalDamage);
            Debug.Log($"모든 적에게 {growthValue} 피해");
            for (int i = monsters.Count - 1; i >= 0; i--)
            {
                monsters[i].TakeDamage(growthValue);
            }
        }
    }
}