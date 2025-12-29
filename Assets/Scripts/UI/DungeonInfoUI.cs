using TMPro;
using UnityEngine;

public class DungeonInfoUI : MonoBehaviour
{
    TextMeshProUGUI dungeonInfoText;

    private void Start()
    {
        dungeonInfoText = GetComponent<TextMeshProUGUI>();
        DungeonManager.Instance.SetDungeonText(dungeonInfoText);
    }

}
