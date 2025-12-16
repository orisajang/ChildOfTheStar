using System;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    string _id;
    Action[] _attribute;
    int[] stack;
    Action _effect;
    int _speed;
    TileStatus _status;
}
