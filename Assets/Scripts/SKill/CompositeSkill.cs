using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositeSkill", menuName = "Scriptable Objects/Skill/CompositeSkill")]
public class CompositeSkill: TileSkillBase
{
    [SerializeField] List<TileSkillBase> _skills;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        ExecuteWithDepth(board, casterTile, 0);
    }

    private void ExecuteWithDepth(Tile[,] board, Tile casterTile, int currentDepth)
    {
        if (currentDepth > 10)
        {
            Debug.LogError($"재귀 : {currentDepth} 넘음. 스킬 잘못넣은듯 타일 Id :{casterTile.TileData.Id} ");
            return;
        }

        if (_skills == null) return;

        foreach (var skill in _skills)
        {
            if (skill == null) continue;

            if (skill is CompositeSkill compositeSkill)
            {
                compositeSkill.ExecuteWithDepth(board, casterTile, currentDepth + 1);
            }
            else
            {
                skill.TryExecute(board, casterTile);
            }
        }
    }
}
