using System.Collections.Generic;
using UnityEngine;


public class SkillExecute
{
    [SerializeField] private TileSkillBase _skill;
    [SerializeField] private SkillConditionBase _skillConditionBase;
}



[CreateAssetMenu(fileName = "TileSO", menuName = "Scriptable Objects/TileSO")]
public class TileSO : ScriptableObject
{
    [SerializeField] private List<SkillExecute> _skillExecuteList;





    [TextArea(3, 10)]
    public string descriptionText;

    [SerializeField] private List<TileSkillBase> _preSkillSOList;
    [SerializeField] private List<TileSkillBase> _skillSOList;



    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _icon;
    [SerializeField] private string _description;
    [SerializeField] private TileColor _color;
    [SerializeField] private Color _spriteColor;
    [SerializeField] private Sprite _baseSprite;
    [SerializeField] private Sprite _rareSprite;
    [SerializeField] private Sprite _iconSprite;
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
    public Sprite BaseSprite => _baseSprite;
    public Sprite RareSprite => _rareSprite;
    public Sprite IconSprite => _iconSprite;
    public int Rarity => _rarity;
    public int Price => _price;
    public int Speed => _speed;
}
