using TMPro;
using UnityEngine;

public class UITileSkileInfoDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _skilNameText;
    [SerializeField] TextMeshProUGUI _skilDescriptionText;
    [SerializeField] TextMeshProUGUI _skilRareRankText;

    public void UpdateTile(Tile tile)
    {
        UpdateSkilInfo(tile.TileData.name, 
                              tile.TileData.descriptionText, 
                              tile.TileData.Rarity.ToString());
    }
    private void UpdateSkilInfo(string name, string description, string rareRank)
    {
        _skilNameText.text = name;
        _skilDescriptionText.text = description;
        _skilRareRankText.text = $"레어도 {rareRank}";
    }
}
