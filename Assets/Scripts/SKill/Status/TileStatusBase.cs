using UnityEngine;

public enum TileStatus
{
    Frenzy,
    Recovery,
    Growth,
    Rebirth,
    Destruction,
}
public abstract class TileStatusBase: ScriptableObject
{
    [SerializeField] private TileStatus _tileStatus;
    public TileStatus TileStatus => _tileStatus;

        /// <summary>
        /// //몬스터배열,player 추가로 받아야함 아직없어서
        /// </summary>
        /// <param name="board"></param>
        /// <param name="casterTile"></param>
    public abstract void Execute(Tile[,] board,Tile casterTile);
}
