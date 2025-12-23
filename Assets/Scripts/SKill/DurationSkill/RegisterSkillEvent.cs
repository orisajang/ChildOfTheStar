using UnityEngine;

[CreateAssetMenu(fileName = "RegisterSkillEvent", menuName = "Scriptable Objects/RegisterSkillEvent")]
public class RegisterSkillEvent : TileSkillBase
{
    [Header("조건 만족시 선택한 행동이 실행 될 때 마다 발동 될 스킬 등록 ")]
    [SerializeField]SkillEventType _eventType;

    [Tooltip("등록할 스킬")]
    [SerializeField]TileSkillBase _skill;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        SkillManager.Instance.TileEventBus.Register(_eventType, _skill);
    }
}
