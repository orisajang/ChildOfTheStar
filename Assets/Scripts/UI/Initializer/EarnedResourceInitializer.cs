using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorTextBinding
{
    public TileColor color;
    public TextMeshProUGUI text;
}

public class EarnedResourceInitializer : MonoBehaviour
{
    //색상UI
    [SerializeField] private List<ColorTextBinding> _earnedResourceList;
    private Dictionary<TileColor, TextMeshProUGUI> _colorUITextDic = new Dictionary<TileColor, TextMeshProUGUI>();
    //나가기버튼
    [SerializeField] private Button _exitButton;

    private IEnumerator Start()
    {
        yield return null;

        SetTileColorUIText();
    }
    private void OnEnable()
    {
        _exitButton.onClick.AddListener(() => SetExitButton());
    }
    private void OnDisable()
    {
        _exitButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 현재 스테이지 인스턴스에서 얻은 색상타일 정보를 설정하는 메서드
    /// </summary>
    public void SetTileColorUIText()
    {
        //중복없다고 가정하고 그냥 넣어버림
        for (int index = 0; index < _earnedResourceList.Count; index++)
        {
            _colorUITextDic[_earnedResourceList[index].color] = _earnedResourceList[index].text;
        }

        //이제 ColorResourceManager과 연동 
        foreach (var color in _colorUITextDic.Keys)
        {
            //UI에 표시되어야하는 타일색상만
            if (_colorUITextDic.ContainsKey(color))
            {
                //색상별 얻은타일 갯수
                int tileAmount = ColorResourceManager.Instance.CurrentColorResourceDic[color];
                _colorUITextDic[color].text = $"{tileAmount}";
            }
        }
    }

    public void SetExitButton()
    {
        GameManager.Instance.GoToStageScene();
    }
}