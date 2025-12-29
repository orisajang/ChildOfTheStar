using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tileName;
    [SerializeField] TextMeshProUGUI _tileDescription;
    [SerializeField] TextMeshProUGUI _tileNum;
    [SerializeField] Image _tileIcon;
    [SerializeField] Image _tileBase;
    [SerializeField] Image _tileRare;

    public void UpdateTileInfo(int num,string name,string description,Sprite icon,Sprite tile, Sprite rare)
    {
        //_tileName.text = name;
        //_tileDescription.text = description;
        //_tileNum.text = $"x {num}";
        _tileName.SetText(name);
        _tileDescription.SetText(description);
        _tileNum.SetText($"x {num}");
        _tileBase.sprite = tile;
        if(icon != null)
            _tileIcon.sprite = icon;
        if(rare != null)
        {
            _tileRare.sprite = rare;
            _tileRare.enabled = true;
        }
    }
}
