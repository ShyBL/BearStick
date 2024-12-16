using System.Collections;
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

    void Start()
    {
        m_Doc = GetComponent<UIDocument>();
        m_Root = m_Doc.rootVisualElement;
        m_Play = m_Root.Q<Button>("Play");
        m_Exit = m_Root.Q<Button>("Exit");
        
        m_Play.RegisterCallback<ClickEvent>(PlayPressed);
        m_Exit.RegisterCallback<ClickEvent>(ExitPressed);
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