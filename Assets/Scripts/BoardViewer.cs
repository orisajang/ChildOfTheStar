using System.Collections;
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

    public void InitTilePoints(Vector2[,] points)
    {
        _tilePoints = points;
        _tileGapX = (points[0, 1] - points[0, 0]).x;
        _tileGapY = (points[0, 0] - points[1, 0]).y;

        _minBoardX = points[0, 0].x - _tileGapX / 2;
        _maxBoardX = points[0, points.GetLength(1) - 1].x + _tileGapX / 2;
        _maxBoardY = points[0, 0].y + _tileGapY / 2;
        _minBoardY = points[points.GetLength(0) - 1,0].y - _tileGapY / 2;

        _tileTotalGapX = (points[0, points.GetLength(1)-1] - points[0, 0]).x + _tileGapX;
        _tileTotalGapY = (points[0, 0] - points[points.GetLength(0) - 1, 0]).y + _tileGapY;
    }
    public void InitTileObject(BoardModel model)
    {
        _tileObject = model.Tiles;
        for(int i =0; i< _tileObject.GetLength(0);i++)
        {
            for(int j = 0; j < _tileObject.GetLength(1);j++)
            {
                _tileObject[i, j].transform.position = _tilePoints[i, j];
            }
        }
    }

    public void DistroyTileObject(int row, int col)
    {

    }

    public void MoveTilesPosition(TileMoveDirection dir,Vector3 toMove, int index)
    {
        int length = 0;
        if (dir == TileMoveDirection.Vertical) length = _tileObject.GetLength(0);
        else if (dir == TileMoveDirection.Horizontal) length = _tileObject.GetLength(1);

        for (int i = 0; i< length; i++)
        {
            
            if (dir == TileMoveDirection.Horizontal)
            {
                _tileObject[index, i].transform.Translate(toMove);
                if (_tileObject[index, i].transform.position.x < _minBoardX) _tileObject[index, i].transform.position = _tileObject[index, i].transform.position + new Vector3(_tileTotalGapX, 0, 0);
                if (_tileObject[index, i].transform.position.x > _maxBoardX) _tileObject[index, i].transform.position = _tileObject[index, i].transform.position - new Vector3(_tileTotalGapX, 0, 0);
            }
            else if (dir == TileMoveDirection.Vertical)
            {
                _tileObject[ i,index].transform.Translate(toMove);
                if (_tileObject[i, index].transform.position.y < _minBoardY) _tileObject[i, index].transform.position = _tileObject[i, index].transform.position + new Vector3(0, _tileTotalGapY, 0);
                if (_tileObject[i, index].transform.position.y > _maxBoardY) _tileObject[i, index].transform.position = _tileObject[i, index].transform.position - new Vector3(0, _tileTotalGapY, 0);
            }
        }
    }

}
