using UnityEngine;

public class TileDeckTest : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefeb;
    [SerializeField] private BoardController _controller;
    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                _controller.SetTile(i, j, Instantiate(_tilePrefeb).GetComponent<Tile>());
            }
        }
    }
}
