using UnityEngine;

public enum InGameLevel
{
    Easy,Normal,Boss,_end
}

public class IngameLevelDisplayUI : MonoBehaviour
{
    [SerializeField] GameObject _easyUIPrefeb;
    [SerializeField] GameObject _NormalUIPrefeb;
    [SerializeField] GameObject _BossUIPrefeb;

    private void Start()
    {
        DisplayLevelUI();
    }
    public void DisplayLevelUI()
    {
        switch(StageManager.Instance._currentLevel)
        {
            case InGameLevel.Easy:
                Instantiate(_easyUIPrefeb);
                break;
            case InGameLevel.Normal:
                Instantiate(_NormalUIPrefeb);
                break;
            case InGameLevel.Boss:
                Instantiate(_BossUIPrefeb);
                break;
        }
        Destroy(gameObject);
    }
}
