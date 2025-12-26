using TMPro;
using UnityEngine;

public class TileInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tileName;
    [SerializeField] TextMeshProUGUI _tileDescription;
    [SerializeField] TextMeshProUGUI _tileNum;

    public void UpdateTileInfo(int num,string name,string description)
    {
        _tileName.text = name;
        _tileDescription.text = description;
        _tileNum.text = $"x {num}";
    }
}
