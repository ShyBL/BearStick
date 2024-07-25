using System;
using UnityEngine;

// Sample item data, could be used for all item data or just for inventory/shop
[CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string ID = Guid.NewGuid().ToString();
    public string FriendlyName;
    public string Description;
    public int SellPrice;
    public Sprite Icon;
    public Dimensions SlotDimension;
}

// Custom Dimensions variable, basically an int 2d vector
[Serializable]
public struct Dimensions
{
    public int Height;
    public int Width;
}