using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument m_Doc;
    private VisualElement m_Root;
    // Start is called before the first frame update
    void Start()
    {
        m_Doc = GetComponent<UIDocument>();
        m_Root = m_Doc.rootVisualElement;
        m_Root.style.display = DisplayStyle.None;

        Player.Instance.playerInput.onPauseMenu += PauseToggle;
    }

    void PauseToggle()
    {
        switch (m_Root.resolvedStyle.display)
        {
            case DisplayStyle.Flex:
                m_Root.style.display = DisplayStyle.None;
                break;
            case DisplayStyle.None:
                m_Root.style.display = DisplayStyle.Flex;
                break;
        }
    }
}
