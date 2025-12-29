using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject lobbyButton;
    [SerializeField] private GameObject dungeonButton;
    //[SerializeField] private GameObject characterButton;
    [SerializeField] private GameObject panel;
    [SerializeField] SpriteRenderer backgroundImage;

    public void Open()
    {
        lobbyButton.SetActive(false);
        panel.SetActive(false);
        dungeonButton.SetActive(true);

        backgroundImage.sprite = Resources.Load<Sprite>("Image/" + "lobby_bg_1");

        //RectTransform rect = characterButton.GetComponent<RectTransform>();
        //rect.anchoredPosition = new Vector2(40f, 28f);
    }
}
