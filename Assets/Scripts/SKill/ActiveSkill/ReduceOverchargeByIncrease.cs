using UnityEngine;

[CreateAssetMenu(fileName = "ReduceOverchargeByIncrease", menuName = "Scriptable Objects/Skill/ReduceOverchargeByIncrease")]
public class ReduceOverchargeByIncrease : TileSkillBase
{
    [Tooltip("과충전 n만큼 상승 할 때마다")]
    [SerializeField] private int _chargeAmount = 5;

    [Tooltip("과충전 n만큼 감소시킴")]
    [SerializeField] private int _reduceAmount = 1;

    protected override void Execute(Tile[,] board, Tile casterTile, int val)
    {
        int increaseAmount = val;

        if (increaseAmount <= 0) return;

        int currentTotal = SkillManager.Instance.TotalOverchargeIncrease;

        int prevTotal = currentTotal - increaseAmount;

        int prevNum = prevTotal / _chargeAmount;
        int currentNum = currentTotal / _chargeAmount;

        int triggerCount = currentNum - prevNum;

#if UNITY_EDITOR
        Debug.Log($"[계산] 누적 {prevTotal}->{currentTotal} (증가량 +{increaseAmount}) / 결과: {triggerCount}회 발동");
#endif

        if (triggerCount > 0)
        {
            int totalReduce = triggerCount * _reduceAmount;

            SkillManager.Instance.BoardController.BoardModel.SetOverChargeValue(-totalReduce);
        }
    }
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        Execute(board, casterTile, 0);
    }
}