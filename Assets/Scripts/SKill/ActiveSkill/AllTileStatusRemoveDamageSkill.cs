using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllTileStatusRemoveDamageSkill", menuName = "Scriptable Objects/Skill/AllTileStatusRemoveDamageSkill")]
public class AllTileStatusRemoveDamageSkill : TileSkillBase
{
    [Tooltip("계수")]
    [SerializeField]private int _damage=1;
    public int Damage=> _damage;
    [Tooltip("데미지 계산에 포함시킬 상태이상 종류들")]
    [SerializeField] private List<TileStatus> _targetStatuses;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int row = board.GetLength(0);
        int col = board.GetLength(1);
        int statusCount= 0;

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;

                target.ExecuteStatus(board);
                if (_targetStatuses != null)
                {
                    foreach (var statusType in _targetStatuses)
                    {
                        statusCount += target.GetStatusCount(statusType);
                    }
                }
                target.ClearStatus();
            }
        }
       if(casterTile.HasKeyword(TileKeyword.Link))
        {
            int totalDamage = statusCount * _damage;

            //int growthValue = casterTile.GetApplyGrowth(_damage)* statusNum;
            Debug.Log($"적에게 {totalDamage} 피해");

            if (MonsterManager.Instance._targetMonster != null)
            {
                MonsterManager.Instance._targetMonster.TakeDamage(totalDamage);
            }
        }
       
    }
}
