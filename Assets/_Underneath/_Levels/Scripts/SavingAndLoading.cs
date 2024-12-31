// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;
//
// //This Script should be added to any level manager.
// public class SavingAndLoading : MonoBehaviour
// {
//     public static SavingAndLoading Instance { get; private set; }
//     private GameObject refPlayerInformation;
//     private string playerDataDirPath = "";
//     private string playerDataFileName = "PlayerData";
//
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else if(Instance != this)
//         {
//             Destroy(this);
//         }
//         playerDataDirPath = Application.persistentDataPath;
//
//     }
//
//     public void SavePlayerInformation()
//     {
//         Debug.Log("SavePlayerInfo Called");
//         //Get all the information
//         PlayerInformation playerInfo = new PlayerInformation();
//         //Save data to a file 
//         //use Path.Combine to account for different OS's having different path seperators
//         string fullPath = Path.Combine(playerDataDirPath, playerDataFileName);
//         try
//         {
//             //Create the directory the file will be written to if it doesnt already exist
//             Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
//
//             //Serialize the Player Data into JSON
//             string dataToStore = JsonUtility.ToJson(playerInfo, true);
//
//             //Write the serialized data to the file
//             using (FileStream stream = new FileStream(fullPath, FileMode.Create))
//             {
//                 using (StreamWriter writer = new StreamWriter(stream))
//                 {
//                     writer.Write(dataToStore);
//                 }
//
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
//         }
//     }
//
//     public PlayerInformation LoadPlayerInformation()
//     {
//         Debug.Log("LoadPlayerInfo Called");
//         string fullPath = Path.Combine(playerDataDirPath, playerDataFileName);
//         PlayerInformation loadedPlayerData = null;
//
//         if (File.Exists(fullPath)) 
//         {
//             try
//             {
//                 //Load the serialized data from the file
//                 string DataToLoad = ""; 
//                 using (FileStream stream = new FileStream(fullPath, FileMode.Open))
//                 {
//                     using (StreamReader reader = new StreamReader(stream))
//                     {
//                         DataToLoad = reader.ReadToEnd();
//                     }
//                 }
//
//                 //Deserialize the file from Json back to the Object
//                loadedPlayerData = JsonUtility.FromJson<PlayerInformation> (DataToLoad);
//             }
//             catch (Exception e)
//             {
//                 Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
//             }
//         }
//         else
//         {
//             //Error occurs here
//             loadedPlayerData = new PlayerInformation();
//         }
//         loadedPlayerData.SendPlayerInfo();
//         return loadedPlayerData;
//     }
//
//     public void CheckIfFileExistsOnStart()
//     {
//         //If the File Exists already
//        if(File.Exists(Path.Combine(playerDataDirPath, playerDataFileName)))
//        {
//             LoadPlayerInformation();
//        }
//        else if(!File.Exists(Path.Combine(playerDataDirPath, playerDataFileName)))
//        {
//             SavePlayerInformation();
//        }
//     }
//     
//     // public void DeleteDataFileByName<T>() where T : ISaveData
//     // {
//     //     var typeName = typeof(T).FullName;
//     //     var path = $"{Application.persistentDataPath}/{typeName}.Save";
//     //
//     //     if (HasData(path))
//     //     {
//     //         File.Delete(path);
//     //     }
//     // }
//     //
//     // public void ClearAllDataInAppPath()
//     // {
//     //     var path = Application.persistentDataPath;
//     //     var files = Directory.GetFiles(path);
//     //
//     //     foreach (var fileName in files)
//     //     {
//     //         if (fileName.Contains(""))
//     //         {
//     //             File.Delete(fileName);
//     //         }
//     //     }
//     // }
//     //     
//     // public void SaveData(ISaveData saveData)
//     // {
//     //     var typeName = saveData.GetType().FullName;
//     //     var savePath = $"{Application.persistentDataPath}/{typeName}.Save";
//     //     var dataText = JsonConvert.SerializeObject(saveData);
//     //     File.WriteAllText(savePath, dataText);
//     // }
//     //     
//     // public T LoadData<T>() where T : ISaveData
//     // {
//     //     var typeName = typeof(T).FullName;
//     //     var loadPath = $"{Application.persistentDataPath}/{typeName}.Save";
//     //     if (HasData(loadPath))
//     //     {
//     //         var dataLoaded = File.ReadAllText(loadPath);
//     //         return JsonConvert.DeserializeObject<T>(dataLoaded);
//     //     }
//     //     return default;
//     // }
//     //
//     // /// <summary>
//     // /// Load Data, if null will create new instance
//     // /// Also Save the new created data
//     // /// using reflection, should be used only when game is loaded
//     // /// </summary>
//     // /// <typeparam name="T"></typeparam>
//     // /// <returns></returns>
//     // public T LoadDataAndCreateIfNull<T>() where T : ISaveData
//     // {
//     //     var data = LoadData<T>();
//     //
//     //     if (data == null)
//     //     {
//     //         data = Activator.CreateInstance<T>();
//     //         data.SaveData();
//     //     }
//     //
//     //     return data;
//     // }
//     //
//     // public bool HasData<T>() where T : ISaveData
//     // {
//     //     var typeName = typeof(T).FullName;
//     //     var loadPath = $"{Application.persistentDataPath}/{typeName}.Save";
//     //     return HasData(loadPath);
//     // }
//     //     
//     // public bool HasData(string path)
//     // {
//     //     return File.Exists(path);
//     // }
// }
//
// //This class is needed because the information that loads from the JSON seems to struggle with using PlayerData
// [System.Serializable]
// public class PlayerInformation
// {
//     public Inventory inventoryRef;
//     public List<StoredItem> inventoryItemRef;
//
//     public PlayerInformation()
//     {
//         inventoryRef = Inventory.Instance;
//         inventoryItemRef = Inventory.Instance.StoredItems;
//     }
//
//     public void SendPlayerInfo()
//     {
//         Inventory.Instance = inventoryRef;
//         Inventory.Instance.StoredItems = inventoryItemRef;
//     }
// }
//
// public interface ISaveData
// {
//         
// }
//
// public static class SaveExtensions
// {
//     public static void SaveData(this ISaveData saveData)
//     {
//        // SavingAndLoading.Instance.SaveData(saveData);
//     }
// }