using System.Collections.Generic;
using UnityEngine;

public class ColorResourceManager : Singleton<ColorResourceManager>
{
    //빨강, 파랑, 초록, 흰색, 검정, 5가지 색생에 대한 자원 관리
    //색상을 enum으로 정의하고, 딕셔너리로 키값을 넣어서 관리
    // 초기값 0으로 해둠 나중에 저장 기능 추가 예정
    Dictionary<TileColor, int> _colorResourceDic = new Dictionary<TileColor, int>
    {
        {TileColor.Red, 0},
        {TileColor.Blue, 0},
        {TileColor.Green, 0},
        {TileColor.White, 0},
        {TileColor.Black, 0}
    };
    protected void Awake()
    {
        base.Awake();
    }

    public int GetResource(TileColor color)
    {
        if (_colorResourceDic.TryGetValue(color, out int value))
            return value;

        return 0;
    }


    /// <summary>
    /// 타일 매치 발생하면 발생한 타일 갯수만큼 자원 증가
    /// </summary>
    public void AddColorResource(TileColor color, int amount)
    {
        if (!_colorResourceDic.ContainsKey(color))
        {
            _colorResourceDic[color] = amount;
        }
        else
        {
            _colorResourceDic[color] += amount;
        }
        UIManager.Instance.ResourceUI.UpdateResource(color,_colorResourceDic[color]);
    }
}
