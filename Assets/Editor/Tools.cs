using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
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

    [MenuItem("Underneath/Play")]
    public static void PlayFromBootstrapper()
    {
        var currentSceneName = EditorSceneManager.GetActiveScene().name;
        File.WriteAllText(".lastScene", currentSceneName);
        EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_Underneath/Scenes/Bootstrapper.unity");
        EditorApplication.isPlaying = true;
    }
    
    [MenuItem("Underneath/Load Last Edited Scene")]
    public static void ReturnToLastScene()
    {
        string lastScene = File.ReadAllText(".lastScene");
        EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_Underneath/Scenes/{lastScene}.unity");
    }
}