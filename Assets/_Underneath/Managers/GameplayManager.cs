using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : OurMonoBehaviour
{
   
    public Vector2 v_SpawnLocation;
    public List<StoredItem> StoredItems = new List<StoredItem>();

    public delegate void RefreshMoney();
    public RefreshMoney m_RefreshMoney;
    
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
        SceneManager.sceneLoaded += InitializeInventory;

        
        InitializeFromSave();
    }

    private void InitializeInventory(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "newLevel") return;
        var inv = Inventory.Instance.StoredItems;
        
        foreach (var storedItem in StoredItems)
        {
            inv.Add(storedItem);
        }

        Inventory.Instance.InventoryChanged += SaveInventory;
    }

    private void SaveInventory(List<StoredItem> obj)
    {
        StoredItems.Clear();
        StoredItems.AddRange(obj);
        PlayerData.StoredItems.Clear();
        PlayerData.StoredItems.AddRange(StoredItems);
    }


    private void InitializeFromSave()
    {
        PlayerData = Resources.Load<SavePlayerData>("SavedPlayerData");
        v_SpawnLocation = PlayerData.v_SpawnLocation;

        foreach (var storedItem in PlayerData.StoredItems)
        {
            StoredItems.Add(storedItem);
        }
    }
    
    
    private void InitializeRespawnPoint(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "newLevel") return;
        
        GameObject spawnLocation = GameObject.Find("PlayerRespawnPoint");

        if (spawnLocation != null)
        {
            PlayerData.v_SpawnLocation = spawnLocation.transform.position;

        }
        else
        {
            GameObject newSpawnLocation = new GameObject("PlayerRespawnPoint");
            newSpawnLocation.transform.position = this.transform.position;

            PlayerData.v_SpawnLocation = newSpawnLocation.transform.position;
        }
    }

    private void IncreaseMoney(int amount)
    {
       PlayerData.TempMoneyValue += amount;
    }

    public void DecreaseMoney(int amount)
    {
      PlayerData.TempMoneyValue -= amount;
    }

    public void IncrementDayCount()
    {
        PlayerData.DayCount++;
    }
    public void IncreaseExpenses(int amount)
    {
        PlayerData.Expenses += amount;
    }

    public void ApplyMoneyChange()
    {
        PlayerData.Money += PlayerData.TempMoneyValue;
        PlayerData.TempMoneyValue = 0;
        m_RefreshMoney.Invoke();
    }

    public int GetMoney()
    {
        return PlayerData.Money;
    }

    public int GetDayCount()
    {
        return PlayerData.DayCount;
    }

    public int GetExpenses()
    {
        return PlayerData.Expenses;
    }

    public int GetMoneyEarned()
    {
        return PlayerData.Expenses;
    }

    public void ShopPayoff()
    {
        IncreaseMoney(Inventory.Instance.GetInventoryvalue());
        Inventory.Instance.ClearInventory();
        ApplyMoneyChange();
    }

}