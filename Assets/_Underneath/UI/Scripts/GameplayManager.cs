using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : OurMonoBehaviour
{
    public static GameplayManager Instance; // Since this class is static you can use this instance to access it following the singleton pattern.

    public int Money;
    public int TempMoneyValue;
    public int DayCount;
    public int Expenses;
    public Vector2 v_SpawnLocation;

    public delegate void RefreshMoney();
    public RefreshMoney m_RefreshMoney;
    
    public SavedPlayerData PlayerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        
        InitializeFromSave();
    }

    private void InitializeFromSave()
    {
        PlayerData = GameManager.SaveManager.LoadDataAndCreateIfNull<SavedPlayerData>();
        
        foreach (var kvp in PlayerData.SavedPlayerDateDic)
        {
            switch (kvp.Key)
            {
                case "Money":
                    Money = kvp.Value;
                    break;
                case "DayCount":
                    DayCount = kvp.Value;
                    break;
                case "Expenses": 
                    Expenses = kvp.Value;
                    break;
            }
        }
    }

    private void Start()
    {
        GameObject spawnLocation = GameObject.Find("PlayerRespawnPoint");
    
        if (spawnLocation != null)
        {
            v_SpawnLocation = spawnLocation.transform.position;
        }
        else
        {
            GameObject newSpawnLocation = new GameObject("PlayerRespawnPoint");
            newSpawnLocation.transform.position = this.transform.position;
            v_SpawnLocation = newSpawnLocation.transform.position;
        }
    }

    private void IncreaseMoney(int amount)
    {
        TempMoneyValue += amount; 
    }

    public void DecreaseMoney(int amount)
    {
        TempMoneyValue -= amount;
    }

    public void IncrementDayCount()
    {
        DayCount++;
        if (PlayerData.SavedPlayerDateDic.ContainsKey("DayCount"))
        {
            PlayerData.SavedPlayerDateDic["DayCount"] = DayCount;
        }
        PlayerData.SaveData();
    }
    public void IncreaseExpenses(int amount)
    {
        Expenses += amount;
        if (PlayerData.SavedPlayerDateDic.ContainsKey("Expenses"))
        {
            PlayerData.SavedPlayerDateDic["Expenses"] = Expenses;
        }
        PlayerData.SaveData();
    }

    public void ApplyMoneyChange()
    {
        Money += TempMoneyValue;
        TempMoneyValue = 0;
        m_RefreshMoney.Invoke();
        if (PlayerData.SavedPlayerDateDic.ContainsKey("Money"))
        {
            PlayerData.SavedPlayerDateDic["Money"] = Money;
        }
        PlayerData.SaveData();
    }

    public int GetMoney()
    {
        return Money;
    }

    public int GetDayCount()
    {
        return DayCount;
    }

    public int GetExpenses()
    {
        return Expenses;
    }

    public int GetMoneyEarned()
    {
        return TempMoneyValue;
    }

    public void ShopPayoff()
    {
        IncreaseMoney(Inventory.Instance.GetInventoryvalue());
        Inventory.Instance.ClearInventory();
        ApplyMoneyChange();
        //SavingAndLoading.Instance.SavePlayerInformation();
    }

}

[Serializable]
public class SavedPlayerData : ISaveData
{
    public Dictionary<string, int> SavedPlayerDateDic = new()
    {
        {"Money",0}, {"DayCount",1}, {"Expenses",20}
    };
}