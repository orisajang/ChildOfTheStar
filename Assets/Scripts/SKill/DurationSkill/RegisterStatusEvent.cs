using UnityEngine;

[CreateAssetMenu(fileName = "RegisterStatusEvent", menuName = "Scriptable Objects/RegisterStatusEvent")]
public class RegisterStatusEvent : TileSkillBase
{
    [Header("조건 만족시 선택한 상태가 부여될 때 마다 발동 될 스킬 등록")]
    [SerializeField]TileStatus _status;

    [Tooltip("등록할 스킬")]
    [SerializeField]TileSkillBase _skill;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        SkillManager.Instance.TileEventBus.Register(_status, _skill);
    }
}
