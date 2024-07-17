using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
public class InventoryItem : ScriptableObject
{
    public string ID = Guid.NewGuid().ToString();
    public string FriendlyName;
    public string Description;
    public int SellPrice;
    public Sprite Icon;
    public Dimensions SlotDimension;
}

[Serializable]
public struct Dimensions
{
    public int Height;
    public int Width;
}