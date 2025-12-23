using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneName
{
    //씬의 이름을 저장하는 static 클래스
    public const string Tile = "TestSampleLobbyScene";
    public const string Battle = "MVPScene_BattleTest";
    public const string shop = "Shop";
    //이후 추가되는 씬들을 똑같이 string으로 추가

}

public class GameManager : Singleton<GameManager>
{
    //씬 관리 (로비, 전투, 상점)
    //게임시작, 게임종료 판정시 어떤 씬으로 이동하라 라는 명령을 내려준다


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //테스트용. 바로 배틀씬으로 이동
        GoToBattleScene();
    }

    public void GoToBattleScene()
    {
        //전투 시작 시
        Debug.Log("배틀씬으로 이동");
        SceneManager.LoadScene(SceneName.Battle);
    }
    public void GoToTitleScene()
    {
        //게임시작, 게임종료될 경우 이 메서드 사용
        Debug.Log("타이틀씬으로 이동");
        SceneManager.LoadScene(SceneName.Tile);
    }

}
