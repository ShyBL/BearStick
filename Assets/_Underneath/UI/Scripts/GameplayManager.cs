using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : OurMonoBehaviour
{
   // public static GameplayManager Instance; // Since this class is static you can use this instance to access it following the singleton pattern.

    public int Money;
    public int TempMoneyValue;
    public int DayCount;
    public int Expenses;
    public Vector2 v_SpawnLocation;

    public delegate void RefreshMoney();
    public RefreshMoney m_RefreshMoney;
    
    //public SavedPlayerData PlayerData;
    public SavePlayerData PlayerData;
    private void Awake()
    {
        // if (Instance == null)
        // {
        //     Instance = this;
        // }
        // else if (Instance != this)
        // {
        //     Destroy(this);
        // }

        SceneManager.sceneLoaded += InitializeRespawnPoint;
        
        InitializeFromSave();
    }

    private void InitializeFromSave()
    {
        PlayerData = Resources.Load<SavePlayerData>("SavedPlayerData");
        
        Money = PlayerData.Money ;
        Expenses = PlayerData.Expenses;
        DayCount = PlayerData.DayCount;
        TempMoneyValue = PlayerData.TempMoneyValue;
        v_SpawnLocation = PlayerData.v_SpawnLocation;
        
        // PlayerData = GameManager.SaveManager.LoadDataAndCreateIfNull<SavedPlayerData>();
        //
        // foreach (var kvp in PlayerData.SavedPlayerDateDic)
        // {
        //     switch (kvp.Key)
        //     {
        //         case "Money":
        //             Money = kvp.Value;
        //             break;
        //         case "DayCount":
        //             DayCount = kvp.Value;
        //             break;
        //         case "Expenses": 
        //             Expenses = kvp.Value;
        //             break;
        //     }
        // }
    }
    
    private void InitializeRespawnPoint(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "newLevel") return;
        
        GameObject spawnLocation = GameObject.Find("PlayerRespawnPoint");

        if (spawnLocation != null)
        {
            // v_SpawnLocation = spawnLocation.transform.position;
            PlayerData.v_SpawnLocation = spawnLocation.transform.position;

        }
        else
        {
            GameObject newSpawnLocation = new GameObject("PlayerRespawnPoint");
            newSpawnLocation.transform.position = this.transform.position;
            //  v_SpawnLocation = newSpawnLocation.transform.position;

            PlayerData.v_SpawnLocation = newSpawnLocation.transform.position;
        }
    }

    private void IncreaseMoney(int amount)
    {
       // TempMoneyValue += amount; 
       PlayerData.TempMoneyValue += amount;
    }

    public void DecreaseMoney(int amount)
    {
      //  TempMoneyValue -= amount;
      PlayerData.TempMoneyValue -= amount;
    }

    public void IncrementDayCount()
    {
        PlayerData.DayCount++;
      //  DayCount++;
        // if (PlayerData.SavedPlayerDateDic.ContainsKey("DayCount"))
        // {
        //     PlayerData.SavedPlayerDateDic["DayCount"] = DayCount;
        // }
        //PlayerData.SaveData();
    }
    public void IncreaseExpenses(int amount)
    {
        PlayerData.Expenses += amount;
        // Expenses += amount;
        // if (PlayerData.SavedPlayerDateDic.ContainsKey("Expenses"))
        // {
        //     PlayerData.SavedPlayerDateDic["Expenses"] = Expenses;
        // }
        //PlayerData.SaveData();
    }

    public void ApplyMoneyChange()
    {
        PlayerData.Money += PlayerData.TempMoneyValue;
        PlayerData.TempMoneyValue = 0;
        m_RefreshMoney.Invoke();
        // Money += TempMoneyValue;
        // TempMoneyValue = 0;
        // m_RefreshMoney.Invoke();
        // if (PlayerData.SavedPlayerDateDic.ContainsKey("Money"))
        // {
        //     PlayerData.SavedPlayerDateDic["Money"] = Money;
        // }
        //PlayerData.SaveData();
    }

    public int GetMoney()
    {
        return PlayerData.Money;
        // return Money;
    }

    public int GetDayCount()
    {
        return PlayerData.DayCount;
       // return DayCount;
    }

    public int GetExpenses()
    {
        return PlayerData.Expenses;
       // return Expenses;
    }

    public int GetMoneyEarned()
    {
        return PlayerData.Expenses;
       // return TempMoneyValue;
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