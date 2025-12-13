using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Model임시용(그냥 있다고 가정하기 위해서)
/// </summary>
public class TileBoardModelTest
{
    public event Action<bool, int[,]> OnMoveSuccessed;
    int[,] tileArray = new int[6, 5];
    //실제 타일 이동 로직을 처리
    public void MoveTile(int a, int b)
    {
        //타일 이동하고, 3매치되면 터지고, 다시채우는 로직작성
        //성공하면 이벤트 발동
        OnMoveSuccessed?.Invoke(true, tileArray);
        //실패하면 이벤트 발동
        //OnMoveSuccessed?.Invoke(false, tileArray);
    }
}



