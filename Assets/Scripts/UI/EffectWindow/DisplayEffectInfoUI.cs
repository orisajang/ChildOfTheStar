using UnityEngine;

public class DisplayEffectInfoUI : MonoBehaviour
{
    [SerializeField] GameObject _effectUIPrefeb;
    [SerializeField] int _displayMaxEffect = 20;

    private GameObject[] _effectUIs;

    private void Awake()
    {
        _effectUIs = new GameObject[_displayMaxEffect];
    }
}
