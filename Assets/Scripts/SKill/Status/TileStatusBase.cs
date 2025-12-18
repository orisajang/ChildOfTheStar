using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Attack(Player player);
}

public abstract class Punch : Weapon
{
    public override void Attack(Player player)
    {
        Debug.Log("playerp.atk");
    }
}
public abstract class Gun : Weapon
{
    int gunAtk = 10;
    public override void Attack(Player player)
    {
        Debug.Log("playerp.atk" + gunAtk);
    }
}
public abstract class Bat : Weapon
{
    public override void Attack(Player player)
    {
        int batAtk = 5;
        Debug.Log("playerp.atk" + batAtk);
    }
}
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
