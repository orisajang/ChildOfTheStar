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
        _holdingTileDict = new Dictionary<int, HoldingTileInfo>();
    }
    private void Start()
    {
        for (int i = 0; i< _tileDeck.DrawDeck.Count; i++)
        {
            _holdingTilesUIs.Add( Instantiate(_effectUIPrefeb, transform).GetComponent<TileInfoUI>());
            _holdingTilesUIs[i].gameObject.SetActive(false);
        }
    }
    private void GetTileInfo()
    {
        foreach(var tile in _tileDeck.DrawDeck)
        {
            if (_holdingTileDict.ContainsKey(tile.Id))
            {
                _holdingTileDict[tile.Id]._tileNum++;
            }

            _holdingTileDict.Add(tile.Id,new HoldingTileInfo(1,tile.name,tile.descriptionText));

        }
    }
    private void OnEnable()
    {
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
            _holdingTilesUIs[uiInfoCount].UpdateTileInfo(_holdingTileDict[key]._tileNum, _holdingTileDict[key]._tileName, _holdingTileDict[key]._tileDescription);
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

    public HoldingTileInfo(int num, string name, string description)
    {
        _tileDescription = description;
        _tileNum = num;
        _tileName = name;
    }
}