using UnityEngine;

[CreateAssetMenu(fileName = "NormalShieldSkill", menuName = "Scriptable Objects/Skill/NormalShieldSkill")]
public class NormalShieldSkill : TileSkillBase
{
    [SerializeField]private int _shieldAmout;
    public int ShieldAmout => _shieldAmout;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int growthValue = casterTile.GetApplyGrowth(_shieldAmout);

        //여기서 플레이어 쉴드 처리
        PlayerManager.Instance._player.AddShieldValue(growthValue);
    }
}
