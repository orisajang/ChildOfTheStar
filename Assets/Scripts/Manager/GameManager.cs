using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneName
{
    //씬의 이름을 저장하는 static 클래스
    public const string Tile = "TestSampleLobbyScene";
    public const string Battle = "InGameMainTest";
    public const string Shop = "ShopScene";
    public const string Stage = "StageSelectScene";
    public const string Lobby = "LobbyTest";
    public const string EarnedResource = "EarnedResourceScene";
    //이후 추가되는 씬들을 똑같이 string으로 추가

}

public class GameManager : Singleton<GameManager>
{
    //씬 관리 (로비, 전투, 상점)
    //게임시작, 게임종료 판정시 어떤 씬으로 이동하라 라는 명령을 내려준다

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return; //이거도 추가
    }

    private void Start()
    {
        //테스트용. 바로 배틀씬으로 이동
        //GoToBattleScene();
    }

    public void GoToBattleScene()
    {
        //전투 시작 시
        //Debug.Log("배틀씬으로 이동");
        SceneManager.LoadScene(SceneName.Battle);
    }
    public void GoToTitleScene()
    {
        SceneManager.LoadScene(SceneName.Tile);
    }
    public void GoToStageScene()
    {
        SceneManager.LoadScene(SceneName.Stage);
    }
    public void GoToLobbyScene()
    {
        ShopManager.Instance.Init();
        SceneManager.LoadScene(SceneName.Lobby);
    }
    public void GoToResourceEarnedScene()
    {
        //        Debug.Log("인게임클리어후 자원획득량창으로 이동");
        ShopManager.Instance.SuffleShopSlots();
        SceneManager.LoadScene(SceneName.EarnedResource);
    }

    public void GoToShopScene()
    {
 //       Debug.Log("상점씬으로 이동");
        SceneManager.LoadScene(SceneName.Shop);
    }


}
