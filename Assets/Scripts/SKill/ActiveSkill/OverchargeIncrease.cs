using UnityEngine;

[CreateAssetMenu(fileName = "OverchargeIncrease", menuName = "Scriptable Objects/Skill/SelfOverChargeSkill")]
public class OverchargeIncrease : TileSkillBase
{
    [SerializeField]private int _overCharge;
    public int OverCharge => _overCharge;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int growthValue = casterTile.GetApplyGrowth(_overCharge);
        SkillManager.Instance.BoardController.BoardModel.SetOverChargeValue(growthValue);
    }
}
