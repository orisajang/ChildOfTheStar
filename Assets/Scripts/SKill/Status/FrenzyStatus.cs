using UnityEngine;

[CreateAssetMenu(fileName = "FrenzyStatus", menuName = "Scriptable Objects/FrenzyStatus")]//
public class FrenzyStatus : TileStatusBase
{
    [SerializeField] private int _atk=1;
    public override void Execute(Tile[,] board, Tile casterTile)//player 인자 필요함. 추후 추가.
    {
        //player.Damage(_atk);
    }
}
