using TMPro;
using UnityEngine;

public class UITileTileeInfoDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tileNameText;
    [SerializeField] TextMeshProUGUI _tileDescriptionText;
    [SerializeField] TextMeshProUGUI _tileRareRankText;
    [SerializeField] TextMeshProUGUI _tileHoldingSkilText;

    public void UpdateTile(Tile tile)
    {
        tile.GetStatusCount(TileStatus.Frenzy);//광분
        tile.GetStatusCount(TileStatus.Rebirth);//윤회
        tile.GetStatusCount(TileStatus.Growth);//성장
        tile.GetStatusCount(TileStatus.Destruction);//파괴
        tile.GetStatusCount(TileStatus.Recovery);//회복
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
}
