using System;
using UnityEngine;

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
/// <summary>
/// View임시용(그냥 있다고 가정하기 위해서
/// </summary>
public class TileBoardViewTest : MonoBehaviour
{
    public event Action<int, int> OnTileMoved;

    //대충 타일 이동에서 이벤트가 인보크 되었다고 가정
    private void Start()
    {
        //타일이 좌우방향으로 이동, 오른쪽 방향으로 3만큼 이동했다
        OnTileMoved?.Invoke(0, 2);
    }


    /// <summary>
    /// 예시용으로 작성 (어떤 매개변수 들어올지는 아직 미정)
    /// Controll에서 타일정보를 입력하면 이 정보를 토대로 View가 정보를 입력한다
    /// </summary>
    /// <param name="tileArray"></param>
    public void SetTileInfoUI(int[,] tileArray)
    {
        //타일 정보(tileArray)를 읽어서 UI에 표시
    }
}

public class TileBoardController : MonoBehaviour
{
    //1.View에서 가로로 드래그했다 입력 받기
    //2.Model에게 이동 요청
    //3.Model의 이동이 정상적으로 되었다면?
    //- 성공하면 View에게 어떻게 변하라고 전달 (이동만 해라, 매치 발생하면? Model에서 변한 배열을 View에 넘겨줘서 View에서 처리하게?? 
    //- 실패하면 View에게 변하지말라고 전달
    TileBoardModelTest modelTest = new TileBoardModelTest();
    TileBoardViewTest viewTest;

    private void Awake()
    {
        //view를 어딘가에서 가져왔다고 가정
        viewTest = GetComponent<TileBoardViewTest>();
    }
    private void OnEnable()
    {
        viewTest.OnTileMoved += OnTileMoveTriggered;
        modelTest.OnMoveSuccessed += OnTileMoveSuccess;
    }
    private void OnDisable()
    {
        viewTest.OnTileMoved -= OnTileMoveTriggered;
        modelTest.OnMoveSuccessed -= OnTileMoveSuccess;
    }
    private void OnTileMoveTriggered(int pos, int moveValue)
    {
        modelTest.MoveTile(pos, moveValue);
    }
    /// <summary>
    /// 타일 이동이 정상적으로 완료되었는지
    /// </summary>
    /// <param name="isSuccess"></param>
    private void OnTileMoveSuccess(bool isSuccess, int[,] tileInfo)
    {
        //성공과 실패 처리
        if (isSuccess)
        {
            //성공했다면? View를 바꾸는 행동 진행
            //View에 바뀐 타일 정보를 보내주면, 그 정보를 토대로 진행
            //int[,] tiles = new int[6, 5];
            viewTest.SetTileInfoUI(tileInfo);
        }
        else
        {
            //실패했다면? View에서 아무것도 안함
        }
    }
}
