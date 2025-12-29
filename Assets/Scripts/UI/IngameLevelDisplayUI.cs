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
    public void DisplayLevelUI(InGameLevel level)
    {

    }
}
