using UnityEngine;

[CreateAssetMenu(fileName = "FrenzyStatus", menuName = "Scriptable Objects/Status/FrenzyStatus")]
public class FrenzyStatus : TileStatusBase
{
    [SerializeField] private int _damage = 1;
    public override void Execute(Tile[,] board, Tile casterTile)
    {
        SkillManager.Instance.TileEventBus.TriggerEvent(TileStatus);
        if ( SkillManager.Instance.notSelfDamagedFrenzy)
        {
            var monsters = MonsterManager.Instance.SpawnedMonster;
            if (monsters == null
                || monsters.Count <= 0)
            {
                return;
            }


            int randTarget = Random.Range(0, monsters.Count);
            if (monsters[randTarget]!=null)
                monsters[randTarget].TakeDamage(_damage);
        }
        else
        {

            PlayerManager.Instance._player.TakeDamage(_damage);
        }

    }
}
