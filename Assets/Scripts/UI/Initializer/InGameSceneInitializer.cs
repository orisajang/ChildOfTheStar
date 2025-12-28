using System.Collections;
using UnityEngine;

public class InGameSceneInitializer : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        //스테이지 시작전 스테이지마다 얼마나 자원 얻었는지 확인하는 딕셔너리 초기화
        ColorResourceManager.Instance.InitCurrentColorResource();
        //스테이지 시작 명령
        StageManager.Instance.StartStageTask();
    }
}
