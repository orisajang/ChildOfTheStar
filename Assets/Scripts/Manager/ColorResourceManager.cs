using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class ColorResourceManager : Singleton<ColorResourceManager>
{
    //빨강, 파랑, 초록, 흰색, 검정, 5가지 색생에 대한 자원 관리
    //색상을 enum으로 정의하고, 딕셔너리로 키값을 넣어서 관리
    // 초기값 0으로 해둠 나중에 저장 기능 추가 예정
    //전체 타일 자원에 대한 딕셔너리
    Dictionary<TileColor, int> _colorResourceDic = new Dictionary<TileColor, int>
    {
        {TileColor.Red, 0},
        {TileColor.Blue, 0},
        {TileColor.Green, 0},
        {TileColor.White, 0},
        {TileColor.Black, 0}
    };
    //스테이지에서 얻은 자원에 대한 딕셔너리
    Dictionary<TileColor, int> _currentColorResourceDic = new Dictionary<TileColor, int>
    {
        {TileColor.Red, 0},
        {TileColor.Blue, 0},
        {TileColor.Green, 0},
        {TileColor.White, 0},
        {TileColor.Black, 0}
    };
    //읽기전용 딕셔너리
    public IReadOnlyDictionary<TileColor, int> ColorResourceDic => _colorResourceDic;
    public IReadOnlyDictionary<TileColor, int> CurrentColorResourceDic => _currentColorResourceDic;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return; //이거도 추가
    }

    public int GetResource(TileColor color)
    {
        if (_colorResourceDic.TryGetValue(color, out int value))
            return value;

        return 0;
    }
    public void SetResource(List<ColorResourceDataJson> colorData)
    {
        //색상 정보 넣기
        foreach(var tileResourceData in colorData)
        {
            _colorResourceDic[tileResourceData.color] = tileResourceData.amount;
        }
    }

    /// <summary>
    /// 타일 매치 발생하면 발생한 타일 갯수만큼 자원 증가
    /// </summary>
    public void AddColorResource(TileColor color, int amount)
    {
        if (!_colorResourceDic.ContainsKey(color))
        {
            _colorResourceDic[color] = amount;
            _currentColorResourceDic[color] = amount;
        }
        else
        {
            _colorResourceDic[color] += amount;
            _currentColorResourceDic[color] += amount;
        }
        UIManager.Instance.ResourceUI.UpdateResource(color,_colorResourceDic[color]);
    }

    /// <summary>
    /// 스테이지에서 얻은 타일 자원 갯수를 세야하기때문에 스테이지 시작전 초기화 해줘야함
    /// </summary>
    public void InitCurrentColorResource()
    {
        //초기화
        foreach(TileColor color in _currentColorResourceDic.Keys.ToList())
        {
            _currentColorResourceDic[color] = 0;
        }
    }
}
