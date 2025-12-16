
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
    private int _x, _y;
    [SerializeField] private TileSO _tileDataSO;
    private TileColor _curColor;
    Dictionary<TileStatus, List<TileStatusBase>> _statusDictionary;
    TileStatus[] _statusSequence = { TileStatus.Frenzy, TileStatus.Recovery, TileStatus.Growth, TileStatus.Rebirth, TileStatus.Destruction };

    public int X => _x;
    public int Y => _y;
    public TileColor Color => _curColor;
    public TileSO TileData => _tileDataSO;

    public void ExecuteTile(Tile[,] Board)
    {
        foreach (var skill in _tileDataSO.SkillSOList)
        {
            skill.Execute();
        }
        foreach (var seq in _statusSequence)
        {
            if (_statusDictionary.TryGetValue(seq, out var statusList))
            {
                foreach (var status in statusList)
                {   
                    status.Execute();
                }
            }
        }
    }
}
