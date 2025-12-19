using System.Collections.Generic;
using UnityEngine;

public class ColorResourceManager : Singleton<ColorResourceManager>
{
    //빨강, 파랑, 초록, 흰색, 검정, 5가지 색생에 대한 자원 관리
    //색상을 enum으로 정의하고, 딕셔너리로 키값을 넣어서 관리
    Dictionary<TileColor, int> _colorResourceDic = new Dictionary<TileColor, int>();

    protected void Awake()
    {
        base.Awake();
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

            

    }


}
