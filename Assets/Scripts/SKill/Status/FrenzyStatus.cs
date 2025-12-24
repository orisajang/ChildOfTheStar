using UnityEngine;

[CreateAssetMenu(fileName = "FrenzyStatus", menuName = "Scriptable Objects/Status/FrenzyStatus")]
public class FrenzyStatus : TileStatusBase
{
    [SerializeField] private int _damage = 1;
    public override void Execute(Tile[,] board, Tile casterTile)
    {  
        if( SkillManager.Instance.notSelfDamagedFrenzy)
        {
            var monsters = MonsterManager.Instance.SpawnedMonster;
            if (monsters == null
                || monsters.Count <= 0)
            {
                return;
            }

            Debug.Log($"광분으로 랜덤 적에게 {_damage} 피해");

            int randTarget = Random.Range(0, monsters.Count);
            if (monsters[randTarget]!=null)
                monsters[randTarget].TakeDamage(_damage);
        }
        else
        {

            Debug.Log("광분으로 인한 자해 데미지");
            PlayerManager.Instance._player.TakeDamage(_damage);
        }
    }
}
