using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavedPlayerData", menuName = "SavedPlayerData")]
public class SavePlayerData : ScriptableObject
{
    public int Money;
    public int TempMoneyValue;
    public int DayCount;
    public int Expenses;
    public Vector2 v_SpawnLocation;
    public List<StoredItem> StoredItems = new List<StoredItem>();

}