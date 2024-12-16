using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument m_Doc;
    private VisualElement m_Root;
    private Button m_Play;
    private Button m_Exit;
    private Button m_Delete;

    void Start()
    {
        m_Doc = GetComponent<UIDocument>();
        m_Root = m_Doc.rootVisualElement;
        m_Play = m_Root.Q<Button>("Play");
        m_Delete = m_Root.Q<Button>("Delete");
        m_Exit = m_Root.Q<Button>("Exit");
        
        m_Play.RegisterCallback<ClickEvent>(PlayPressed);
        m_Delete.RegisterCallback<ClickEvent>(DeletePressed);
        m_Exit.RegisterCallback<ClickEvent>(ExitPressed);
    }

    private void DeletePressed(ClickEvent evt)
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

    void PlayPressed(ClickEvent evt)
    {
#if UNITY_WEBGL
        StartCoroutine(LoadMainLevelCoroutine());
#else
            LoadMainLevelAsync();
#endif
    }

#if UNITY_WEBGL
    private IEnumerator LoadMainLevelCoroutine()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            // run loading animation
            yield return null;
        }
        SceneManager.UnloadSceneAsync(2);
    }
#else
    private async Task LoadMainLevelAsync()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            // run loading animation
            await Task.Yield();
        }
        SceneManager.UnloadSceneAsync(2);
    }
#endif

    void ExitPressed(ClickEvent evt)
    {
        Application.Quit();
    }
}