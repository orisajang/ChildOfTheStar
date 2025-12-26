using UnityEngine;
using System.Collections.Generic;

public class DeckManager:Singleton<DeckManager>
{
    [SerializeField]private List<TileSO> _baseDeckSO; 

    [SerializeField]private List<TileSO> _useDeck = new List<TileSO>(); 

    public List<TileSO> GetUseDeck()
    {
        return _useDeck;
    }
    public List<TileSO> GetBaseDeck()
    {
        return _baseDeckSO;
    }
    public void ResetDeck()
    {
        _useDeck.Clear();
        _useDeck.AddRange(_baseDeckSO);
    }

    public void AddTile(TileSO tileSO)
    {
        _useDeck.Add(tileSO);
    }
    public void RemoveTile(TileSO tileSO)
    {
        if (_useDeck.Contains(tileSO))
        {
            _useDeck.Remove(tileSO);
        }
    }
}
