using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardViewer : MonoBehaviour
{
    private Tile[,] _tileObject;
    private Vector2[,] _tilePoints;
    private float _tileGapX;
    private float _tileGapY;
    private float _minBoardX;
    private float _maxBoardX;
    private float _minBoardY;
    private float _maxBoardY;
    private float _tileTotalGapX;
    private float _tileTotalGapY;
    private int _overTileIndex;

    //private Coroutine _dropTileCoroutine;
    [SerializeField] float _tileDropSpeed;

    public void InitTilePoints(Vector2[,] points)
    {
        _tilePoints = points;
        _tileGapX = (points[0, 1] - points[0, 0]).x;
        _tileGapY = (points[0, 0] - points[1, 0]).y;
        _minBoardX = points[0, 0].x - _tileGapX / 2;
        _maxBoardX = points[0, points.GetLength(1) - 1].x + _tileGapX / 2;
        _maxBoardY = points[0, 0].y + _tileGapY / 2;
        _minBoardY = points[points.GetLength(0) - 1, 0].y - _tileGapY / 2;
        _tileTotalGapX = (points[0, points.GetLength(1) - 1] - points[0, 0]).x + _tileGapX;
        _tileTotalGapY = (points[0, 0] - points[points.GetLength(0) - 1, 0]).y + _tileGapY;
    }

    public void InitTileObject(BoardModel model)
    {
        _tileObject = model.Tiles;
        for (int i = 0; i < _tileObject.GetLength(0); i++)
        {
            for (int j = 0; j < _tileObject.GetLength(1); j++)
            {
                // ★ null 체크 추가!
                if (_tileObject[i, j] != null)
                {
                    _tileObject[i, j].transform.position = _tilePoints[i, j];
                }
            }
        }
    }

    public void DistroyTileObject(int row, int col)
    {
    }

   
    public void MoveTilesPosition(TileMoveDirection dir, Vector3 toMove, int index)
    {
        int length = 0;
        if (dir == TileMoveDirection.Vertical) length = _tileObject.GetLength(0);
        else if (dir == TileMoveDirection.Horizontal) length = _tileObject.GetLength(1);

        for (int i = 0; i < length; i++)
        {
            if (dir == TileMoveDirection.Horizontal)
            {
                // ★ null 체크 추가!
                if (_tileObject[index, i] == null) continue;

                _tileObject[index, i].transform.Translate(toMove);

                if (_tileObject[index, i].transform.position.x < _minBoardX)
                    _tileObject[index, i].transform.position = _tileObject[index, i].transform.position + new Vector3(_tileTotalGapX, 0, 0);

                if (_tileObject[index, i].transform.position.x > _maxBoardX)
                    _tileObject[index, i].transform.position = _tileObject[index, i].transform.position - new Vector3(_tileTotalGapX, 0, 0);
            }
            else if (dir == TileMoveDirection.Vertical)
            {
                // ★ null 체크 추가!
                if (_tileObject[i, index] == null) continue;

                _tileObject[i, index].transform.Translate(toMove);

                if (_tileObject[i, index].transform.position.y < _minBoardY)
                    _tileObject[i, index].transform.position = _tileObject[i, index].transform.position + new Vector3(0, _tileTotalGapY, 0);

                if (_tileObject[i, index].transform.position.y > _maxBoardY)
                    _tileObject[i, index].transform.position = _tileObject[i, index].transform.position - new Vector3(0, _tileTotalGapY, 0);
            }
        }  // ★ 중괄호 제대로 닫기
    }

    public void DropTile(int srow, int scol, int dropIndex)
    {
        StartCoroutine(DropTileMoving(srow, scol, dropIndex));
    }

    private IEnumerator DropTileMoving(int srow, int scol, int dropIndex)
    {
        bool isMoveComplate= false;
        while(!isMoveComplate)
        {
            _tileObject[srow, scol].transform.position = Vector2.Lerp(_tileObject[srow, scol].transform.position,
                                                                                               _tilePoints[srow + dropIndex, scol],_tileDropSpeed);
            if (Vector2.Distance(_tileObject[srow, scol].transform.position, _tilePoints[srow + dropIndex, scol]) < 0.1f) isMoveComplate = true;
            yield return null;
        }
    }
}