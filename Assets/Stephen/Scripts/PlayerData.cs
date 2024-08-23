using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance; // Since this class is static you can use this instance to access it following the singleton pattern.

    private int m_Money = 0;
    private int m_NewMoney = 0;
    private int m_DayCount = 1;
    private int m_CurrentExpenses = 0;

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
    }

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
        for (var index = 0; index < Inventory.Instance.StoredItems.Count; index++)
        {
            var storedItem = Inventory.Instance.StoredItems[index];
            IncreaseMoney(storedItem.Details.SellPrice);
        }
        Inventory.Instance.ClearInventory();
    }
}
