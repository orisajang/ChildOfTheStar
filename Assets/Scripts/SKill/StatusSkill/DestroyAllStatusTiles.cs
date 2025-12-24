using UnityEngine;

[CreateAssetMenu(fileName = "DestroyAllStatusTiles", menuName = "Scriptable Objects/ActionSkill/DestroyAllStatusTiles")]
public class DestroyAllStatusTiles : TileSkillBase
{
    [SerializeField] private TileStatus _statusType = TileStatus.Destruction;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        int destroyCount = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;

                if (target.GetStatusCount(_statusType) > 0)
                {
                    target.Destroy();

                    destroyCount++;
                }
            }
        }
    }
}