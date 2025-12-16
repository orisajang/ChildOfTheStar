using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CreateTilePoint createTilePoint;
    [SerializeField] private TileBoardView[,] tileViews;

    private Transform[,] tilePoints;

    private int rows;
    private int cols;

    private void Awake()
    {
        BuildTilefill();
    }

    /// <summary>
    /// createTilePoint에서 좌표 값 받아오는 용도
    /// </summary>
    private void BuildTilefill()
    {
        Transform parent = createTilePoint.transform;

        rows = 0;
        cols = 0;

        // createTilePoint에서 가져옴
        foreach (Transform child in parent)
        {
            // pointn_m에서 [n, m]으로 추출
            string[] split = child.name.Replace("point", "").Split('_');

            // 숫자 변화
            int row = int.Parse(split[0]);
            int col = int.Parse(split[1]);

            // 갱신
            rows = Mathf.Max(rows, row);
            cols = Mathf.Max(cols, col);
        }

        // 배열 생성
        tilePoints = new Transform[rows, cols];

        // 배열에 넣기
        foreach (Transform child in parent)
        {
            string[] split = child.name.Replace("point", "").Split('_');

            int row = int.Parse(split[0]) - 1;
            int col = int.Parse(split[1]) - 1;

            tilePoints[row, col] = child;
        }
    }

    /// <summary>
    /// 가로줄 이동
    /// </summary>
    /// <param name="rowIndex">몇 번쨰 줄</param>
    /// <param name="moveAmount">몇 칸 이동</param>
    public void MoveRow(int rowIndex, int moveAmount)
    {
        // 가로줄 탐색
        for (int col = 0; col < cols; col++)
        {
            // 현재 위치의 타일
            TileBoardView tile = tileViews[rowIndex, col];

            // 이동 후 도착할 열 인덱스 계산
            int newCol = (col + moveAmount + cols) % cols;

            // 도착해야 할 좌표
            Vector2 targetPos = tilePoints[rowIndex, newCol].position;

            // 타일에게 이동 명령
            tile.tileMoveToPosition(targetPos);
        }
    }
    /// <summary>
    /// 세로줄 이동
    /// </summary>
    /// <param name="colIndex">몇 번째 줄</param>
    /// <param name="moveAmount">몇 칸 이동</param>
    public void MoveColumn(int colIndex, int moveAmount)
    {
        // 세로줄 탐색
        for (int row = 0; row < rows; row++)
        {
            // 현재 위치의 타일
            TileBoardView tile = tileViews[row, colIndex];

            // 이동 후 도착할 행 인덱스 계산
            int newRow = (row + moveAmount + rows) % rows;

            // 도착해야 할 좌표
            Vector2 targetPos = tilePoints[newRow, colIndex].position;

            // 타일에게 이동 명령
            tile.tileMoveToPosition(targetPos);
        }
    }
}
