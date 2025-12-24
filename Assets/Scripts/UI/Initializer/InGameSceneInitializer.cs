using System.Collections;
using UnityEngine;

public class InGameSceneInitializer : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        //스테이지 시작 명령
        StageManager.Instance.StartStageTask();
    }
}
