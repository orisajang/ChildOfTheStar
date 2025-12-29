using System.Collections.Generic;
using UnityEngine;

public class DisplayHoldingTileUI : MonoBehaviour
{
    [SerializeField] GameObject _effectUIPrefeb;
    [SerializeField] TileDeck _tileDeck;
    //[SerializeField] int _displayMaxEffect = 20;

    private List<TileInfoUI> _holdingTilesUIs;
    private Dictionary<int, HoldingTileInfo> _holdingTileDict;

    private void Awake()
    {
        _holdingTilesUIs = new List<TileInfoUI>();
    }

    private void GetTileInfo()
    {
        foreach(var tile in _tileDeck.DrawDeck)
        {
            if (_holdingTileDict.ContainsKey(tile.Id))
            {
                _holdingTileDict[tile.Id]._tileNum++;
            }

            _holdingTileDict.Add(tile.Id,new HoldingTileInfo(1,tile.Name,tile.descriptionText, tile.IconSprite,tile.BaseSprite,tile.RareSprite));
            

        }
    }
    private void OnEnable()
    {
        _holdingTileDict = new Dictionary<int, HoldingTileInfo>();
        GetTileInfo();
        UpdateTileInfo();
    }
    private void UpdateTileInfo()
    {
        var keys = _holdingTileDict.Keys;
        if (keys.Count == 0) return;
        int uiInfoCount = 0;
        foreach(var key in keys)
        {
            if(_holdingTilesUIs.Count == uiInfoCount)
            {
                _holdingTilesUIs.Add(Instantiate(_effectUIPrefeb, transform).GetComponent<TileInfoUI>());
            }
            _holdingTilesUIs[uiInfoCount].UpdateTileInfo(_holdingTileDict[key]._tileNum, _holdingTileDict[key]._tileName, _holdingTileDict[key]._tileDescription, _holdingTileDict[key]._tileIcon, _holdingTileDict[key]._tileSprite, _holdingTileDict[key]._rareSprite);
            _holdingTilesUIs[uiInfoCount].gameObject.SetActive(true);
            uiInfoCount++;
        }
    }
    private void OnDisable()
    {
        foreach(var tils in _holdingTilesUIs)
        {
            tils.gameObject.SetActive(false);
        }
    }
}

class HoldingTileInfo
{
    public int _tileNum;
    public string _tileName;
    public string _tileDescription;
    public Sprite _tileIcon;
    public Sprite _tileSprite; 
    public Sprite _rareSprite;

    public HoldingTileInfo(int num, string name, string description,Sprite icon, Sprite tile, Sprite rare)
    {
        _tileDescription = description;
        _tileNum = num;
        _tileName = name;
        _tileSprite = tile;
        if(icon != null)
            _tileIcon = icon;
        if(rare != null)
            _rareSprite = rare;
    }
}