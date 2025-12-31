    using System.Collections.Generic; 
    using System.IO;
    using UnityEngine;

    [System.Serializable]
    public class DungeonDeckSaveData
    {
        public int dungeonID;          
        public List<int> deckTileIDList;  
    }

    [System.Serializable]
    public class SaveContainer
    {
        public List<DungeonDeckSaveData> dungeonSaveSlots = new List<DungeonDeckSaveData>();
    }
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

        private Dictionary<int, TileSO> _tileIdMap = new Dictionary<int, TileSO>();
        [SerializeField] private List<TileSO> _baseDeck;

        [SerializeField] private List<TileSO> _useDeck = new List<TileSO>();

        [SerializeField] private List<TileSO> _shopTileSlots = new List<TileSO>(30);
        [SerializeField] private int _baseRemovePrice = 100;
        [SerializeField] private int _RemovePriceIncrease = 50;
        [SerializeField] private Dictionary<TileColor, int> _RemovepriceTable = new Dictionary<TileColor, int>();

        public List<TileSO> BaseDeck => _baseDeck;
        public List<TileSO> UseDeck => _useDeck;
        public List<TileSO> ShopTileSlots => _shopTileSlots;

        private int _currentDungeonID = -1;
        public int CurrentDungeonID => _currentDungeonID;

        private SaveContainer _saveContainer = new SaveContainer();
        private string SavePath
        {
            get
            {
                return Application.persistentDataPath + "/DungeonDeckSave.json";
            }
        }
    protected override void Awake()
        {
            base.Awake();
            InitTileDB();

            LoadFromFile();
        if (_useDeck.Count == 0)
            {
                _useDeck.AddRange(_baseDeck);
        }

            InitSellPrice();
        }


    private void InitTileDB()
    {
        _tileIdMap.Clear();


        for (int i = 0; i < _rarityGroup.Count; i++)
        {
            RarityGroup group = _rarityGroup[i];

            for (int j = 0; j < group.tiles.Count; j++)
            {
                TileSO tile = group.tiles[j];
                if (tile != null)
                {
                  _tileIdMap.Add(tile.Id, tile);
                }
            }
        }
    }
    public void Init()
        {
    #if UNITY_EDITOR
            Debug.Log("ShopManager Init");
    #endif
            _useDeck.Clear();
            _useDeck.AddRange(_baseDeck);

        }

        public void InitSellPrice()
        {
            foreach (TileColor color in System.Enum.GetValues(typeof(TileColor)))
            {
                _RemovepriceTable[color] = _baseRemovePrice;
            }
        }
        public void SuffleShopSlots()
        {
            _shopTileSlots.Clear();
            int totalSlots = 30;

            for (int i = 0; i < totalSlots; i++)
            {
                TileSO tile = GetRandomTile(GetRandomRarity());

                if (tile != null) _shopTileSlots.Add(tile);
            }
        }


        public void OnBuyTile(TileSO tileToBuy)
        {
            _useDeck.Add(tileToBuy);
            _shopTileSlots.Remove(tileToBuy);
            SoundManager.Instance.PlayEffect("sfx_shop");
        }

        public void OnSellTile(TileSO tileToSell)
        {
            if (_useDeck.Contains(tileToSell))
            {
                _useDeck.Remove(tileToSell);
                SoundManager.Instance.PlayEffect("sfx_shop");
            }
        }

        public bool OnTargetExchange(List<TileSO> inputTiles, TileSO targetTile)
        {
            if (inputTiles == null || inputTiles.Count != 3)
                return false; 

            TileColor baseColor = inputTiles[0].Color;
            foreach (var target in inputTiles)
            {
                if (target.Color != baseColor) 
                    return false; 
                if (!_useDeck.Contains(target)) 
                    return false; 
            }

            foreach (var target in inputTiles)
            {
                _useDeck.Remove(target);
            }

            _useDeck.Add(targetTile);

            _shopTileSlots.Remove(targetTile);
            SoundManager.Instance.PlayEffect("sfx_shop");
            return true; 
        }
        private Rarity GetRandomRarity()
        {
            int rnd = Random.Range(0, 100);

            if (rnd < 30)
                return Rarity.Common;
            if (rnd < 55)
                return Rarity.Uncommon;
            if (rnd < 75)
                return Rarity.Rare;
            if (rnd < 90) return
                    Rarity.Epic;


            return Rarity.Legendary;
        }

        private TileSO GetRandomTile(Rarity rarity)
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


        public Dictionary<TileColor, int> GetPrice(TileSO tile)
        {
            var priceTable = new Dictionary<TileColor, int>();

            int rarity = tile.Rarity;
            TileColor mainColor = tile.Color;

            if (rarity <= 3)
            {
                int cost = 0;
                if (rarity == 1)
                    cost = 5;
                else if (rarity == 2)
                    cost = 10;
                else cost = 15;

                priceTable.Add(mainColor, cost);
            }
            else
            {
                int mainCost = 0;
                if (rarity == 4)
                    mainCost = 20;
                else
                    mainCost = 25;

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

        public Dictionary<TileColor, int> GetRemovePrice(TileSO tile)
        {
            var priceTable = new Dictionary<TileColor, int>();

            TileColor mainColor = tile.Color;
            if (_RemovepriceTable.ContainsKey(mainColor))
            {
                priceTable.Add(mainColor, _RemovepriceTable[mainColor]);
            }
            return priceTable;
        }

        public void IncreaseRemovePrice(TileColor color)
        {
            if (_RemovepriceTable.ContainsKey(color))
            {
                _RemovepriceTable[color] += _RemovePriceIncrease;
            }
        }


    public void EnterDungeon(int dungeonId)
    {
        _currentDungeonID = dungeonId;

        DungeonDeckSaveData targetSlot = null;
        for (int i = 0; i < _saveContainer.dungeonSaveSlots.Count; i++)
        {
            if (_saveContainer.dungeonSaveSlots[i].dungeonID == dungeonId)
            {
                targetSlot = _saveContainer.dungeonSaveSlots[i];
                break;
            }
        }

        _useDeck.Clear();

        if (targetSlot != null && targetSlot.deckTileIDList.Count > 0)
        {
            for (int i = 0; i < targetSlot.deckTileIDList.Count; i++)
            {
                int id = targetSlot.deckTileIDList[i];
                if (_tileIdMap.TryGetValue(id, out TileSO tile))
                {
                    _useDeck.Add(tile);
                }
            }
        }
        else
        {
            _useDeck.AddRange(_baseDeck);
            SaveCurrentDeck();
        }
    }

    public void SaveCurrentDeck()
    {
        if (_currentDungeonID == -1) 
            return;

        DungeonDeckSaveData targetSlot = null;
        for (int i = 0; i < _saveContainer.dungeonSaveSlots.Count; i++)
        {
            if (_saveContainer.dungeonSaveSlots[i].dungeonID == _currentDungeonID)
            {
                targetSlot = _saveContainer.dungeonSaveSlots[i];
                break;
            }
        }

        if (targetSlot == null)
        {
            targetSlot = new DungeonDeckSaveData();
            targetSlot.dungeonID = _currentDungeonID;
            targetSlot.deckTileIDList = new List<int>();
            _saveContainer.dungeonSaveSlots.Add(targetSlot);
        }

        targetSlot.deckTileIDList.Clear();

        for (int i = 0; i < _useDeck.Count; i++)
        {
            TileSO tile = _useDeck[i];
            if (tile != null)
            {
                targetSlot.deckTileIDList.Add(tile.Id);
            }
        }


        string json = JsonUtility.ToJson(_saveContainer, true);
        File.WriteAllText(SavePath, json);
    }

    private void LoadFromFile()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            _saveContainer = JsonUtility.FromJson<SaveContainer>(json);
            if (_saveContainer == null) _saveContainer = new SaveContainer();
        }
    }


}


