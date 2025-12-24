using UnityEngine;

[CreateAssetMenu(fileName = "ReduceOverchargeByIncrease", menuName = "Scriptable Objects/Skill/ReduceOverchargeByIncrease")]
public class ReduceOverchargeByIncrease : TileSkillBase
{
    [Tooltip("과충전 n만큼 상승 할 때마다")]
    [SerializeField] private int _chargeAmount = 5; 

    [Tooltip("과충전 n만큼 감소시킴")]
    [SerializeField] private int _reduceAmount = 1; 

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int currentTotal = SkillManager.Instance.TotalOverchargeIncrease; 
        int lastAmount = SkillManager.Instance.LastOverchargeIncrease;          
        int prevTotal = currentTotal - lastAmount;                          

        int prevNum = prevTotal / _chargeAmount;
        int currentNum = currentTotal / _chargeAmount;

      
        int triggerCount = currentNum - prevNum;

        if (triggerCount > 0)
        {
            int totalReduce = triggerCount * _reduceAmount;
          
            SkillManager.Instance.BoardController.BoardModel.SetOverChargeValue(-totalReduce);
          
        }
    }
}