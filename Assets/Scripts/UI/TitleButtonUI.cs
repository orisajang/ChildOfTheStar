using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonUI : MonoBehaviour
{
    public void OnTitleButton()
    {
        SceneManager.LoadScene("LobbyTest");
    }
}
