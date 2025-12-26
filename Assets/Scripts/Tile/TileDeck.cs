using System.Collections.Generic;
using UnityEngine;

public class TileDeck : MonoBehaviour
{
    private int Height = 5;
    private int Width = 6;
    [SerializeField] private int _poolSize = 50;
    public int PoolSize => _poolSize;
    [SerializeField]private GameObject _tilePrefeb;
    [SerializeField]private BoardController _controller;
    // 덱매니저로 옮겼습니다.
    // [SerializeField]private List<TileSO> _baseDeckSO; 
    // 덱매니저로 옮겼습니다.
    // [SerializeField]private List<TileSO> _copyDeck = new List<TileSO>(); 

    [SerializeField]private List<TileSO> _drawDeck = new List<TileSO>();
    private Queue<Tile> _tilePool = new Queue<Tile>();
    private void Awake()
    {
        InitPool();
        SetTileOnBoard();
    }

    public void SetBoardController(BoardController controller)
    {
        _controller = controller;
    }
    public void SetTileOnBoard()
    {
        if (_controller != null)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {

                    _controller.SetTile(i, j, CreatTile(i, j));
                }
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
        if (_controller.BoardModel.Tiles[tile.Row, tile.Col] == tile)
        {
            //보드 이차원배열안에서 해당위치 null로 만들기
            _controller.BoardModel.RemoveTile(tile);
        }
        tile.gameObject.SetActive(false);
        _tilePool.Enqueue(tile);
        
    }

    private TileSO DrawTileSO()
    {
        int lastIndex = _drawDeck.Count - 1;
        TileSO item = _drawDeck[lastIndex];
        _drawDeck.RemoveAt(lastIndex);

        if (_drawDeck.Count <= 0)
        {
            SuffleDeck();
        }
        return item;
    }
    /// <summary>
    /// 실제로 사용할 덱 원본에서 복사하여, 피셔에이츠 셔플(중복 방지)
    /// </summary>
    private void SuffleDeck()
    {
        _drawDeck.Clear();
        _drawDeck.AddRange(DeckManager.Instance.GetUseDeck());

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
