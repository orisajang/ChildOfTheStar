using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonSelectUI : MonoBehaviour
{
    [SerializeField] private Button[] dungeonButtons;
    [SerializeField] private Image[] dungeonImages;

    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Sprite unlockSprite;

    // 테스트용 코드
    int currentDungeon = 2;

    /// <summary>
    /// 세이브 파일로 현재 currentDengeon번호를 입력할때 사용하는 메서드 (기존 클리어한 던전은 해금시키기 위해)
    /// </summary>
    /// <param name="currentDengeonNum"></param>
    public void UpdatecurrentDengeon(int currentDengeonNum)
    {
        currentDungeon = currentDengeonNum;
    }

    private void Start()
    {
        DungeonSelect();
    }
    /// <summary>
    /// 몇번째 던전을 선택할 것인지 버튼 관련 설정
    /// </summary>
    public void DungeonSelect()
    {
        for(int i = 0; i < dungeonButtons.Length; i++)
        {
            bool isUnlocked = i < currentDungeon;

            dungeonButtons[i].interactable = isUnlocked;

            if (isUnlocked)
            {
                dungeonImages[i].sprite = unlockSprite;
            }
            else
            {
                dungeonImages[i].sprite = lockSprite;
            }
            //버튼 클릭시 어떤 동작을 하는지 결정
            //버튼을 클릭하면 던전을 선택한것. 던전 번호를 던전 매니저에 넘겨주도록 한다
            //델리게이트 캡쳐 막기위해서 index라는 임시변수 선언
            int index = i;
            dungeonButtons[i].onClick.AddListener(() => OnDungeonSelect(index));
        }
    }
    public void OnDungeonSelect(int dengeonNumber)
    {
        Debug.Log($"선택한 던전 번호: {dengeonNumber}");
        //던전 정보 설정
        DungeonManager.Instance.SetDengeonNumber(dengeonNumber + 1);
        //씬 이동
        GameManager.Instance.GoToStageScene();
        
    }
}
