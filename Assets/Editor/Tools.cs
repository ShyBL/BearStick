using System.IO;
using UnityEditor;
using UnityEngine;

public class Tools : MonoBehaviour
{
    [MenuItem("Underneath/Tests/Delete Saves")]
    public static void ClearAllData()
    {
        var path = Application.persistentDataPath;
        var files = Directory.GetFiles(path);

        foreach (var fileName in files)
        {
            if (fileName.Contains("PlayerData"))
            {
                Debug.Log($"Deleting {fileName}");
                File.Delete(fileName);
            }
        }
    }
}