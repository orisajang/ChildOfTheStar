
using UnityEngine;
using System.Collections.Generic;
public class Shop
{
    [SerializeField] private TileDeck _tileDeck;
    [SerializeField] private List<TileSO> _shopTileList=new List<TileSO>();


    public void SetTileDeck(TileDeck tileDeck)
    {
        _tileDeck = tileDeck;
    }
}
