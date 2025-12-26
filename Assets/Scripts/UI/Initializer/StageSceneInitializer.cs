using System.Collections;
using UnityEngine;

public class StageSceneInitializer : MonoBehaviour
{
    //스테이지씬이 시작될때 동작을 위해서 스크립트 정의
    private IEnumerator Start()
    {
        yield return null;
        //던전 매니저의 스테이지 설정 시작
        DungeonManager.Instance.ReturnToStageSelect();

    }
}
