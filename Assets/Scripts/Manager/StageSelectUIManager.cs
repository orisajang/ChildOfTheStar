
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StageSelectUIManager : Singleton<StageSelectUIManager>
{
    //StageSelectUIManager -> 스테이지 선택해서 전투씬으로 이동하고 다시 스테이지 선택UI로 돌아왔을때 가지고있어야할 정보 모아둠
    //스테이지 노드와 인스턴스들을 가지고있는 부모 오브젝트 위치
    [SerializeField] private Transform _gridStageParentNode;
    //버튼 클릭할때마다 얼마만큼 위로 이동할건지 
    [SerializeField] private int _moveValue = 100;
    //스테이지 UI (세로로 긴 객체) -> 전용 스크립트를 만들어서 동적할당 할지 고민중
    [SerializeField] private Transform _stageButtonNodeParent; 
    //플레이어 이미지 위치
    [SerializeField] private Image _playerImage;
    //버튼 클릭한 위치 저장
    private Vector2 _clickPositionTransform;

    List<List<Button>> btnList = new List<List<Button>>();

    protected override void Awake()
    {
        //씬마다 초기화되어야해서 isDestroyOnLoad = false처리
        isDestroyOnLoad = false;
        base.Awake();
        if (Instance != this) return; //이거도 추가
    }
    public void SetBtnList(Dictionary<int, int> selStageInfo)
    {
        //1. 버튼 정보 설정
        //자식 컴포넌트를 순회
        //Hierachy에서 StageParent - Node_1 - 버튼들 구조로 되어있을때 버튼들을 리스트에 저장하기 위해 사용
        //StageParent들 자식을 순회한다
        int rowIndex = 0;
        int colIndex = 0;
        foreach (Transform stageNodes in _gridStageParentNode)
        {
            colIndex = 0;
            List<Button> listBuf = new List<Button>();
            //Node들 자식 순회
            foreach (Transform stageBtns in stageNodes)
            {
                Button btn = stageBtns.GetComponent<Button>();
                Button btn_capture = btn;
                btn_capture.interactable = false;
                btn_capture.onClick.AddListener(() => StageInstanceBtnClick(btn_capture));

                //추가로 이미 이전에 클리어한 스테이지버튼들의 색상을 빨간색으로 지정해주자
                if(selStageInfo.ContainsKey(rowIndex) &&
                    selStageInfo[rowIndex] == colIndex)
                {
                    btn_capture.image.color = Color.gray;
                    //마지막 플레이어 위치를 다시 표시하기위해
                    int lastPos = DungeonManager.Instance.LastSelectStageIndexKey;
                    if (lastPos == rowIndex)
                    {
                        RectTransform btnTransform = btn_capture.GetComponent<RectTransform>();
                        Vector2 pos = btnTransform.anchoredPosition;

                        _playerImage.rectTransform.anchoredPosition = pos; //위치가 이상함
                    }
                }

                listBuf.Add(btn);
                colIndex++;
            }
            btnList.Add(listBuf);
            rowIndex++;
        }
    }

    public void SetStageInfo(int stageNo, int instanceNo, Dictionary<int, int> selStageInfo)
    {
        //if (btnList.Count == 0)
        //{
        //    //초기1회 버튼 리스트 정보설정
        //    SetBtnList();
        //}
        //씬마다 버튼이벤트 등록해야하므로 무조건 진행
        SetBtnList(selStageInfo);
        btnList[stageNo][instanceNo].interactable = true;
        SetStageUIInit(btnList[stageNo][instanceNo]);
    }

    public void SetStageUIInit(Button btn)
    {
        //다시 스테이지UI로 넘어오면 아래 명령어 실행
        Vector3 vec = _stageButtonNodeParent.position;
        vec.y -= (btn.transform.position.y - _moveValue);
        //stageUIParent.position.y = btn.transform.position.y - moveValue;
        _stageButtonNodeParent.position = vec;
    }

    public void StageInstanceBtnClick(Button btn)
    {
        //던전매니저에서 시작하는 명령을 내리도록 함 (어떤 버튼인지는 상관없이)
        //클릭한 버튼 위치 저장
        _clickPositionTransform = btn.GetComponent<RectTransform>().anchoredPosition;
        //한번이라도 스테이지 인스턴스 버튼을 클릭했었으면 해당 버튼 위치로 이동
        _playerImage.rectTransform.anchoredPosition = _clickPositionTransform;
        //클릭한 스테이지 진입, 전투시작
        DungeonManager.Instance.SetAndStartNextStage();
    }

}
