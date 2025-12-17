using System.Collections.Generic;
using UnityEngine;

public class TileDeck : MonoBehaviour
{
    public int Height = 5;
    public int Width = 6;
    public int PoolSize = 50;
    [SerializeField] private GameObject _tilePrefeb;
    [SerializeField] private BoardController _controller;
    public List<TileSO> BaseDeckSO;

    private List<TileSO> _drawDeck = new List<TileSO>();
    private Queue<Tile> _tilePool = new Queue<Tile>();
    private void Awake()
    {
        InitializePool();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            {
               
                _controller.SetTile(i, j, CreatTile(j, i));
            }
        }
    }
    private void Start()
    {
        _controller.BoardModel.CreateTile = this.CreatTile;
        _controller.BoardModel.ReturnTile = this.ReturnTilePool;
    }

    private void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = Instantiate(_tilePrefeb);
            Tile tileComponent = obj.GetComponent<Tile>();

            obj.SetActive(false); 
            _tilePool.Enqueue(tileComponent);
        }
    }

    private Tile GetTilePool()
    {
        if (_tilePool.Count == 0)
        {
            GameObject obj = Instantiate(_tilePrefeb);
            return obj.GetComponent<Tile>();
        }

        Tile tile = _tilePool.Dequeue();
        tile.gameObject.SetActive(true);
        return tile;
    }

    public void ReturnTilePool(Tile tile)
    {
        tile.gameObject.SetActive(false);
        _tilePool.Enqueue(tile);
    }

    private TileSO DrawTileSO()
    {
        if (_drawDeck.Count <= 0)
        {
            RefillDeck();
        }
        int lastIndex = _drawDeck.Count - 1;
        TileSO item = _drawDeck[lastIndex];
        _drawDeck.RemoveAt(lastIndex);

        return item;
    }

    private void RefillDeck()
    {
        _drawDeck.Clear();
        _drawDeck.AddRange(BaseDeckSO);

        for (int i = _drawDeck.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);

            var temp = _drawDeck[i];
            _drawDeck[i] = _drawDeck[rnd];
            _drawDeck[rnd] = temp;
        }
    }

    private Tile CreatTile(int row, int col)
    {
        Tile newTile = GetTilePool();
        TileSO data = DrawTileSO(); 
        newTile.Init(col, row, data);

        return newTile;
    }

}
