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
        var scene = SceneManager.LoadSceneAsync("MainLevel");
        if (scene.isDone)
        {
            Destroy(gameObject);
        }
    }

    void ExitPressed(ClickEvent evt)
    {
        Application.Quit();
    }
}
