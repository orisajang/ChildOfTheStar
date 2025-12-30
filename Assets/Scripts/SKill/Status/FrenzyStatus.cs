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
            if (monsters == null || monsters.Count <= 0)
            {
#if UNITY_EDITOR
                Debug.Log("타오르는 용기 발동 실패 : 몬스터 없음");
#endif

                return;
            }


            int randTarget = Random.Range(0, monsters.Count);
            if (monsters[randTarget]!=null)
                monsters[randTarget].TakeDamage(_damage);
#if UNITY_EDITOR
            Debug.Log($"타오르는 용기 발동 : 몬스터 {monsters[randTarget].name}에게 {_damage} 피해");
#endif
        }
        else
        {
#if UNITY_EDITOR
           Debug.Log($"광분 발동 : 플레이어에게 {_damage} 피해");
#endif
            PlayerManager.Instance._player.TakeDamage(_damage);
        }

    }
}
