using Unity.Hierarchy;
using UnityEngine;

public class CreateTilePoint : MonoBehaviour
{
    [SerializeField] TileBoardSizeSO _tileBoardSize;

    private float _tileBoardWith;
    private float _tileBoardHeight;

    public float _tileGapX { get; private set; }
    public float _tileGapY { get; private set; }

    private SpriteRenderer tileBoardSprite;

    //TileBoardController.cs 파일에서 사용하기 위해서 정의
    //보드 안쪽에 실제 배치된 타일들의 위치 계산
    public Vector2[,] tempPointPosition { get; private set; }
    //타일의 시작점, 끝점을 저장
    public Vector2 standardTilePositionStart { get; private set; }
    public Vector2 standardTilePositionEnd { get; private set; }

    private void Awake()
    {
        tileBoardSprite = GetComponent<SpriteRenderer>();

        _tileBoardWith = tileBoardSprite.bounds.size.x;
        _tileBoardHeight = tileBoardSprite.bounds.size.y;

        _tileGapX = _tileBoardWith / (_tileBoardSize.xSize + 1);
        _tileGapY = _tileBoardHeight/(_tileBoardSize.ySize + 1);

        var standardPosition = new Vector2(transform.position.x - (_tileBoardWith / 2), transform.position.y + (_tileBoardHeight / 2));
        GameObject tempPoint;

        //포지션의 위치를 지정하기 위해서 추가
        tempPointPosition = new Vector2[_tileBoardSize.xSize, _tileBoardSize.ySize];


        //특정 위치에 point용 게임 오브젝트 생성
        for (int i = 1; i<= _tileBoardSize.ySize; i++)
        {
            for(int j = 1; j<= _tileBoardSize.xSize; j++)
            {
                tempPoint = new GameObject($"point{i}_{j}");
                tempPoint.transform.parent = transform;
                tempPoint.transform.position = standardPosition + new Vector2(j * _tileGapX, (-i) * _tileGapY);

                //TileBoardController에서 사용하기위해 좌표, 디버그용 라인추가
                //좌표 추가
                tempPointPosition[j - 1, i - 1] = tempPoint.transform.position;
            }
        }
        //계산 끝난뒤에 처음시작하는 타일의 위치점과 끝점을 계산
        //1. 타일에서 왼쪽위의 지점
        float tileStartX = tempPointPosition[0, 0].x - (_tileGapX/2);
        float tileStartY = tempPointPosition[0, 0].y + (_tileGapY/2);
        standardTilePositionStart = new Vector2(tileStartX, tileStartY);
        //2. 타일에서 오른쪽 아래의 지점
        float tileEndX = tempPointPosition[_tileBoardSize.xSize-1, _tileBoardSize.ySize-1].x + (_tileGapX/2);
        float tileEndY = tempPointPosition[_tileBoardSize.xSize-1, _tileBoardSize.ySize-1].y - (_tileGapY/2);
        standardTilePositionEnd = new Vector2(tileEndX, tileEndY);
    }

}
