using UnityEngine;
using UnityEngine.UI;

public class ChangButtonImge : MonoBehaviour
{
    [SerializeField] private Image shopButtonImage;
    [SerializeField] private Sprite shopOnSprite;
    [SerializeField] private Sprite shopOffSprite;

    [SerializeField] private Image exchangeButtonImage;
    [SerializeField] private Sprite exchangeOnSprite;
    [SerializeField] private Sprite exchangeOffSprite;

    public void SetShopModeUI()
    {
        shopButtonImage.sprite = shopOnSprite;
        exchangeButtonImage.sprite = exchangeOffSprite;
    }

    public void SetExchangeModeUI()
    {
        shopButtonImage.sprite = shopOffSprite;
        exchangeButtonImage.sprite = exchangeOnSprite;
    }
}
