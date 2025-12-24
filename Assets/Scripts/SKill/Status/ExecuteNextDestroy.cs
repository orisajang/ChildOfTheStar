using UnityEngine;

[CreateAssetMenu(fileName = "ExecuteNextDestroy", menuName = "Scriptable Objects/StatusSkill/ExecuteNextDestroy")]
public class ExecuteNextDestroy : TileSkillBase
{
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
            
       SkillManager.Instance.IsExecuteNextDestory = true;

    }
}