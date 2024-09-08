using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static PlayerData;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance; // Since this class is static you can use this instance to access it following the singleton pattern.

    public int m_Money = 0;
    public int m_NewMoney = 0;
    public int m_DayCount = 1;
    public int m_CurrentExpenses = 0;
    public Vector2 v_SpawnLocation;

    public delegate void RefreshMoney();
    public RefreshMoney m_RefreshMoney;

    public PlayerData()
    {
        m_Money = 0;
        m_NewMoney = 0;
        m_DayCount = 1;
        m_CurrentExpenses = 0;
        v_SpawnLocation = Vector2.zero;
    }

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

    }

    private void Start()
    {
        GameObject spawnLocation = GameObject.Find("PlayerRespawnPoint");
        v_SpawnLocation = spawnLocation.gameObject.transform.position;
    }

    public void IncreaseMoney(int amount)
    {
        m_NewMoney += amount; 
    }

    public void DecreaseMoney(int amount)
    {
        m_NewMoney -= amount;
    }

    public void IncrementDayCount()
    {
        m_DayCount++;
    }
    public void IncreaseExpenses(int amount)
    {
        m_CurrentExpenses += amount;
    }

    public void ApplyMoneyChange()
    {
        m_Money += m_NewMoney;
        m_NewMoney = 0;
        m_RefreshMoney.Invoke();
    }

    public void SetMoney(int amount){m_Money = amount;}
    public void SetNewMoney(int amount) {m_NewMoney = amount;}
    public void SetDayCount(int count) {m_DayCount = count;}
    public void SetExpenses(int amount) {m_CurrentExpenses = amount;}

    public int GetMoney()
    {
        return m_Money;
    }

    public int GetDayCount()
    {
        return m_DayCount;
    }

    public int GetExpenses()
    {
        return m_CurrentExpenses;
    }

    public int GetMoneyEarned()
    {
        return m_NewMoney;
    }

    public void DoCache()
    {
        IncreaseMoney(Inventory.Instance.GetInventoryvalue());
        Inventory.Instance.ClearInventory();
        ApplyMoneyChange();
    }

}
