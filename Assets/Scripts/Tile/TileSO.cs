using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSO", menuName = "Scriptable Objects/TileSO")]
public class TileSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string descriptionText;

    [SerializeField] private List<TileSkillBase> _skillSOList;
    [SerializeField] private List<TileSkillBase> _preSkillSOList;
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _icon;
    [SerializeField] private string _description;
    [SerializeField] private TileColor _color;
    [SerializeField] private Color _spriteColor;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _rarity;
    [SerializeField] private int _price;
    [SerializeField] private int _speed;

    public List<TileSkillBase> SkillSOList => _skillSOList;
    public List<TileSkillBase> PreSkillSOList => _preSkillSOList;
    public int Id => _id;
    public string Name => _name;
    public string Icon => _icon;
    public string Description => _description;
    public TileColor Color => _color;
    public Color SpriteColor => _spriteColor;
    public Sprite Sprite => _sprite;
    public int Rarity => _rarity;
    public int Price => _price;
    public int Speed => _speed;
}
