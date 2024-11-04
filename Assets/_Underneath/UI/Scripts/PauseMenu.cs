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
        SceneManager.LoadScene("MainMenu");
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
                break;
            case DisplayStyle.None:
                m_Root.style.display = DisplayStyle.Flex;
                CurfewTimer.Instance.PauseTimer();
                break;
        }
    }
}
