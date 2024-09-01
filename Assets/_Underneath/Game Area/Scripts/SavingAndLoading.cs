using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This Script should be added to any level manager.
public class SavingAndLoading : MonoBehaviour
{
    public static SavingAndLoading Instance { get; private set; }
    private GameObject refPlayerInformation;
    private string playerDataDirPath = "";
    private string playerDataFileName = "PlayerData";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        playerDataDirPath = Application.persistentDataPath;

}

    public void SavePlayerInformation()
    {
        Debug.Log("SavePlayerInfo Called");
        //Save data to a file 
        //use Path.Combine to account for different OS's having different path seperators
        string fullPath = Path.Combine(playerDataDirPath, playerDataFileName);
        try
        {
            //Create the directory the file will be written to if it doesnt already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the Player Data into JSON
            string dataToStore = JsonUtility.ToJson(PlayerData.Instance, true);

            //Write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }

            }
        }
        catch (Exception e)
        {
            Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public PlayerData LoadPlayerInformation()
    {
        Debug.Log("LoadPlayerInfo Called");
        string fullPath = Path.Combine(playerDataDirPath, playerDataFileName);
        PlayerData loadedPlayerData = null;

        if (File.Exists(fullPath)) 
        {
            try
            {
                //Load the serialized data from the file
                string DataToLoad = ""; 
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        DataToLoad = reader.ReadToEnd();
                    }
                }

                //Deserialize the file from Json back to the Object
               loadedPlayerData = JsonUtility.FromJson<PlayerData> (DataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        else
        {
            //Error occurs here
            loadedPlayerData = new PlayerData();
        }

        return loadedPlayerData;
    }

    public void CheckIfFileExistsOnStart()
    {
        //If the File Exists already
       if(File.Exists(Path.Combine(playerDataDirPath, playerDataFileName)))
       {
            LoadPlayerInformation();
       }
       else if(!File.Exists(Path.Combine(playerDataDirPath, playerDataFileName)))
       {
            SavePlayerInformation();
       }
    }
}

//This class is needed because the information that loads from the JSON seems to struggle with using PlayerData
[System.Serializable]
public class PlayerInformation
{
    public int m_Money;
    public int m_NewMoney;
    public int m_DayCount;
    public int m_CurrentExpenses;

    public PlayerInformation()
    {
        m_Money = PlayerData.Instance.GetMoney();
        m_NewMoney = PlayerData.Instance.GetMoneyEarned();
        m_DayCount = PlayerData.Instance.GetDayCount();
        m_CurrentExpenses = PlayerData.Instance.GetExpenses();
    }
}