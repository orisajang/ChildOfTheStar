
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum TileColor
{
    Black,
    White,
    Red,
    Green,
    Blue,
}

public class Tile : MonoBehaviour
{
    static private TileStatus[] _statusSequence = { TileStatus.Frenzy, TileStatus.Recovery, TileStatus.Growth, TileStatus.Rebirth, TileStatus.Destruction };

    private int _x, _y;
    [SerializeField] private TileSO _tileDataSO;
    private TileColor _curColor;
    private Dictionary<TileStatus, List<TileStatusBase>> _statusDictionary;

    public int X => _x;
    public int Y => _y;
    public TileColor Color => _curColor;
    public TileSO TileData => _tileDataSO;
    public Dictionary<TileStatus, List<TileStatusBase>> StatusDictionarty => _statusDictionary;

    public void ExecuteTile(Tile[,] board)//추후에 플레이어, 몬스터배열 인자 추가 필요함
    {
        //스테이터스 딕셔너리 순서대로 쭉죽 스테이터스가 가지고있는 함수 실행
        foreach (var seq in _statusSequence)
        {
            if (_statusDictionary.TryGetValue(seq, out var statusList))
            {
                foreach (var status in statusList)
                {
                    status.Execute(board,this);
                }
            }
        }
        // 스킬리스트 순서대로 쭉쭉 실행
        foreach (var skill in _tileDataSO.SkillSOList)
        {
            skill.Execute();
        }
    }
}
