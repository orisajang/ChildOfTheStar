using TMPro;
using UnityEngine;

public class UITileTileeInfoDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tileNameText;
    [SerializeField] TextMeshProUGUI _tileDescriptionText;
    [SerializeField] TextMeshProUGUI _tileRareRankText;
    [SerializeField] TextMeshProUGUI _tileStatusFrenzyText;
    [SerializeField] TextMeshProUGUI _tileStatusRebirthText;
    [SerializeField] TextMeshProUGUI _tileStatusGrowthText;
    [SerializeField] TextMeshProUGUI _tileStatusDestructText;
    [SerializeField] TextMeshProUGUI _tileStatusRecoveryText;

    public void UpdateTile(Tile tile)
    {
        UpdateTileStatusText(
            tile.GetStatusCount(TileStatus.Frenzy),
            tile.GetStatusCount(TileStatus.Rebirth),
            tile.GetStatusCount(TileStatus.Growth),
            tile.GetStatusCount(TileStatus.Destruction),
            tile.GetStatusCount(TileStatus.Recovery)
            );
        UpdateSkilInfo(tile.TileData.name, 
                              tile.TileData.descriptionText, 
                              tile.TileData.Rarity.ToString());
    }
    private void UpdateSkilInfo(string name, string description, string rareRank)
    {
        _tileNameText.text = name;
        _tileDescriptionText.text = description;
        _tileRareRankText.text = $"레어도 {rareRank}";
    }
    private void UpdateTileStatusText(int fenz, int rebirth, int growth, int destruct,int recovery)
    {
        _tileStatusFrenzyText.text = $"x{fenz:D2}";
        _tileStatusRebirthText.text = $"x{rebirth:D2}";
        _tileStatusGrowthText.text = $"x{growth:D2}";
        _tileStatusDestructText.text = $"x{destruct:D2}";
        _tileStatusRecoveryText.text = $"x{recovery:D2}";
    }
}
