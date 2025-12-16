using UnityEngine;

public class TileDeckTest : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefeb;
    [SerializeField] private TileBoardController _controller;
    private void Awake()
    {
        for(int i = 0;i<6;i++)
        {
            for(int j = 0;j<5;i++)
            {
                _controller.InitTiles(i, j, Instantiate(_tilePrefeb).GetComponent<Tile>());
            }
        }
    }
}
