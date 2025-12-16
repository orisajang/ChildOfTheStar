using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSO", menuName = "Scriptable Objects/TileSO")]
public class TileSO : ScriptableObject
{

    [SerializeField] private List<TileSkillBase> _skillSOList;
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _icon;
    [SerializeField] private string _description;
    [SerializeField] private TileColor _color;
    [SerializeField] private int _rarity;
    [SerializeField] private int _price;
    [SerializeField] private int _speed;

    public List<TileSkillBase> SkillSOList => _skillSOList;
    public int Id => _id;
    public string Name => _name;
    public string Icon => _icon;
    public string Description => _description;
    public TileColor Color => _color;
    public int Rarity => _rarity;
    public int Price => _price;
    public int Speed => _speed;
}
