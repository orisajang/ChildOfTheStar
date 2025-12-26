using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5
}

public class ShopManager : Singleton<ShopManager>
{
    [System.Serializable]
    public class RarityGroup
    {
        public Rarity rarity;
        public List<TileSO> tiles = new List<TileSO>();
    }

    [SerializeField] private List<RarityGroup> _rarityGroup;

    [SerializeField] private List<TileSO> _baseDeck;

    [SerializeField] private List<TileSO> _useDeck = new List<TileSO>();

    [SerializeField] private List<TileSO> _shopTileSlots = new List<TileSO>();

    public List<TileSO> baseDeck => _baseDeck;
    public List<TileSO> useDeck => _useDeck;
    public List<TileSO> ShopTileSlots => _shopTileSlots;

    protected override void Awake()
    {
        base.Awake();

        if (_useDeck.Count == 0)
        {
            _useDeck = new List<TileSO>(_baseDeck);
        }
    }

    public void RefreshShopSlots()
    {
        _shopTileSlots.Clear();
        int totalSlots = 30;

        for (int i = 0; i < totalSlots; i++)
        {
            TileSO tile = GetRandomTileOfRarity(GetRandomRarity());

            if (tile != null) _shopTileSlots.Add(tile);
        }
    }


    public void OnBuyTile(TileSO tileToBuy)
    {
        var priceTag = GetPrice(tileToBuy);

        if (ColorResourceManager.Instance.TryPurchase(priceTag))
        {
            _useDeck.Add(tileToBuy);
            Debug.Log($"[Shop] 구매 성공: {tileToBuy.Name}");
        }
    }

    public void OnSellTile(TileSO tileToSell)
    {
        if (_useDeck.Contains(tileToSell))
        {
            _useDeck.Remove(tileToSell);
            Debug.Log($"[Shop] 판매 완료: {tileToSell.Name}");
        }
    }

    public void OnExchangeTiles(List<TileSO> selectedTiles)
    {
        if (selectedTiles == null || selectedTiles.Count != 3) return;

        TileColor baseColor = selectedTiles[0].Color;
        foreach (var t in selectedTiles) if (t.Color != baseColor) return;
        foreach (var t in selectedTiles) if (!_useDeck.Contains(t)) return;

        foreach (var t in selectedTiles) _useDeck.Remove(t);

        TileSO newTile = GetRandomTileExcludingColor(baseColor);
        if (newTile != null)
        {
            _useDeck.Add(newTile);
            Debug.Log($"[Shop] 교환 성공: {newTile.Name}");
        }
    }

    private Rarity GetRandomRarity()
    {
        int rnd = Random.Range(0, 100);

        if (rnd < 30) return Rarity.Common;    
        if (rnd < 55) return Rarity.Uncommon; 
        if (rnd < 75) return Rarity.Rare;      
        if (rnd < 90) return Rarity.Epic;      

        return Rarity.Legendary;               
    }

    private TileSO GetRandomTileOfRarity(Rarity rarity)
    {
        foreach (var group in _rarityGroup)
        {
            if (group.rarity == rarity)
            {
                if (group.tiles.Count == 0) 
                    return null;
                return group.tiles[Random.Range(0, group.tiles.Count)];
            }
        }
        return null;
    }

    private TileSO GetRandomTileExcludingColor(TileColor excludeColor)
    {
        List<TileSO> candidates = new List<TileSO>();
        foreach (var group in _rarityGroup)
        {
            foreach (var tile in group.tiles)
            {
                if (tile.Color != excludeColor) candidates.Add(tile);
            }
        }
        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    public Dictionary<TileColor, int> GetPrice(TileSO tile)
    {
        var priceTable = new Dictionary<TileColor, int>();

        int rarity = tile.Rarity; 
        TileColor mainColor = tile.Color;

        if (rarity <= 3)
        {
            int cost = 0;
            if (rarity == 1) cost = 5;
            else if (rarity == 2) cost = 10;
            else cost = 15; 

            priceTable.Add(mainColor, cost);
        }
        else
        {
            int mainCost = 0;
            if (rarity == 4)
                mainCost = 20;
            else mainCost = 25; 

            priceTable.Add(mainColor, mainCost);

            if (mainColor == TileColor.Red || mainColor == TileColor.Blue || mainColor == TileColor.Green)
            {
                int subCost = 0;
                if (rarity == 4) 
                    subCost = 5;
                else subCost = 10;

                if (mainColor == TileColor.Red)
                {
                    priceTable.Add(TileColor.Blue, subCost);
                    priceTable.Add(TileColor.Green, subCost);
                }
                else if (mainColor == TileColor.Blue)
                {
                    priceTable.Add(TileColor.Red, subCost);
                    priceTable.Add(TileColor.Green, subCost);
                }
                else if (mainColor == TileColor.Green)
                {
                    priceTable.Add(TileColor.Red, subCost);
                    priceTable.Add(TileColor.Blue, subCost);
                }
            }
            else
            {
                int subCost = 0;
                if (rarity == 4) subCost = 10;
                else subCost = 20; 

                if (mainColor == TileColor.White)
                    priceTable.Add(TileColor.Black, subCost);
                else if (mainColor == TileColor.Black)
                    priceTable.Add(TileColor.White, subCost);
            }
        }
        return priceTable;
    }
}