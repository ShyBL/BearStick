using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // // An enumerator for rarities. This may not be the actual list of rarities in the game, but it works as a placeholder.
    // public enum Rarities
    // {
    //     common,
    //     uncommon,
    //     rare,
    //     veryRare
    // }
    //
    // // An int for value, a float for weight, and an enum value for rarity.
    // [SerializeField] private int value;
    // [SerializeField] private float weight;
    // [SerializeField] private Rarities rarity;
    //
    // // Basic Get methods.
    // public int GetValue(){ return value; }
    // public float GetWeight(){ return weight; }
    // public Rarities GetRarity() { return rarity; }
    public Item CollectableData;

    private void Awake()
    {
        throw new NotImplementedException();
    }
}
