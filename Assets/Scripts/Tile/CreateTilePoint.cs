using UnityEngine;

public class CreateTilePoint : MonoBehaviour
{
    [SerializeField] TileBoardSizeSO _tileBoardSize;

    private float _tileBoardWith;
    private float _tileBoardHeight;

    private float _tileGapX;
    private float _tileGapY;

    private SpriteRenderer tileBoardSprite;
    private void Awake()
    {
        tileBoardSprite = GetComponent<SpriteRenderer>();

        _tileBoardWith = tileBoardSprite.bounds.size.x;
        _tileBoardHeight = tileBoardSprite.bounds.size.y;

        _tileGapX = _tileBoardWith / (_tileBoardSize.xSize + 1);
        _tileGapY = _tileBoardHeight/(_tileBoardSize.ySize + 1);

        var standardPosition = new Vector2(transform.position.x - (_tileBoardWith / 2), transform.position.y + (_tileBoardHeight / 2));
        GameObject tempPoint;
        //특정 위치에 point용 게임 오브젝트 생성
        for (int i = 1; i<= _tileBoardSize.ySize; i++)
        {
            for(int j = 1; j<= _tileBoardSize.xSize; j++)
            {
                tempPoint = new GameObject($"point{i}_{j}");
                tempPoint.transform.parent = transform;
                tempPoint.transform.position = standardPosition + new Vector2(j * _tileGapX, (-i) * _tileGapY);
            }
        }
    }

}
