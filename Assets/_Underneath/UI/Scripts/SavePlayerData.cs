using UnityEngine;

[CreateAssetMenu(fileName = "SavedPlayerData", menuName = "SavedPlayerData")]
public class SavePlayerData : ScriptableObject
{
    public int Money;
    public int TempMoneyValue;
    public int DayCount;
    public int Expenses;
    public Vector2 v_SpawnLocation;
}