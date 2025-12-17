#if IS_OLD
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CreateTilePoint createTilePoint;

    [SerializeField] private TileBoardView tilePrefab;

    private TileBoardView[,] tileViews;

    private BoardModel model;

    public void Init(BoardModel boardModel)
    {
        model = boardModel;

        tileViews = new TileBoardView[model.Rows, model.Columns];

        // 최초 상태를 화면에 반영
        BoardDraw();
    }

    /// <summary>
    /// model 상태로 다시 그리기
    /// </summary>
    public void BoardDraw()
    {
        Tile[,] tiles = model.Tiles;

        for(int i = 0; i < model.Rows; i++)
        {
            for(int j = 0; j < model.Columns; j++)
            {
                Tile tile = tiles[i, j];

                Vector2 targetPos = createTilePoint.tempPointPosition[j, i];

                // model이 비어있을 때
                if(tile == null)
                {
                    if (tileViews[i, j] != null)
                    {
                        Destroy(tileViews[i, j].gameObject);
                        tileViews[i, j] = null;
                    }
                    continue;
                }
                // view에 타일이 없을 때
                if (tileViews[i, j] == null)
                {
                    tileViews[i, j] = Instantiate(tilePrefab, targetPos,Quaternion.identity, transform);
                }
                else
                {
                    // View가 있으면 위치만 이동
                    tileViews[i, j].tileMoveToPosition(targetPos);
                }
            }
        }
    }
}
#endif