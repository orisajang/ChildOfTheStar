using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUISlot : MonoBehaviour
{
    public enum SlotType { ShopTile, DeckTile, ExchangeTarget }

    [SerializeField] private Image _tile;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _rare;
    [SerializeField] private TextMeshProUGUI _nameText;

    [SerializeField] private GameObject _shopUIGroup;
    [SerializeField] private TextMeshProUGUI _priceText;

    [SerializeField] private GameObject _deckUIGroup;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Button _Button;
    [SerializeField] private TextMeshProUGUI _ButtonText;
    [SerializeField] private GameObject _Border;

    private TileSO _data;
    private ShopUIMain _main;
    private SlotType _type;
    private bool _isSelected = false;

    public TileSO Data => _data;
    public bool IsSelected => _isSelected;

    public void Init(TileSO data, SlotType type, ShopUIMain main, int count = 1)
    {
       
        _data = data;
        _type = type;
        _main = main;
        _isSelected = false;
        _rare.enabled = false;
        _tile.sprite = data.BaseSprite;
        if(data.IconSprite != null)
            _icon.sprite = data.IconSprite;
        if(data.RareSprite != null)
        {
            _rare.sprite = data.RareSprite;
            _rare.enabled = true;
        }
        _nameText.text = data.Name;

        _Border.SetActive(false);

        if (_type == SlotType.ShopTile)
        {
            _shopUIGroup.SetActive(true);
            _deckUIGroup.SetActive(false);

            var prices = ShopManager.Instance.GetPrice(data);
            _priceText.text = GetPriceText(prices);
        }
        else if (_type == SlotType.DeckTile)
        {
            _shopUIGroup.SetActive(false);
            _deckUIGroup.SetActive(true);
            _countText.gameObject.SetActive(true);
            _countText.text = count.ToString();
            
            if(_main.CurrentMode == ShopUIMain.UIMode.Shop)
            {
                _ButtonText.text = "제거";
            }
            else
            {
                _ButtonText.text = "선택";
            }

            _Button.onClick.RemoveAllListeners();
            _Button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            _shopUIGroup.SetActive(false);
            _deckUIGroup.SetActive(true);
            _countText.gameObject.SetActive(false);

            _ButtonText.text = "선택";

            _Button.onClick.RemoveAllListeners();
            _Button.onClick.AddListener(OnButtonClick);
        }

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnSlotButtonClick);
    }
    private void OnSlotButtonClick()
    {
   
        _main.OnSlotClick(this, _type);
    }
    private void OnButtonClick()
    {
        if (_main.CurrentMode == ShopUIMain.UIMode.Shop)
        {
            if (_type == SlotType.DeckTile)
            {
                _main.OpenRemovePopup(_data);
            }
        }
        else
        {
            if (_type == SlotType.DeckTile)
            {
              
                ToggleSelection();
            }
            else if (_type == SlotType.ExchangeTarget)
            {
               
                if (_ButtonText.text == "선택")
                {
                  
                    _main.OnSlotClick(this, _type);
                }
                else 
                {
                   
                    _main.CheckExchangeCondition();
                }
            }
        }
    }

    public void SetExchangeButtonState(bool isTarget)
    {
        if (_type != SlotType.ExchangeTarget) 
            return;

        _isSelected = isTarget;

        _Border.SetActive(isTarget);

        if (isTarget)
        {
            _ButtonText.text = "교환";
        }
        else
        {
            _ButtonText.text = "선택";
        }
    }

    public void ToggleSelection()
    {
        _isSelected = !_isSelected;
        _Border.SetActive(_isSelected);
        if (_isSelected)
        {
            _ButtonText.text = "해제";
        }
        else
        {
            _ButtonText.text = "선택";
        }

        _main.OnDeckSelectChanged(this);
    }

    public void Deselect()
    {
        _isSelected = false;

        _Border.SetActive(false);
        if (_type == SlotType.DeckTile)
            _ButtonText.text = "선택";
        else if (_type == SlotType.ExchangeTarget)
            _ButtonText.text = "선택";
    }

    private string GetPriceText(Dictionary<TileColor, int> prices)
    {
        string text = "";
        foreach (var color in prices)
        {
            if (color.Key == TileColor.Red) 
                text += $"빨강 별:{color.Value} ";
            else if (color.Key == TileColor.Blue) 
                text += $"파랑 별:{color.Value} ";
            else if (color.Key == TileColor.Green) 
                text += $"초록 별:{color.Value} ";
            else if (color.Key == TileColor.White) 
                text += $"하양 별:{color.Value} ";
            else if (color.Key == TileColor.Black) 
                text += $"검정 별:{color.Value} ";
        }
        return text;
    }
}