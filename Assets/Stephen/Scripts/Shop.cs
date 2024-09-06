using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    UIDocument doc;
    VisualElement m_Root;

    void Start()
    {
        doc = GetComponent<UIDocument>();
        m_Root = doc.rootVisualElement;

        m_Root.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            m_Root.style.display = DisplayStyle.Flex;
        if (Input.GetKeyUp(KeyCode.E))
            m_Root.style.display = DisplayStyle.None;
    }
}
