using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIMain : MonoBehaviour
{
    public enum UIMode { Shop, Exchange }
    public UIMode CurrentMode { get; private set; }

    [System.Serializable]
    public class DetailPanelSet
    {
        public GameObject Panel;  
        public TextMeshProUGUI Name;   
        public TextMeshProUGUI Desc;
        public TextMeshProUGUI ColorInfo;
        public TextMeshProUGUI RarityInfo;

        public Button Button;       
    }

    [SerializeField] private Button _buttonnTabShop;
    [SerializeField] private Button _buttonTabExchange;
    [SerializeField] private GameObject _panelShopListArea;      
    [SerializeField] private GameObject _panelExchangeListArea;  

    [SerializeField] private GameObject _slotPrefab;        
    [SerializeField] private Transform _shopContent;        
    [SerializeField] private Transform _exchangeContent;    
    [SerializeField] private Transform _deckContent;   

    [SerializeField] private GameObject _shopTopResourcePanel;

    [SerializeField] private TextMeshProUGUI _textRed;
    [SerializeField] private TextMeshProUGUI _textBlue;
    [SerializeField] private TextMeshProUGUI _textGreen;
    [SerializeField] private TextMeshProUGUI _textWhite;
    [SerializeField] private TextMeshProUGUI _textBlack;

    [SerializeField] private DetailPanelSet _shopBottomPanel;

    [SerializeField] private DetailPanelSet _exchangeTopPanel;

    [SerializeField] private DetailPanelSet _exchangeBottomPanel;

    [SerializeField] private GameObject _popupBuy;
    [SerializeField] private GameObject _popupRemove;
    [SerializeField] private GameObject _popupExchange;
    [SerializeField] private GameObject _popupError;
    [SerializeField] private TextMeshProUGUI _popupErrorText;

    private TileSO _currentSelectedTile; 
    private TileSO _currentTargetTile; 
    private List<TileSO> _selectedExchangeTiles = new List<TileSO>();

    private List<ShopUISlot> _spawnedShopSlots = new List<ShopUISlot>();
    private List<ShopUISlot> _spawnedExchangeSlots = new List<ShopUISlot>();
    private List<ShopUISlot> _spawnedDeckSlots = new List<ShopUISlot>();

    private void Start()
    {
        _buttonnTabShop.onClick.AddListener(OnShopClick);
        _buttonTabExchange.onClick.AddListener(Exchange);

        if (ShopManager.Instance.ShopTileSlots.Count == 0)
            ShopManager.Instance.SuffleShopSlots();

        SwitchMode(UIMode.Shop);
    }
    void OnShopClick()
    {
        SwitchMode(UIMode.Shop);
    }
    void Exchange()
    {
        SwitchMode(UIMode.Exchange);
    }
    public void SwitchMode(UIMode mode)
    {
        CurrentMode = mode;

        _selectedExchangeTiles.Clear();
        _currentTargetTile = null;
        _currentSelectedTile = null;

        _shopTopResourcePanel.SetActive(false);
        _shopBottomPanel.Panel.SetActive(false);
        _exchangeTopPanel.Panel.SetActive(false);
        _exchangeBottomPanel.Panel.SetActive(false);

        if (mode == UIMode.Shop)
        {
            _panelShopListArea.SetActive(true);
            _panelExchangeListArea.SetActive(false);

            _shopTopResourcePanel.SetActive(true); 

            UpdateResourceUI();

            DisPlayShopList();
        }
        else
        {
            _panelShopListArea.SetActive(false);
            _panelExchangeListArea.SetActive(true);
            _exchangeTopPanel.Panel.SetActive(true);

            DisplayExchangeTargetList();
        }

        DisplayDeck(); 
    }

    public void UpdateResourceUI()
    {
            _textRed.text = ColorResourceManager.Instance.GetResource(TileColor.Red).ToString();
            _textBlue.text = ColorResourceManager.Instance.GetResource(TileColor.Blue).ToString();
            _textGreen.text = ColorResourceManager.Instance.GetResource(TileColor.Green).ToString();
            _textWhite.text = ColorResourceManager.Instance.GetResource(TileColor.White).ToString();
            _textBlack.text = ColorResourceManager.Instance.GetResource(TileColor.Black).ToString();
    }

    public void OnSlotClick(ShopUISlot slot, ShopUISlot.SlotType type)
    {
        if (type == ShopUISlot.SlotType.ExchangeTarget)
        {
            _currentTargetTile = slot.Data;
            SetDescriptionlUI(_exchangeTopPanel, slot.Data);

            foreach (Transform child in _exchangeContent)
            {
                var slotContent = child.GetComponent<ShopUISlot>();
                if (slotContent != null) 
                    slotContent.SetExchangeButtonState(false);
            }

            slot.SetExchangeButtonState(true);


            return;
        }

        if (type == ShopUISlot.SlotType.ShopTile)
        {
            _currentSelectedTile = slot.Data;
            _shopBottomPanel.Panel.SetActive(true);
            SetDescriptionlUI(_shopBottomPanel, slot.Data);

            var button = _shopBottomPanel.Button;
            button.gameObject.SetActive(true);

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenBuyPopup);
            return;
        }

        if (type == ShopUISlot.SlotType.DeckTile)
        {
            if (CurrentMode == UIMode.Shop)
            {
                _shopBottomPanel.Panel.SetActive(true);
                SetDescriptionlUI(_shopBottomPanel, slot.Data);
               _shopBottomPanel.Button.gameObject.SetActive(false);
            }
            else
            {
                _exchangeBottomPanel.Panel.SetActive(true);
                SetDescriptionlUI(_exchangeBottomPanel, slot.Data);
            }
        }
    }

    private void SetDescriptionlUI(DetailPanelSet panelSet, TileSO data)
    {
            panelSet.Name.text = data.Name;
            panelSet.Desc.text = data.descriptionText;
            panelSet.ColorInfo.text = $"{(TileColor)data.Color}";
            panelSet.RarityInfo.text = $"{data.Rarity}";
    }

    public void OnDeckSelectChanged(ShopUISlot slot)
    {
        if (slot.IsSelected)
        {
            if (_selectedExchangeTiles.Count >= 3)
            {
                slot.Deselect();
                DisplayErrorPopup("최대 3개까지만 선택 가능합니다.");
                return;
            }
            if (_selectedExchangeTiles.Count > 0 &&
                _selectedExchangeTiles[0] != slot.Data)
            {
                slot.Deselect();
                DisplayErrorPopup("동일한 타일만 선택 가능합니다.");
                return;
            }
            _selectedExchangeTiles.Add(slot.Data);
        }
        else
        {
            _selectedExchangeTiles.Remove(slot.Data);
        }

        _exchangeBottomPanel.Panel.SetActive(true);
        SetDescriptionlUI(_exchangeBottomPanel, slot.Data);

    }
    private void PoolingSlotList(List<TileSO> dataList, List<ShopUISlot> poolList, Transform parent, ShopUISlot.SlotType type)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            ShopUISlot slot;

            if (i < poolList.Count)
            {
                slot = poolList[i];
                slot.gameObject.SetActive(true);
            }
            else
            {
                var go = Instantiate(_slotPrefab, parent);
                slot = go.GetComponent<ShopUISlot>();
                poolList.Add(slot);
            }

            slot.Init(dataList[i], type, this);
        }

        for (int i = dataList.Count; i < poolList.Count; i++)
        {
            poolList[i].gameObject.SetActive(false);
        }
    }

    private void DisPlayShopList()
    {
        PoolingSlotList(ShopManager.Instance.ShopTileSlots, _spawnedShopSlots, _shopContent, ShopUISlot.SlotType.ShopTile);
    }

    private void DisplayExchangeTargetList()
    {
        PoolingSlotList(ShopManager.Instance.ShopTileSlots, _spawnedExchangeSlots, _exchangeContent, ShopUISlot.SlotType.ExchangeTarget);
    }

    private void DisplayDeck()
    {
        PoolingSlotList(ShopManager.Instance.UseDeck, _spawnedDeckSlots, _deckContent, ShopUISlot.SlotType.DeckTile);
    }

    public void ConfirmBuy()
    {
        var price = ShopManager.Instance.GetPrice(_currentSelectedTile);

       
        if (price != null && ColorResourceManager.Instance.TryPurchase(price))
        {
            ShopManager.Instance.OnBuyTile(_currentSelectedTile);
            _popupBuy.SetActive(false);
            DisplayDeck();
            UpdateResourceUI();
            DisPlayShopList();
        }
        else
        {
            _popupBuy.SetActive(false);
            DisplayErrorPopup("재화가 부족합니다.");
        }
    }

    public void OpenRemovePopup(TileSO data)
    {
        _currentSelectedTile = data;
        _popupRemove.SetActive(true);
    }
    private void OpenBuyPopup()
    {
        _popupBuy.SetActive(true);
    }

    public void ConfirmRemove()
    {
        ShopManager.Instance.OnSellTile(_currentSelectedTile);
        _popupRemove.SetActive(false);
        DisplayDeck();
    }
    public void CheckExchangeCondition()
    {
        _popupExchange.SetActive(true);
        DisplayExchangeTargetList();
    }

    public void ConfirmExchange()
    {
        bool isSuccess = ShopManager.Instance.OnTargetExchange(_selectedExchangeTiles, _currentTargetTile);

        if (isSuccess)
        {
            _selectedExchangeTiles.Clear();
            _popupExchange.SetActive(false);
            _currentTargetTile = null;

            SwitchMode(UIMode.Exchange);

        }
        else
        {
            _popupExchange.SetActive(false);
            DisplayErrorPopup("교환에 실패했습니다.\n(재료 부족 또는 오류)");
        }
    }

    private void DisplayErrorPopup(string str)
    {
        _popupErrorText.text = str;
        _popupError.SetActive(true);
    }
}