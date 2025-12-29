using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tileName;
    [SerializeField] TextMeshProUGUI _tileDescription;
    [SerializeField] TextMeshProUGUI _tileNum;
    [SerializeField] Image _tileIcon;

    public void UpdateTileInfo(int num,string name,string description,Sprite icon)
    {
        //_tileName.text = name;
        //_tileDescription.text = description;
        //_tileNum.text = $"x {num}";
        _tileName.SetText(name);
        _tileDescription.SetText(description);
        _tileNum.SetText($"x {num}");
        _tileIcon.sprite = icon;
    }
}
