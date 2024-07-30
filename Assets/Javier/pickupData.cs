
//pickup data class

using UnityEngine;

public class PickupData
{
    //Declaring variables to be used - taken from the GDD
    [SerializeField] private int weight;
    [SerializeField] private int value;
    [SerializeField] private string definition;
    [SerializeField] private string name;
    [SerializeField] private string[] rarity = { "Common", "Uncommon", "Rare", "Exotic" };

    //weight - getter and setter
    public int GetWeight()
    {
        return weight;
    }

    public void SetWeight(int newWeight)
    {
        weight = newWeight;
    }

    //value - getter and setter
    public int GetValue()
    {
        return value;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    //definition - getter and setter
    public string GetDefinition()
    {
        return definition;
    }

    public void SetDefinition(string newDefinition)
    {
        definition = newDefinition;
    }

    //name - getter and setter
    public string GetName()
    {
        return name;
    }

    public void SetName(string newName)
    {
        name = newName;
    }

    //rarity - getter and setter
    public string GetRarity(int i)
    {
        return rarity[i];
    }

    public void SetRarity(int i, string newRarity)
    {
        rarity[i] = newRarity;
    }
}