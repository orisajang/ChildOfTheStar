using UnityEngine;

public class DungeonButton : MonoBehaviour
{
    [SerializeField] private GameObject lobbyButton;
    [SerializeField] private GameObject dungeonButton;
    //[SerializeField] private GameObject characterButton;
    [SerializeField] private GameObject panel;
    [SerializeField] SpriteRenderer backgroundImage;


    public void Open()
    {
        lobbyButton.SetActive(true);
        panel.SetActive(true);
        dungeonButton.SetActive(false);

        backgroundImage.sprite = Resources.Load<Sprite>("Image/" + "selectstage_bg");

        //RectTransform rect = characterButton.GetComponent<RectTransform>();
        //rect.anchoredPosition = new Vector2(112f, 28f);
    }
}