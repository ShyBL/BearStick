using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument m_Doc;
    private VisualElement m_Root;
    private Button m_Continue;
    private Button m_Exit;
    private Button m_MenuButton;
    public UIDocument Hud;

    // Start is called before the first frame update
    void Start()
    {
        m_Doc = GetComponent<UIDocument>();
        m_Root = m_Doc.rootVisualElement;
        m_Continue = m_Root.Q<Button>("Continue");
        m_Exit = m_Root.Q<Button>("Exit");
        m_MenuButton = Hud.rootVisualElement.Q<Button>("Menu");

        Player.Instance.playerInput.onPauseMenu += PauseToggle;
        
        m_Continue.RegisterCallback<ClickEvent>(ContinueButton);
        m_Exit.RegisterCallback<ClickEvent>(ExitButton);
        m_MenuButton.RegisterCallback<ClickEvent>(MenuButton);

        m_Root.style.display = DisplayStyle.None;
    }

    void ExitButton(ClickEvent evt)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
#if UNITY_WEBGL
        StartCoroutine(LoadMainLevelCoroutine());
#else
        LoadMainLevelAsync();
#endif
    }
    

    private IEnumerator LoadMainLevelCoroutine()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            // run loading animation
            yield return null;
        }
        SceneManager.UnloadSceneAsync(3);
    }

    private async Task LoadMainLevelAsync()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            // run loading animation
            await Task.Yield();
        }
        SceneManager.UnloadSceneAsync(3);
    }

    void ContinueButton(ClickEvent evt)
    {
        PauseToggle();
    }

    void MenuButton(ClickEvent evt)
    {
        PauseToggle();
    }

    void PauseToggle()
    {
        switch (m_Root.resolvedStyle.display)
        {
            case DisplayStyle.Flex:
                m_Root.style.display = DisplayStyle.None;
                CurfewTimer.Instance.ResumeTimer();
                Time.timeScale = 1;
                
                break;
            case DisplayStyle.None:
                m_Root.style.display = DisplayStyle.Flex;
                CurfewTimer.Instance.PauseTimer();
                Time.timeScale = 0;
                
                break;
        }
    }
}