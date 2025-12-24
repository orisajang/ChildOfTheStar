using System.Collections.Generic;
using UnityEngine;

public class TileDeck : MonoBehaviour
{
    private  int Height = 5;
    private int Width = 6;
    [SerializeField] private int _poolSize = 50;
    public int PoolSize => _poolSize;
    [SerializeField]private GameObject _tilePrefeb;
    [SerializeField]private BoardController _controller;
    [SerializeField]private List<TileSO> BaseDeckSO;

    [SerializeField]private List<TileSO> _drawDeck = new List<TileSO>();
    private Queue<Tile> _tilePool = new Queue<Tile>();
    private void Awake()
    {
        InitPool();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
               
                _controller.SetTile(i, j, CreatTile(i, j));
            }
        }
    }

    private void OnEnable()
    {
        //Action 및 Func 연결
        _controller.BoardModel.CreateTile = CreatTile;
        _controller.BoardModel.ReturnTile = ReturnTilePool;
    }

    private void OnDisable()
    {
        _controller.BoardModel.CreateTile -= CreatTile;
        _controller.BoardModel.ReturnTile -= ReturnTilePool;
    }

    /// <summary>
    /// 풀 초기 생성 
    /// </summary>
    private void InitPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = Instantiate(_tilePrefeb);
            Tile tileComponent = obj.GetComponent<Tile>();

            obj.SetActive(false); 
            _tilePool.Enqueue(tileComponent);
        }
    }

    /// <summary>
    /// 풀링에서 타일 얻어오기
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 풀링에 타일 반환, 비활성화
    /// </summary>
    /// <param name="tile"></param>
    public void ReturnTilePool(Tile tile)
    {
        tile.gameObject.SetActive(false);
        _tilePool.Enqueue(tile);
    }

    private TileSO DrawTileSO()
    {
        if (_drawDeck.Count <= 0)
        {
            SuffleDeck();
        }
        int lastIndex = _drawDeck.Count - 1;
        TileSO item = _drawDeck[lastIndex];
        _drawDeck.RemoveAt(lastIndex);

        return item;
    }
    /// <summary>
    /// 실제로 사용할 덱 원본에서 복사하여, 피셔에이츠 셔플(중복 방지)
    /// </summary>
    private void SuffleDeck()
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

    /// <summary>
    /// 해당 좌표에 타일 생성, 생성한 타일 반환
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private Tile CreatTile(int row, int col)
    {
        Tile newTile = GetTilePool();
        TileSO data = DrawTileSO(); 
        newTile.Init(row, col, data, ReturnTilePool);


        return newTile;
    }

}
